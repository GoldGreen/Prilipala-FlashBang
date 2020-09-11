using System;
using System.Collections.Generic;
using UnityEngine;

public class Plunger : MonoBehaviour, IHaveIdCode, ISetData<InteractiveData>, IDisposable, IRepoolable
{
    public IdCode IdCode => IdCode.Plunger;

    [SerializeField] private GameObject bulletPrefab;
    private Pool plungerBulletsPool;

    [SerializeField] private Sprite withBullet;
    [SerializeField] private Sprite withoutBullet;

    [SerializeField] private Vector2 accelerate;

    [SerializeField] private float shootingReload;

    private new Transform transform;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        transform = GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetData(InteractiveData interactiveData)
    {
        plungerBulletsPool = new Pool(InstantiateBullet(interactiveData));
    }

    private void Start()
    {
        shootingReload = shootingReload.Randomize(1.5f);
    }

    private IEnumerable<IPoolable> InstantiateBullet(InteractiveData interactiveData)
    {
        var bullet = Instantiate(bulletPrefab);
        bullet.name = "bullet";

        bullet.GetComponent<ISetData<InteractiveData>>().SetData(interactiveData);

        var poolable = new Poolable(bullet);
        poolable.OnFloated.Subscribe
        (
            () =>
            {
                spriteRenderer.sprite = withoutBullet;
                bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation;

                var rigitbody = bullet.GetComponent<Rigidbody2D>();
                rigitbody.velocity = Vector2.zero;
                rigitbody.AddForce(accelerate);

                StartCoroutine(CoroutineT.Single
                (
                    () => spriteRenderer.sprite = withBullet, shootingReload / 2)
                );
            }
        );

        yield return poolable;
    }

    public void Repool()
    {
        accelerate.x = (transform.position.x < 0 ? 1 : -1) * Mathf.Abs(accelerate.x);
        StopAllCoroutines();
        CoroutineT.InfiniteBefore(plungerBulletsPool.PoolObject, shootingReload).Start(this);
    }

    public void Dispose()
    {
        gameObject.SetActive(false);
        plungerBulletsPool.HideAll();
    }
}
