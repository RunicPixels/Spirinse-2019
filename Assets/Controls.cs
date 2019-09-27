using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class Controls : MonoBehaviour
{
    [Header("Chi")]
    [Range(0f, 1f)] public float chi =1f;

    public float chiRechargeRate = 0.5f;

    public Vector2 velocity;

    [Header("Moving")] public float hAcceleration;
    public float vAcceleration, speed;

    [Header("Dash")] public float stopBeforeDash;
    public float dashAcceleration, dashSpeed, dashDuration;
    public AnimationCurve dashCurve, dashCameraCurve;

    private Rigidbody2D rb;
    private bool dashing = false;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        velocity = new Vector2(Input.GetAxis("Horizontal") * hAcceleration, Input.GetAxis("Vertical") * vAcceleration) * 60 * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashing == false)
        {
            rb.velocity *= stopBeforeDash;
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
        rb.gravityScale = Mathf.Max(0f, 1f - chi);

        if (rb.velocity.magnitude > speed && dashing == false)
        {
            rb.velocity = rb.velocity.normalized * speed;
        }
        else if (rb.velocity.magnitude > dashSpeed && dashing == true)
        {
            rb.velocity = rb.velocity.normalized * speed;
            rb.gravityScale = 0f;
        }
        
    }

    private IEnumerator<float> _Dashing(Vector2 direction)
    {
        float fov;
        Camera camera = Camera.main;
        fov = camera.fieldOfView;
        float currentDash = dashDuration;
        chi -= 0.25f;
        dashing = true;
        while (currentDash > 0f)
        {
            Time.timeScale = 0.8f;
            float progression = 1f - (currentDash/ dashDuration);
            velocity = direction * dashSpeed * dashCurve.Evaluate(progression);
            camera.fieldOfView = fov + (30 * dashCameraCurve.Evaluate(progression));
            currentDash -= Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }

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
        while (Input.GetButton("Fire1"))
        {
            Time.timeScale = 0.25f;
            camera.fieldOfView += Time.unscaledDeltaTime * 5f;
            chi -= 0.5f * Time.unscaledDeltaTime;
            yield return Timing.WaitForOneFrame;
        }

        Shoot(Vector2.right);
        Time.timeScale = 1f;
        camera.fieldOfView = fov;
    }

    public void Shoot(Vector2 direction) 
    {
        Debug.Log("Kaboom");
    }
}
