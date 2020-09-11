using System;
using UnityEngine;

public class Batut : MonoBehaviour, IHaveIdCode, IInteractWithPhysics, IDisposable
{
    public IdCode IdCode => IdCode.NormalBatut;

    public void Interact(Transform playerTransform, Rigidbody2D playerRigitBody, PlayerControl control)
    {
        if (!control.OnWall)
        {
            var velocity = playerRigitBody.velocity;
            velocity.x *= -1;
            playerRigitBody.velocity = velocity;

            control.NullJumpCount();
            control.AddJumpCount();
        }
    }

    public void Dispose() => gameObject.SetActive(false);
}