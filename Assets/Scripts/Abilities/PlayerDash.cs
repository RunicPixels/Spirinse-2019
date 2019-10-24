using UnityEngine;

public class PlayerDash : BaseAbility
{
    // Gravity 
    [SerializeField] protected float gravityScale;

    // Speed
    [SerializeField] protected float dashSpeed;
    [SerializeField] protected float dashSpeedMultiplier;
    [SerializeField] protected float stopBeforeDash = 0.2f;
    [SerializeField] protected Vector2 dashVelocity = new Vector2(0, 0);
    
    // Curves
    [SerializeField] protected AnimationCurve dashCurve;
    [SerializeField] protected AnimationCurve dashCameraCurve;

    // GameObjects
    [SerializeField] protected GameObject dashSphere;
    
    public enum CurveType { Dash, Camera }

    private Vector2 direction;

    public float GetGravityScale => gravityScale;
    public float GetDashSpeed => dashSpeed;
    public float GetDashSpeedMultiplier => dashSpeedMultiplier;
    public float GetStopBeforeDash => stopBeforeDash;
    public Vector2 GetDashVelocity => dashVelocity;
    


    public override void Play()
    {
        dashSphere.SetActive(true);
        base.Play();
    }

    public override bool Run()
    {
        var progression = GetProgression();

        dashVelocity = direction * dashSpeed * dashCurve.Evaluate(progression);


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
        return duration;
    }

    public virtual float GetProgression()
    {
        return 1f - (currentDuration / duration);
    }

    public float GetCurveProgression(CurveType type)
    {
        var progression = GetProgression();
        switch(type)
        {
            case CurveType.Dash:
                return dashCurve.Evaluate(progression);
            case CurveType.Camera:
                return dashCameraCurve.Evaluate(progression);
            default:
                Debug.LogWarning("Invalid Dash Curve type Specified with " + type);
                return 0f;
        }
    }
}
