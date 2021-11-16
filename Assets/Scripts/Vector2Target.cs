using UnityEngine;

public class Vector2Target : ITarget
{
    public Vector2Target(Vector2 target)
    {
        Position = target;
    }
    
    public Vector2 Position { get; }
}
