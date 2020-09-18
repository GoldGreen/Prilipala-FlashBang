using System;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour, IHave<IdCode>, ISetData<InteractiveData>, IInteract, IDisposable
{
    public IdCode Item => IdCode.Mine;

    private Sprite sleepMine;
    [SerializeField] private Sprite activeMine;
    [SerializeField] private float activeTime;

    [SerializeField] private GameObject explosionPrefab;
    private Pool explosionPool;

    private SpriteRenderer spriteRenderer;
    private new Transform transform;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform = GetComponent<Transform>();
        sleepMine = spriteRenderer.sprite;
    }

    public void SetData(InteractiveData interactiveData)
    {
        explosionPool = new Pool(InstansiateExposion(interactiveData));
    }

    public IEnumerable<IPoolable> InstansiateExposion(InteractiveData interactiveData)
    {
        var explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
        explosion.name = "MineExplosion";

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

    public void Interact()
    {
        spriteRenderer.sprite = activeMine;
        StartCoroutine(CoroutineT.Single(Explode, activeTime.Randomize(1.4f)));
    }

    private void Explode()
    {
        spriteRenderer.sprite = sleepMine;
        Dispose();
        explosionPool.PoolObject();
    }

    public void Dispose()
    {
        gameObject.SetActive(false);
    }
}
