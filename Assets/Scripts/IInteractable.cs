using UnityEngine;

public interface IInteractable : ITarget
{
    void Interact(CharacterController interactor);

    //bool CanInteract();
}
