using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : IAction
{
    protected CharacterController actor;

    public BaseAction(CharacterController actor)
    {
        this.actor = actor;
    }

    public abstract bool Act();

    public virtual void ActionCompleted()
    {
        
    }
}
