using System;
using UnityEngine;

public class PowerfulBatut : MonoBehaviour, IHaveIdCode, IInteractWithPhysics, IDisposable
{
    public IdCode IdCode => IdCode.PowerfulBatut;

    private Vector2 vector45Degree = new Vector2(0.7f, 0.7f);
    private float powerMultiply = 1.3f;

    public void Interact(Transform playerTransform, Rigidbody2D playerRigitBody, PlayerControl control)
    {
        if (!control.OnWall)
        {
            var localAccelerate = vector45Degree * (control.Accelerate * powerMultiply);

            if (playerRigitBody.velocity.x > 0)
            {
                localAccelerate.x *= -1;
            }

            playerRigitBody.velocity *= 0;

            playerRigitBody.AddForce(localAccelerate);

            control.NullJumpCount();
            control.AddJumpCount();
        }
    }

    public void Dispose()
    {
        gameObject.SetActive(false);
    }
}