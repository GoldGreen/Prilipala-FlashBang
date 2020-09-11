using UnityEngine;

public interface IInteractWithPhysics
{
    void Interact(Transform playerTransform, Rigidbody2D playerRigitBody, PlayerControl control);
}

public interface IInteractWithPlayerCharacter
{
    void Interact(PlayerCharacterLogic playerCharacter);
}

public interface IInteract
{
    void Interact();
}