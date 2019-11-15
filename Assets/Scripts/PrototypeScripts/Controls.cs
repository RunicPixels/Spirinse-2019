using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Spirinse.Interfaces;
using MEC;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class Controls : MonoBehaviour
{
    
    [Header("Visuals")] 
    public ParticleSystem trailSystem;

    [Range(0f, 1f)] public static float chi =1f;
    [Header("Chi")]
    // Chi
    public float chiRechargeRate = 0.5f;
    public float chiAttackCost = 0.1f;
    public float chiDrainAttackMode = 0.5f;
    public float chiDashConsumption = 0.2f;
    public float chiGravity = 5f;

    [Header("TimeScale")]
    private float attackTimeScale = 0.4f;
    private float dashTimeScale = 0.8f;
    
    [Header("Moving")]
    public float hAcceleration;
    public float vAcceleration, speed;
    public float altitudeModifier = 2;
    public float altitudeVelocity = 1;
    public enum DirectionMode { Velocity, Mouse, Arrows};

    [Header("Dash")]
    public Dash dashAbility;

    [Header("Attacks")] 
    // Laser
    public bool laser;
    public float laserLength = 15f;
    public LayerMask laserMask;
    public ParticleSystem laserSystem;

    // Bullet
    [Space(10)]
    public bool bullets;
    public Bullet bulletPrefab;
    public float fireRate = 0.12f;

    // Charged Bullet
    [Space(10)] public bool chargedBullet;

    // Melee
    [Space(10)] public bool meleeAttack;
    public BaseAttack[] meleeAttackPrefab;
    
    // Privates 
    private LineRenderer lineRenderer;
    private Rigidbody2D rb;

    private float powVelocity;
    private Vector2 direction;
    private float speedMultiplier = 1f;
    private Vector2 dashVelocity = new Vector2(0,0);

    private bool dashing = false;
    private bool goDoDash = false;

    public Action<bool> playerMovementAction; // Move this to input manager.
    public Action<float> useDashAction;
    // Start is called before the first frame update
    private void Start()
    {
        if (meleeAttack)
        {
            foreach (var melee in meleeAttackPrefab)
            {
                melee.gameObject.SetActive(false);
            }
        }

        //meleeAttackPrefab.ToList().MMShuffle();
        //meleeAttackPrefab.ToList().OrderBy((x => x.GetComponent<ControllerColliderHit>().transform.position.y)).Take(3);
        
        lineRenderer = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        if (Input.GetButtonDown("Fire3") && dashing == false && chiDashConsumption < chi)
        {
            StartCoroutine(_Dashing(direction.normalized));
        }

        if (chi < 1f)
        {
            chi += chiRechargeRate * Time.deltaTime;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Time.timeScale = 1f;
            StartCoroutine(_Shooting());
        }
    }

    private void FixedUpdate()
    {
        CalculateAltitudeVelocity();

        LimitVelocity();

        rb.AddForce(CalculateVelocity());

        rb.gravityScale = CalculateGravity();

        
    }
    private void CalculateAltitudeVelocity()
    {
        if (rb.velocity.y < -0.1f)
        {
            altitudeVelocity -= rb.velocity.y * 0.1f * Time.fixedDeltaTime;
        }
        else if (altitudeVelocity > 1f)
        {
            if (dashing)
            {
                altitudeVelocity += rb.velocity.magnitude * 0.01f * Time.fixedDeltaTime;
            }
            else
            {
                altitudeVelocity -= rb.velocity.y * 0.06f * Time.fixedDeltaTime;
                
            }
            altitudeVelocity -= (0.5f + altitudeVelocity * 0.38f) * Time.fixedDeltaTime;

        }
        if(rb.velocity.magnitude < speed)
        {
            altitudeVelocity *= 0.8f;
        }
        if (altitudeVelocity < 1f) altitudeVelocity = 1f;

        powVelocity = Mathf.Pow(altitudeVelocity, 0.33f);
    }

    private Vector2 CalculateVelocity()
    {
        var velocity = Time.fixedDeltaTime * 60 * speedMultiplier * new Vector2(direction.x * hAcceleration * powVelocity, direction.y * vAcceleration * powVelocity);

        //Debug.Log(velocity.magnitude);
        if (velocity.magnitude > 1f)
        {
            playerMovementAction.Invoke(true);
        }
        else
        {
            playerMovementAction.Invoke(false);
        }

        if (dashing)
        {
            velocity *= dashAbility.GetDashSpeedMultiplier;
            velocity += dashAbility.GetDashVelocity;
        }

        return velocity;
    }

    private float CalculateGravity()
    {
        var gravity = Mathf.Max(0f, (1f - chi) * chiGravity) * 2;
        if (dashing)
        {
            gravity = dashAbility.GetGravityScale();
        }
        return gravity;
    }

    private void LimitVelocity()
    {
        var speedLimit = speed * altitudeVelocity;
        if(dashing)
        {
            speedLimit *= 1+dashAbility.GetDashVelocityMagnitude();
        }
        if (rb.velocity.magnitude > speedLimit)
        {
            rb.velocity = rb.velocity.normalized * Mathf.Lerp(rb.velocity.magnitude, speedLimit, 0.33f);
        }
    }

    /*    private void SomeFunction(bool test, params int[] input)
    {
        SomeFunction(true, 1,2,3,4);
    }*/
    private IEnumerator<float> _Dashing(Vector2 direction)
    {
        useDashAction?.Invoke(dashAbility.GetDuration());
        goDoDash = false;
        var trailSystemMain = trailSystem.main; // NEEDS particle system Manager
        var tLife = trailSystem.main.startLifetime;

        //var trailSystemRibbon = trailSystem.trails;

        dashAbility.SetDirection(direction);
        
        dashAbility.Play();

        var currentDash = dashAbility.GetDuration();

        Camera camera = Camera.main; // Needs FOV Manager
        var fov = camera.fieldOfView;

        chi -= chiDashConsumption;
        dashing = true;
        
        rb.velocity *= dashAbility.GetStopBeforeDash;

        while (dashAbility.Run())
        {
            camera.fieldOfView = fov + (30 * dashAbility.GetCurveProgression(Dash.CurveType.Camera));

            // Needs Particle System Manager
            trailSystemMain.startLifetime = new ParticleSystem.MinMaxCurve(tLife.Evaluate(0) + currentDash + currentDash * dashAbility.GetDuration(), tLife.Evaluate(1f) + currentDash + currentDash * dashAbility.GetDuration() + 2f);
            Time.timeScale = dashTimeScale;

            yield return Time.deltaTime;
        }

        dashAbility.Stop();

        speedMultiplier = 1f;
        camera.fieldOfView = fov;

        Time.timeScale = 1f;

        trailSystemMain.startLifetime = tLife;
        
//      if (rb.velocity.magnitude > speed)
//      {
//          rb.velocity = rb.velocity.normalized * speed;
//      }

        dashing = false;
    }

    private IEnumerator<float> _Shooting()
    {
        float fov;
        Camera camera = Camera.main;
        fov = camera.fieldOfView;

        var bulletCD = 0.1f;
        var charge = 0.5f;

        chi -= chiAttackCost;

        DoMelee();

        while (Input.GetButton("Fire1"))
        {
            var direction = rb.velocity.normalized;
            camera.fieldOfView += Time.unscaledDeltaTime * 1f;
            chi -= chiDrainAttackMode * Time.unscaledDeltaTime;

            var position = transform.position;

            // Laser Code
            if (laser)
            {
                lineRenderer.SetPosition(0, position);

                RaycastHit2D hit = Physics2D.Raycast(position, direction, laserLength, laserMask);

                if (hit.collider != null)
                {
                    lineRenderer.SetPosition(1, hit.point);
                    if (!laserSystem.isPlaying) laserSystem.Play();

                }
                else
                {
                    lineRenderer.SetPosition(1, position + (Vector3) rb.velocity.normalized * laserLength);
                    laserSystem.Stop();

                }
                laserSystem.transform.position = lineRenderer.GetPosition(1);

            }
            else
            {
                laserSystem.Stop();
            }

            // Bullets Code

            if (bullets && bulletCD < 0f)
            {
                Bullet bullet = Instantiate(bulletPrefab, transform);
                bullet.Init(direction);
                bullet.transform.SetParent(null);
                bulletCD = fireRate;
            }
            else
            {
                bulletCD -= Time.unscaledDeltaTime;
            }

            // Charge Bullet Code

            charge += Time.unscaledDeltaTime;

            // Melee Attack
            DoMelee();

            if (chi < 0f) break;

            yield return Time.unscaledDeltaTime;
        }

        if (chargedBullet)
        {
            Bullet bullet = Instantiate(bulletPrefab, transform);
            bullet.Init(rb.velocity.normalized);
            bullet.transform.localScale = bullet.transform.localScale * charge;
            bullet.transform.SetParent(null);
        }

        foreach (var melee in meleeAttackPrefab)
        {
            if (melee.gameObject.activeSelf)
            {
                melee.gameObject.SetActive(false);
            }
        }

        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);
        //Time.timeScale = 1f;
        camera.fieldOfView = fov;
        laserSystem.Stop();
    }

    private void DoMelee()
    {
        if (!meleeAttack) return;
        foreach (var melee in meleeAttackPrefab)
        {
            if (!melee.CompareTag("Selected")) continue;
            melee.gameObject.SetActive(true);
            //melee.GetComponent<Animator>().StartPlayback();

            Vector2 v = rb.velocity;
            var angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            melee.transform.transform.parent.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
