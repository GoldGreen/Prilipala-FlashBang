using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour, ISetData<InteractiveData>, IDestroyedByWave, IRepoolable
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private ParticleSystem flyingParticle;
    private Pool explosionsPool;

    private new Transform transform;

    private void Awake() => transform = GetComponent<Transform>();
    public void SetData(InteractiveData interactiveData)
    => explosionsPool = new Pool(InstansiateExplosion(interactiveData));

    private IEnumerable<IPoolable> InstansiateExplosion(InteractiveData interactiveData)
    {
        var explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
        explosion.name = "BazookaExplosion";

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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.isTrigger)
        {
            gameObject.SetActive(false);
            flyingParticle.Stop();
            explosionsPool.PoolObject();
        }
    }

    public void Repool() => flyingParticle.Play();
    public void Destroy() => gameObject.SetActive(false);
}
