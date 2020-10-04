using UnityEngine;
using UnityEngine.Events;

public interface IInteractWithPhysics
{
    void Interact(Transform playerTransform, Rigidbody2D playerRigitBody, PlayerControl control);
}

public interface IInteractWithPlayerCharacter
{
    void Interact(PlayerCharacterLogic playerCharacter);
}

public interface ISubscribedInteract : IInteract
{
    UnityEvent OnInteracted { get; }
}

public interface IInteractWithSound
{
    void Interact(AudioSource source);
}

public interface IInteract
{
    void Interact();
}