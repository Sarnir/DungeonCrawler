using UnityEngine;

[CreateAssetMenu(menuName = "Behaviours/ArriveBehaviour", fileName = "ArriveBehaviour")]
public class ArriveBehaviour : EntityBehaviour
{
    [SerializeField]
    private float arrivingRadius;
    [SerializeField]
    private float destinationRadius;
    
    public override void Act(CharacterController controller)
    {
        /*if (controller.Target == null)
            return;
        
        Vector2 desired = controller.Target.Position - controller.Position;
        float distance = desired.magnitude;
        
        if(distance < destinationRadius)
            return;

        desired.Normalize();

        // find way to ease movement exponentially(or any other way)
        desired *= controller.MaxSpeed * Mathf.Clamp(distance / arrivingRadius, 0.1f, 1f);
        
        // Reynolds’s formula for steering force
        Vector2 steer = desired - controller.Velocity;
        steer = Vector2.ClampMagnitude(steer, controller.MaxForce);

        controller.ApplyForce(steer);*/
    }
}
