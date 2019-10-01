using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class Controls : MonoBehaviour
{
    public Vector2 velocity;
    [Header("Visuals")] 
    public ParticleSystem trailSystem;

    public enum DirectionMode { Velocity, Mouse, Arrows};

    [Range(0f, 1f)] public static float chi =1f;
    [Header("Chi")]
    public float chiRechargeRate = 0.5f;
    public float chiAttackCost = 0.1f;
    public float chiDrainAttackMode = 0.5f;
    public float chiDashConsumption = 0.2f;
    public float chiGravity = 5f;

    [Header("TimeScale")] private float attackTimeScale = 0.8f;
    float dashTimeScale = 0.8f;
    
    [Header("Moving")] public float hAcceleration;
    public float vAcceleration, speed;

    [Header("Dash")] public float stopBeforeDash;
    public float dashSpeed, dashDuration;
    public AnimationCurve dashCurve, dashCameraCurve;


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

    private float currentDashcooldown = 0f;
    


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
        
        lineRenderer = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        velocity = new Vector2(Input.GetAxis("Horizontal") * hAcceleration, Input.GetAxis("Vertical") * vAcceleration) * 60 * Time.deltaTime;
        if (Input.GetButtonDown("Fire3") && dashing == false && chiDashConsumption < chi)
        {
            StartCoroutine(_Dashing(velocity.normalized));
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
        rb.AddForce(velocity);
        rb.gravityScale = Mathf.Max(0f, (1f - chi) * chiGravity);

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
        
        rb.velocity *= stopBeforeDash;
        var trailSystemRibbon = trailSystem.trails;

        while (currentDash > 0f)
        {
            trailSystemMain.startLifetime = new ParticleSystem.MinMaxCurve(tLife.Evaluate(0)+currentDash + currentDash * dashDuration,tLife.Evaluate(1f)+currentDash +currentDash * dashDuration + 2f);
            Time.timeScale = dashTimeScale;
            float progression = 1f - (currentDash/ dashDuration);
            velocity = direction * dashSpeed * dashCurve.Evaluate(progression);
            camera.fieldOfView = fov + (30 * dashCameraCurve.Evaluate(progression));
            currentDash -= Time.deltaTime;
            yield return Time.deltaTime;
        }
        
        trailSystemMain.startLifetime = tLife;
        

        if (rb.velocity.magnitude > speed)
        {
            rb.velocity = rb.velocity.normalized * speed;
        }

        Time.timeScale = 1f;

        camera.fieldOfView = fov;

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
            camera.fieldOfView += Time.unscaledDeltaTime * 5f;
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
