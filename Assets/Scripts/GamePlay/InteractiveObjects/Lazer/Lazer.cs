using System;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour, IHaveIdCode, ISetData<InteractiveData>, IRepoolable, IDisposable, ILinkWithShower
{
    public IdCode IdCode => IdCode.Lazer;

    [SerializeField] private GameObject rayPrefab;
    private Pool raysPool;

    [SerializeField] private float reloadingTime = 1.5f;
    [SerializeField] private float rayLivingTime = 1;

    private MovingObject movingObject;
    private new Transform transform;

    public EffectShower EffectShower { get; set; }

    private void Awake()
    {
        transform = GetComponent<Transform>();
        movingObject = GetComponent<MovingObject>();
    }

    public void SetData(InteractiveData interactiveData)
    {
        raysPool = new Pool(InstantiateRay(interactiveData));
    }

    private IEnumerable<IPoolable> InstantiateRay(InteractiveData interactiveData)
    {
        var ray = Instantiate(rayPrefab, transform.position, transform.rotation);
        ray.name = "Lazer Ray";

        var rayComponent = ray.GetComponent<LazerRay>();

        rayComponent.SetData(interactiveData);

        ray.GetComponent<ILinkWithShower>().EffectShower = EffectShower;

        var poolable = new Poolable(ray);
        poolable.OnFloated.Subscribe
        (
            () =>
            {
                ray.transform.position = transform.position;
                ray.transform.rotation = transform.rotation;

                float y = movingObject.Speed.y;
                movingObject.Speed = movingObject.Speed.Change(y: 0);
                movingObject.UpdateSpeedAndDelta();

                rayComponent.StartCoroutine(CoroutineT.Single
                (
                    () =>
                    {
                        ray.SetActive(false);
                        movingObject.Speed = movingObject.Speed.Change(y: y);
                        movingObject.UpdateSpeedAndDelta();
                    },
                    rayLivingTime
                ));
            }
        );

        yield return poolable;
    }

    public void Repool()
    {
        StopAllCoroutines();
        CoroutineT.Infinite(raysPool.PoolObject, reloadingTime).Start(this);
    }

    public void Dispose()
    {
        gameObject.SetActive(false);
        raysPool.HideAll();
    }
}