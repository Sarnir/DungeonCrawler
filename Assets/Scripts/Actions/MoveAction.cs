using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class MoveAction : BaseAction
{
    private bool arrivedAtTarget;
    private ITarget target;
    private float destinationRange;
    
    public MoveAction(CharacterController actor, ITarget target, float destinationRange) : base(actor)
    {
        arrivedAtTarget = false;
        this.target = target;
        this.destinationRange = destinationRange;
    }

    public override bool Act()
    {
        if (arrivedAtTarget)
        {
            actor.Stop();
            return true;
        }

        Arrive(target.Position);
        
        CheckIfCollision();
        
        actor.Animator.SetBool("IsMoving", actor.Velocity.magnitude > 0.1f);

        Vector2 desired = target.Position - actor.Position;
        float distance = desired.magnitude;

        if (distance < destinationRange)
        {
            arrivedAtTarget = true;
            actor.Stop();
            return true;
        }

        return false;
    }

    public void UpdateTarget(ITarget newTarget)
    {
        target = newTarget;
    }

    protected void Arrive(Vector2 target)
    {
        Vector2 desired = target - actor.Position;
        float distance = desired.magnitude;
        
        if(distance < destinationRange)
            return;

        desired.Normalize();
        
        actor.SpriteRenderer.flipX = desired.x < 0f;

        // find way to ease movement exponentially(or any other way)
        desired *= actor.MaxSpeed * Mathf.Clamp(distance / actor.ArrivingRadius, 0.1f, 1f);
        
        // Reynolds’s formula for steering force
        Vector2 steer = desired - actor.Rigidbody2D.velocity;
        steer = Vector2.ClampMagnitude(steer, actor.MaxForce);

        // apply force
        actor.Rigidbody2D.velocity += steer;
    }

    void CheckIfCollision()
    {
        if(actor.Rigidbody2D.velocity.magnitude == 0f)
            return;
        
        var ray = new Ray2D(actor.Position, actor.Rigidbody2D.velocity);
        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, 0.1f);
            
        Debug.DrawLine(ray.origin, ray.origin + ray.direction, Color.green);
        
        // todo: instead of for linq method like where might be better
        for (int i = 0; i < hits.Length; i++)
        {
            var hit = hits[i];
            if (hit.transform != null && hit.transform != actor.transform
            && !hit.collider.isTrigger)
            {
                var rayDirection = ray.direction;
                var direction = rayDirection.x < 0 ? Vector2.left : Vector2.right;
                
                var horizontalRay = new Ray2D(actor.Position, direction);
                var horizontalHits = Physics2D.RaycastAll(horizontalRay.origin, horizontalRay.direction, 0.2f);

                bool anyHHit = horizontalHits.Any(hHit => hHit.transform != null && hHit.transform != actor.transform
                    && !hHit.collider.isTrigger);
                
                direction = rayDirection.y < 0 ? Vector2.down : Vector2.up;
                var verticalRay = new Ray2D(actor.Position, direction);
                var verticalHits = Physics2D.RaycastAll(verticalRay.origin, verticalRay.direction, 0.2f);

                bool anyVHit = verticalHits.Any(vHit => vHit.transform != null && vHit.transform != actor.transform
                                                                               && !vHit.collider.isTrigger);

                var velocity = actor.Rigidbody2D.velocity;
                actor.Rigidbody2D.velocity = new Vector2(anyHHit ? 0f : velocity.x, anyVHit ? 0f : velocity.y);
                
                if(!anyHHit && !anyVHit)
                    actor.Stop();
                
                return;
            }
        }
    }
}

