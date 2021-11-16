using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractAction : BaseAction
{
    private ITarget target;
    private float destinationRange;
    private bool interactionStarted;

    private MoveAction moveAction;
    
    public InteractAction(CharacterController actor, ITarget target, float destinationRange) : base(actor)
    {
        this.target = target;
        this.destinationRange = destinationRange;
        interactionStarted = false;
    }

    public override bool Act()
    {
        Vector2 desired = target.Position - actor.Position;
        float distance = desired.magnitude;

        if (!interactionStarted && distance < destinationRange)
        {
            actor.Stop();
            // interact here
            if (target is IInteractable interactable)
            {
                interactable.Interact(actor);
                return true;
            }
            interactionStarted = true;
        }
        else
        {
            if (moveAction == null)
                moveAction = new MoveAction(actor, target, destinationRange);

            moveAction.Act();
        }

        return false;
    }
}

