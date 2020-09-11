using System;
using System.Collections.Generic;
using UnityEngine;

public class Bazooka : MonoBehaviour, IHaveIdCode, ISetData<InteractiveData>, IRepoolable, IDisposable
{
    public IdCode IdCode => IdCode.Bazooka;

    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private ParticleSystem shootingExplosion;
    private Pool rocketsPool;

    [SerializeField] private Vector2 accelerate;
    [SerializeField] private float reloadingTime = 2;

    private new Transform transform;

    private void Awake() => transform = GetComponent<Transform>();

    public void SetData(InteractiveData interactiveData)
    => rocketsPool = new Pool(InstansiateRocket(interactiveData));

    private IEnumerable<IPoolable> InstansiateRocket(InteractiveData interactiveData)
    {
        var rocket = Instantiate(rocketPrefab, transform.position, transform.rotation);
        rocket.name = "Rocket";

        rocket.GetComponent<ISetData<InteractiveData>>().SetData(interactiveData);
        var rocketRigidbody = rocket.GetComponent<Rigidbody2D>();
        var poolable = new Poolable(rocket);
        poolable.OnFloated.Subscribe
        (
            () =>
            {
                shootingExplosion.Play();
                rocket.transform.position = shootingExplosion.transform.position;
                rocket.transform.rotation = transform.rotation;
                rocketRigidbody.velocity = Vector2.zero;

                rocketRigidbody.AddForce(accelerate);
            }
        );

        yield return poolable;
    }

    public void Repool()
    {
        accelerate.x = (transform.position.x < 0 ? 1 : -1) * Mathf.Abs(accelerate.x);
        StopAllCoroutines();
        CoroutineT.Infinite(rocketsPool.PoolObject, reloadingTime).Start(this);
    }

    public void Dispose()
    {
        gameObject.SetActive(false);
        rocketsPool.HideAll();
    }
}
