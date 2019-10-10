using UnityEngine;

public class PlayerDash : BaseAbility
{
    public float chiDashConsumption = 0.2f;
    public float chiDashDrain = 0.5f;

    [Header("TimeScale")]
    float dashTimeScale = 0.8f;

    [Header("Dash")] public float stopBeforeDash;
    // Dash
    [SerializeField]
    protected float dashSpeed, dashSpeedMultiplier, dashDuration;

    [SerializeField]
    protected AnimationCurve dashCurve;

    [SerializeField]
    protected internal AnimationCurve dashCameraCurve;
    [SerializeField]
    protected GameObject dashSphere;
    [SerializeField]
    protected bool canChargeDash = false;

    private Vector2 direction;
    private Vector2 dashVelocity = new Vector2(0, 0);
    private float currentDash;

    public override void Play()
    {
        dashSphere.SetActive(true);
        base.Play();
    }

    public override bool Run()
    {
        float progression = 1f - (currentDash / dashDuration);

        dashVelocity = direction * dashSpeed * dashCurve.Evaluate(progression);

        if (Input.GetButton("Fire3") && canChargeDash)
        {
            Controls.chi -= chiDashDrain * Time.deltaTime; // Needs Chi Manager
        }

        else
        {
            //direction = rb.velocity.normalized;
            currentDash -= Time.deltaTime;
        }

        return base.Run();
    }

    public override void Stop()
    {
        dashSphere.SetActive(false);

        dashVelocity *= 0f;
        base.Stop();
    }

    public virtual void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    public virtual float GetDuration()
    {
        return dashDuration;
    }

    public virtual float GetDashSpeedMultiplier()
    {
        return dashSpeedMultiplier;
    }
}
