using System;
using UnityEngine;

public class Cactus : MonoBehaviour, IHave<IdCode>, ISetData<InteractiveData>, IInteractWithPhysics, IInteract, IDisposable, ILinkWithShower, IDestroyedByWave
{
    public IdCode Item => IdCode.Cactus;

    [SerializeField] private Sprite effectIcon;
    private float blockedTime;

    public EffectShower EffectShower { get; set; }
    public void SetData(InteractiveData interactiveData)
    {
        blockedTime = interactiveData.BlockedTime;
    }

    public void Interact(Transform playerTransform, Rigidbody2D playerRigitBody, PlayerControl control)
    {
        if (playerRigitBody.velocity.x == 0)
        {
            playerRigitBody.velocity = Vector2.zero;
        }

        control.BlockPlayerAt(blockedTime);
    }

    public void Interact()
    {
        gameObject.SetActive(false);
        EffectShower.AddOrUpdate(effectIcon, EffectType.Single, blockedTime);
    }

    public void Dispose()
    {
        gameObject.SetActive(false);
    }

    public void Destroy()
    {
        Dispose();
    }
}
