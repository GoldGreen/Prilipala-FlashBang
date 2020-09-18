using System;
using System.Collections.Generic;
using UnityEngine;

public class CrossBow : MonoBehaviour, IHave<IdCode>, ISetData<InteractiveData>, IDisposable, IRepoolable
{
    public IdCode Item => IdCode.CrossBow;

    [SerializeField] private float reloadTime = 2;

    [SerializeField] private GameObject arrowPrefab;

    private Pool arrowPool;

    [SerializeField] private Vector2 accelerate;
    [SerializeField] private float deltaY;

    private new Transform transform;

    private void Awake()
    {
        transform = GetComponent<Transform>();
    }

    public void SetData(InteractiveData interactiveData)
    {
        InstantiateArrows(interactiveData);
    }

    private void InstantiateArrows(InteractiveData interactiveData)
    {
        arrowPool = new Pool
        (
            InstansiateArrow(deltaY, interactiveData)
            .With(InstansiateArrow(0, interactiveData))
            .With(InstansiateArrow(-deltaY, interactiveData))
        );
    }

    private IEnumerable<IPoolable> InstansiateArrow(float deltaY, InteractiveData interactiveData)
    {
        var arrow = Instantiate(arrowPrefab);
        arrow.transform.localScale *= 0.7f;
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
                 arrowRigitBody.gravityScale = 0;
                 arrowRigitBody.AddForce(accelerate.Change(y: deltaY));
             }
        );

        yield return poolable;
    }

    public void Repool()
    {
        accelerate.x = (transform.position.x < 0 ? 1 : -1) * Mathf.Abs(accelerate.x);
        StopAllCoroutines();
        CoroutineT.InfiniteBefore(arrowPool.PoolAll, reloadTime).Start(this);
    }

    public void Dispose()
    {
        gameObject.SetActive(false);
        arrowPool.HideAll();
    }
}