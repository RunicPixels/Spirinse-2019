using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using DefaultNamespace;
using MEC;
using MoreMountains.Tools;
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
    public float chiDashDrain = 0.5f;
    public float chiGravity = 5f;

    [Header("TimeScale")] private float attackTimeScale = 0.8f;
    float dashTimeScale = 0.8f;
    
    [Header("Moving")] public float hAcceleration;
    // Moving
    public float vAcceleration, speed;
    public Vector2 velocity;
    public enum DirectionMode { Velocity, Mouse, Arrows};
    
    [Header("Dash")] public float stopBeforeDash;
    // Dash
    public float dashSpeed, dashSpeedMultiplier, dashDuration;
    public AnimationCurve dashCurve, dashCameraCurve;
    public GameObject dashSphere;
    public bool canChargeDash = false;

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
    public GameObject[] meleeAttackPrefab;
    
    // Privates 
    private LineRenderer lineRenderer;
    private Rigidbody2D rb;
    private bool dashing = false;
    private bool goDoDash = false;
    
    private Vector2 direction;
    private float speedMultiplier = 1f;
    private Vector2 dashVelocity = new Vector2(0,0);


    // Start is called before the first frame update
    private void Start()
    {
        if (meleeAttack)
        {
            foreach (var melee in meleeAttackPrefab)
            {
                melee.SetActive(false);
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
            goDoDash = true;
        }

        if (chi < 1f)
        {
            chi += chiRechargeRate * Time.deltaTime;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(_Shooting());
        }
    }

    private void FixedUpdate()
    {
        velocity = Time.fixedDeltaTime * 60 *  speedMultiplier * new Vector2(direction.x * hAcceleration, direction.y * vAcceleration);
        rb.AddForce(velocity + dashVelocity);
        rb.gravityScale = Mathf.Max(0f, (1f - chi) * chiGravity);

        if (goDoDash)
        {
            StartCoroutine(_Dashing(velocity.normalized));
            goDoDash = false;
        }
        
        if (rb.velocity.magnitude > speed && dashing == false)
        {
            rb.velocity = rb.velocity.normalized * Mathf.Lerp( rb.velocity.magnitude, speed, 0.98f);
        }
        else if (rb.velocity.magnitude > dashSpeed && dashing == true)
        {
            rb.velocity = rb.velocity.normalized * dashSpeed;
            rb.gravityScale = 0f;
        }
        
    }

    void SomeFunction(bool test, params int[] input)
    {
        SomeFunction(true, 1,2,3,4);
    }
    private IEnumerator<float> _Dashing(Vector2 direction)
    {
        var trailSystemMain = trailSystem.main;
        var tLife = trailSystem.main.startLifetime;
        float fov;
        Camera camera = Camera.main;
        fov = camera.fieldOfView;
        float currentDash = dashDuration;
        chi -= chiDashConsumption;
        dashing = true;
        dashSphere.SetActive(true);
        
        rb.velocity *= stopBeforeDash;
        var trailSystemRibbon = trailSystem.trails;

        while (currentDash > 0f)
        {
            trailSystemMain.startLifetime = new ParticleSystem.MinMaxCurve(tLife.Evaluate(0)+currentDash + currentDash * dashDuration,tLife.Evaluate(1f)+currentDash +currentDash * dashDuration + 2f);
            Time.timeScale = dashTimeScale;
            float progression = 1f - (currentDash/ dashDuration);
            speedMultiplier = dashSpeedMultiplier;
            dashVelocity = direction * dashSpeed * dashCurve.Evaluate(progression);
            camera.fieldOfView = fov + (30 * dashCameraCurve.Evaluate(progression));
            
            if (Input.GetButton("Fire3") && canChargeDash)
            {
                chi -= chiDashDrain * Time.deltaTime;
            }
            else
            {
                direction = rb.velocity.normalized;
                currentDash -= Time.deltaTime;
            }
            yield return Time.deltaTime;
        }

        dashVelocity *= 0f;
        speedMultiplier = 1f;
        
        trailSystemMain.startLifetime = tLife;
        

//        if (rb.velocity.magnitude > speed)
//        {
//            rb.velocity = rb.velocity.normalized * speed;
//        }

        Time.timeScale = 1f;

        camera.fieldOfView = fov;

        dashSphere.SetActive(false);
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

        if (meleeAttack)
        {
            foreach (var melee in meleeAttackPrefab)
            {
                if (melee.CompareTag("Selected"))
                {
                    melee.SetActive(true);
                    melee.GetComponentInChildren<Animator>().Play(0);
                }
                
            }
        }

        while (Input.GetButton("Fire1"))
        {
            var direction = rb.velocity.normalized;
            Time.timeScale = attackTimeScale;
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

            if (chi < 0f) break;

            yield return Time.unscaledDeltaTime;

            // Melee Attack

            if (meleeAttack)
            {
                foreach (var melee in meleeAttackPrefab)
                {
                    if (melee.CompareTag("Selected") && melee.activeSelf)
                    {
                        Vector2 v = rb.velocity;
                        var angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
                        melee.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    }

                }
            }

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
            if (melee.activeSelf)
            {
                melee.SetActive(false);
            }
        }

        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);
        //Shoot(direction);
        Time.timeScale = 1f;
        camera.fieldOfView = fov;
        laserSystem.Stop();
    }

//    public void Shoot(Vector2 direction) 
//    {
//        
//        Debug.Log("Kaboom");
//    }
}
