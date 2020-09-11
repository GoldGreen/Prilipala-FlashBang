using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour, ISetData<InteractiveData>, IInteractWithPhysics, IInteract, IRepoolable, IDestroyedByWave
{
    [SerializeField] private GameObject explosionPrefab;
    private Pool explosionsPool;

    [SerializeField] private float activeTime = 0.5f;
    [SerializeField] private float hittedActiveTime = 0.15f;

    private new Rigidbody2D rigidbody2D;
    private new Transform transform;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
    }

    public void SetData(InteractiveData interactiveData)
    => explosionsPool = new Pool(InstansiateExplosion(interactiveData));

    private IEnumerable<IPoolable> InstansiateExplosion(InteractiveData interactiveData)
    {
        var explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
        explosion.name = "GrenadeExplosion";

        explosion.GetComponent<ISetData<InteractiveData>>().SetData(interactiveData);

        var poolable = new Poolable(explosion);
        poolable.OnFloated.Subscribe
        (
             () =>
             {
                 explosion.transform.position = transform.position;
                 explosion.transform.rotation = transform.rotation;
             }
        );

        yield return poolable;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger)
        {
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX;
            rigidbody2D.freezeRotation = true;
        }
    }

    public void Interact(Transform playerTransform, Rigidbody2D playerRigitBody, PlayerControl control)
    {
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX;
        rigidbody2D.freezeRotation = true;
    }

    public void Interact()
    {
        StopAllCoroutines();
        StartCoroutine(CoroutineT.Single(Explode, hittedActiveTime));
    }

    private void Explode()
    {
        Destroy();
        explosionsPool.PoolObject();
    }

    public void Repool()
    {
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        StartCoroutine(CoroutineT.Single(Explode, activeTime.Randomize(3)));
    }

    public void Destroy() => gameObject.SetActive(false);
}
