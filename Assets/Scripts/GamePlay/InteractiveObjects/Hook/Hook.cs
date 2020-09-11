using UnityEngine;
using UnityEngine.Events;

public class Hook : MonoBehaviour, IInteractWithPhysics
{
    private static bool Locked = false;
    public bool Hitted { get; set; } = false;

    private float time = 0.5f;
    public UnityEvent OnHitted { get; private set; } = new UnityEvent();

    public void Interact(Transform playerTransform, Rigidbody2D playerRigitBody, PlayerControl control)
    {
        if (!Locked)
        {
            Locked = true;
            Hitted = true;

            OnHitted.Invoke();

            CoroutineT.WhileBefore
            (
                () => control.LockTransformBy(transform, time),
                () => gameObject.activeInHierarchy && Hitted,
                time,
                () => Locked = false
            ).Start(control);
        }
    }
}