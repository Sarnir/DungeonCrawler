using UnityEngine;

[CreateAssetMenu(menuName = "Behaviours/SeekBehaviour", fileName = "SeekBehaviour")]
public class SeekBehaviour : EntityBehaviour
{
    public override void Act(CharacterController controller)
    {
        Seek(controller);
    }
    
    protected void Seek(CharacterController controller)
    {
        Vector2 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        Vector2 desired = targetPos - controller.Position;
        desired.Normalize();
        
        // Calculating the desired velocity to target at max speed
        desired *= controller.MaxSpeed;
 
        // Reynolds’s formula for steering force
        Vector2 steer = desired - controller.Velocity;
        steer = Vector2.ClampMagnitude(steer, controller.MaxForce);

        controller.ApplyForce(steer);
    }
}
