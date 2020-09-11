using System;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour, IHaveIdCode, ISetData<InteractiveData>, IDisposable, IRepoolable
{
    public IdCode IdCode => IdCode.Bow;

    [SerializeField] private GameObject arrowPrefab;
    private Pool arrowsPool;

    [SerializeField] private float reloadTime = 2;
    [SerializeField] private Vector2 accelerate;

    private new Transform transform;

    private void Awake() => transform = GetComponent<Transform>();
    public void SetData(InteractiveData interactiveData) => arrowsPool = new Pool(InstantiateArrow(interactiveData));

    private IEnumerable<IPoolable> InstantiateArrow(InteractiveData interactiveData)
    {
        var arrow = Instantiate(arrowPrefab);
        arrow.name = "Arrow";

        var arrowComponent = arrow.GetComponent<Arrow>();

        arrowComponent.SetData(interactiveData);
        arrowComponent.IsRotating = true;

        var arrowRigitBody = arrow.GetComponent<Rigidbody2D>();
        var poolable = new Poolable(arrow);
        poolable.OnFloated.Subscribe
        (
            () =>
            {
                arrow.transform.position = transform.position;
                arrow.transform.rotation = transform.rotation;

                arrowRigitBody.velocity = Vector2.zero;
                arrowRigitBody.AddForce(accelerate);
            }
        );

        yield return poolable;
    }

    public void Repool()
    {
        accelerate.x = (transform.position.x < 0 ? 1 : -1) * Mathf.Abs(accelerate.x);
        StopAllCoroutines();
        CoroutineT.InfiniteBefore(arrowsPool.PoolObject, reloadTime).Start(this);
    }

    public void Dispose()
    {
        gameObject.SetActive(false);
        arrowsPool.HideAll();
    }
}
