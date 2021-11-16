using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackAction : BaseAction
{
    private ITarget target;
    private float destinationRange;
    private bool attackStarted;

    private MoveAction moveAction;

    public ITarget Target => target;
    
    public AttackAction(CharacterController actor, IDamageable target, float destinationRange) : base(actor)
    {
        target.OnDeath += OnTargetDeath;
        this.target = target;
        this.destinationRange = destinationRange;
        attackStarted = false;
    }

    public override void ActionCompleted()
    {
        if (target is IDamageable damageable)
            damageable.OnDeath -= OnTargetDeath;
    }

    public override bool Act()
    {
        if (target == null)
            return true;
        
        if(actor == null)
            Debug.LogError($"Actor {actor.name} is null");
        
        Vector2 desired = target.Position - actor.Position;
        float distance = desired.magnitude;

        if (!attackStarted)
        {
            if (distance < destinationRange)
            {
                // attack here
                actor.Animator.SetTrigger("Attack");
                attackStarted = true;
                actor.StartCoroutine(ShowCurrentClipLength());
                actor.SpriteRenderer.flipX = desired.x < 0f;
                actor.Stop();
            }
            else
            {
                if (moveAction == null)
                    moveAction = new MoveAction(actor, target, destinationRange);

                moveAction.Act();
            }
        }

        return false;
    }

    IEnumerator ShowCurrentClipLength()
    {
        // workaround, without it returned length might be incorrect
        yield return new WaitForEndOfFrame();
        
        float attackAnimLength = actor.Animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(attackAnimLength);
        actor.EndAttack();
    }
    
    IEnumerator AttackCoroutine()
    {
        //AnimationController cccc;
        
        yield return null;
    }

    void OnTargetDeath()
    {
        // todo: action is completed at this point
        Debug.LogError("OnTargetDeath");
        ActionCompleted();
        target = null;
    }

    public void DealDamage(int damage)
    {
        if (target is IDamageable damageable)
        {
            // Perform an attack (accuracy) test
            var hitChance = (1f + actor.Accuracy) / (2f + actor.Accuracy);

            if (Random.value <= hitChance)
            {
                damageable.TakeDamage(damage);
            }
            else
            {
                DamagePopup.SpawnMiss(actor.PositionWithHeight);
            }
        }
    }
}

