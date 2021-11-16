using UnityEngine;

public abstract class EntityBehaviour : ScriptableObject
{
    public abstract void Act(CharacterController controller);
}

