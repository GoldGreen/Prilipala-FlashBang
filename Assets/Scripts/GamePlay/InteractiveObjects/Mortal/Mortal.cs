using System;
using System.Collections.Generic;
using UnityEngine;

public class Mortal : MonoBehaviour, IHave<IdCode>, ISetData<InteractiveData>, IRepoolable, IDisposable
{
    public IdCode Item => IdCode.Mortal;

    [SerializeField] private GameObject grenadePrefab;
    private Pool grenadesPool;

    [SerializeField] private Vector2 accelerate;
    [SerializeField] private float reloadTime = 2;

    private new Transform transform;

    private void Awake()
    {
        transform = GetComponent<Transform>();
    }

    public void SetData(InteractiveData interactiveData)
    {
        grenadesPool = new Pool(InstansiateGrenade(interactiveData));
    }

    private IEnumerable<IPoolable> InstansiateGrenade(InteractiveData interactiveData)
    {
        var grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
        grenade.name = "Grenade";

        grenade.GetComponent<ISetData<InteractiveData>>().SetData(interactiveData);

        var grenadeRigidbody = grenade.GetComponent<Rigidbody2D>();

        var poolable = new Poolable(grenade);
        poolable.OnFloated.Subscribe
        (
            () =>
            {
                grenade.transform.position = transform.position;
                grenade.transform.rotation = transform.rotation;

                grenadeRigidbody.velocity = Vector2.zero;
                grenadeRigidbody.AddForce(accelerate);
            }
        );

        yield return poolable;
    }

    public void Repool()
    {
        accelerate.x = (transform.position.x < 0 ? 1 : -1) * Mathf.Abs(accelerate.x);
        StopAllCoroutines();
        CoroutineT.InfiniteBefore(grenadesPool.PoolObject, reloadTime).Start(this);
    }

    public void Dispose()
    {
        gameObject.SetActive(false);
        grenadesPool.HideAll();
    }
}
