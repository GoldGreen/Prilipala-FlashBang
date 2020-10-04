using System;
using UnityEngine;

public class Fire : MonoBehaviour, IInteractWithPlayerCharacter
{
    public void Interact(PlayerCharacterLogic playerCharacter)
    {
        playerCharacter.CanGetDamage.Update();
        CoroutineT.Single
        (
            () => playerCharacter
            .Damage(999999, DamageType.magic)
            .AddColorBy(Color.red, 1), 0.2f
        ).Start(this);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        col.GetComponent<IDisposable>()?.Dispose();
    }
}