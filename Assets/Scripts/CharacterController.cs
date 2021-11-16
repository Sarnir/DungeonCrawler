using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterController : MonoBehaviour, IDamageable
{
    [SerializeField] private CharacterData data;

    [Header("Stats")] [SerializeField] private int accuracy;

    [Header("Debug stuff")] [SerializeField]
    private float height;

    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxForce;
    [SerializeField] private float arrivingRadius;
    [SerializeField] protected float destinationRadius;
    [SerializeField] protected float interactionRadius;
    [SerializeField] private float desiredSeparation;

    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rigidbody2D;
    protected Animator animator;
    public Animator Animator => animator;

    public SpriteRenderer SpriteRenderer => spriteRenderer;
    public Rigidbody2D Rigidbody2D => rigidbody2D;

    public Vector2 Position => rigidbody2D.position;
    public Vector2 PositionWithHeight => rigidbody2D.position + new Vector2(0f, height);
    public float MaxSpeed => maxSpeed;
    public float MaxForce => maxForce;
    public Vector2 Velocity => rigidbody2D.velocity;
    public float ArrivingRadius => arrivingRadius;

    Attribute health;
    public Attribute Health {
        get
        {
            if (health == null)
                health = attributes[AttributeType.Health];

            return health;
        }
    }
    
    public int Accuracy => data.Accuracy;
    
    private Weapon weapon;

    private Attribute fatigue;
    public Attribute Fatigue => fatigue;

    public Action OnDeath { get; set; }

    protected IAction currentAction;

    private Dictionary<AttributeType, Attribute> attributes = new Dictionary<AttributeType, Attribute>();

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        weapon = new Weapon();
        
        health = GetComponent<Health>();
        health.Setup(data.MaxHealth, data.MaxHealth);

        /*foreach (var atData in data.attributes)
        {
            var newAt = gameObject.AddComponent<Attribute>();
            newAt.Setup(atData);
            attributes.Add(atData.Type, newAt);
        }*/
        
        health.OnValueDepleted += Die;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log(name +" is attacked!");
        if (health != null)
        {
            health.Change(-damage);
        }

        DamagePopup.Spawn(damage, PositionWithHeight);
    }

    public void Heal(int amount)
    {
        if (health != null)
        {
            health.Change(amount);
        }
    }

    private void Die()
    {
        Debug.LogError($"{name} has just died");
        OnDeath?.Invoke();
        ClearAction();
        animator.SetTrigger("Death");
        this.enabled = false;
    }

    public void DealDamageEvent()
    {
        if (currentAction is AttackAction attackAction)
        {
            attackAction.DealDamage(weapon.RollDamage());
            return;
        }
    }

    public void DeathEndEvent()
    {
        var corpse = CharacterCorpse.Spawn(spriteRenderer.sprite);
        corpse.transform.position = transform.position;
        Destroy(gameObject);
    }

    public void EndAttack()
    {
        if (currentAction is AttackAction)
            ClearAction();
    }

    private void Start()
    {
        OnInitialize();
    }

    protected virtual void OnInitialize()
    {
    }

    private void Update()
    {
        if (currentAction != null)
        {
            if (currentAction.Act())
            {
                ClearAction();
            }
        }

        OnUpdate();
    }

    protected virtual void OnUpdate()
    {
    }

    public void ApplyForce(Vector2 force)
    {
        rigidbody2D.velocity += force;
    }

    protected void SetAction(IAction action)
    {
        currentAction?.ActionCompleted();
        currentAction = action;
    }
    
    protected void ClearAction()
    {
        currentAction?.ActionCompleted();
        currentAction = null;
    }

    public void Stop()
    {
        rigidbody2D.velocity = Vector2.zero;
        animator.SetBool("IsMoving", false);
    }

    protected void Separate(Vector2[] positions)
    {
        var sum = new Vector2();
        int count = 0;
        foreach (var other in positions)
        {
            float d = Vector2.Distance(Position, other);
            if ((d > 0) && (d < desiredSeparation)) {
                Vector2 diff = Position - other;
                diff.Normalize();
                
                // What is the magnitude of the PVector pointing away from the other vehicle?
                // The closer it is, the more we should flee. The farther, the less.
                // So we divide by the distance to weight it appropriately.
                diff /= d;
                sum += diff;
                count++;
            }
        }
        
        if (count > 0) {
            sum /= count;
            sum.Normalize();
            sum *= maxSpeed;
            Vector2 steer = sum - rigidbody2D.velocity;
            steer = Vector2.ClampMagnitude(steer, maxForce);
            ApplyForce(steer);
        }
    }
}
