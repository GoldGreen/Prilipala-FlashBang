using System;
using System.Collections.Generic;
using UnityEngine;

public class Blaster : MonoBehaviour, IHave<IdCode>, ISetData<InteractiveData>, IDisposable, IRepoolable
{
    public IdCode Item => IdCode.Blaster;

    [SerializeField] private GameObject bulletPrefab;
    private Pool bulletsPool;

    [SerializeField] private int bulletCount;

    [SerializeField] private float reloadTime;
    [SerializeField] private float shootingTime;

    [SerializeField] private float accelerate;

    private new Transform transform;

    private void Awake()
    {
        transform = GetComponent<Transform>();
    }

    public void SetData(InteractiveData interactiveData)
    {
        bulletsPool = new Pool(InstantiateBullets(interactiveData));
    }

    private void Start()
    {
        reloadTime += bulletCount * shootingTime;
    }

    private IEnumerable<IPoolable> InstantiateBullets(InteractiveData interactiveData)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            var bullet = Instantiate(bulletPrefab);
            bullet.name = "BlasterBullet";

            var bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
            bullet.GetComponent<ISetData<InteractiveData>>().SetData(interactiveData);

            var poolable = new Poolable(bullet);
            poolable.OnFloated.Subscribe
            (
                () =>
                {
                    bullet.transform.position = transform.position;
                    bullet.transform.rotation = transform.rotation;

                    bulletRigidbody.velocity = Vector2.zero;
                    bulletRigidbody.AddForce(new Vector2(accelerate, 0));
                }
            );

            yield return poolable;
        }
    }

    private void Shoot()
    {
        bulletsPool.PoolObject();
        for (int i = 1; i < bulletCount; i++)
        {
            CoroutineT.Single(bulletsPool.PoolObject, i * shootingTime).Start(this);
        }
    }

    public void Repool()
    {
        accelerate = (transform.position.x < 0 ? 1 : -1) * Mathf.Abs(accelerate);
        StopAllCoroutines();
        CoroutineT.InfiniteBefore(Shoot, reloadTime).Start(this);
    }

    public void Dispose()
    {
        gameObject.SetActive(false);
        bulletsPool.HideAll();
    }
}