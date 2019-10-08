using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class PrototypeEnemy : MonoBehaviour, IDamagable
{
    public float speed;
    public float health = 5;
    public Animator animator;

    public bool cured = false;
    
    private Vector3 direction;

    private Rigidbody2D rb;

    private static readonly int Cure1 = Animator.StringToHash("Cure");

    private bool flipped;

    public float iFrames = 0f;
    public float stunned = 0;

    public ParticleSystem hitParticles;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (iFrames > 0f)
        {
            iFrames -= Time.fixedDeltaTime;
        }
        
        if (stunned > 0f)
        {
            stunned -= Time.fixedDeltaTime;
            goto StunJump;
        }
        
        direction = (Meditator.Instance.transform.position - transform.position).normalized;
        
        rb.velocity = direction * speed;
        
        if (cured)
        {
            rb.velocity += Vector2.up * 1.5f;
            
        }

        StunJump:
        
        flipped = rb.velocity.x < 0;

        
        var xScale = flipped ? 2f : -2f;

        transform.localScale = new Vector3(2f, xScale, 2f);


        var v = -rb.velocity;
        var angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void TakeDamage(float damage)
    {
        if (iFrames > 0f) return;
        health -= damage;

        rb.velocity = direction * -speed;
        
        iFrames = 0.25f;
        stunned = 0.6f;
        
        hitParticles.Play();
        if (health < 0)
        {
            Cure();
        }
    }

    private void Cure()
    {
        cured = true;
        animator.SetTrigger(Cure1);
        transform.gameObject.layer = LayerMask.NameToLayer("NoCollision");
    }

   public void Destroy()
    {
        Destroy(gameObject);
    }

   private void OnTriggerStay2D(Collider2D other)
   {
       if (iFrames > 0) return;
       var attack = other.gameObject.GetComponent<IAttack>();

       if (attack != null)
       {
           TakeDamage(attack.DoAttack());
       }
   }

}
