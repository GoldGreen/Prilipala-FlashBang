using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class GunAbstraction : MonoBehaviour, IHaveIdCode, ISetData<InteractiveData>, ILinkWithPlayerTransform, ILinkWithShower, IDisposable, IRepoolable
{
    public abstract IdCode IdCode { get; }
    protected abstract string BulletName { get; }
    [SerializeField] protected ParticleSystem shootingExplosion;
    [SerializeField] protected GameObject bulletPrefab;

    [SerializeField] protected int bulletCount;
    protected Pool bulletsPool;

    [SerializeField] protected GameObject hittingEffectPrefab;
    protected Pool<Vector2> hittingEffectsPool;

    [Range(1, 1000)]
    [SerializeField] protected int countIn1Queue;
    [SerializeField] protected float timeBetweenQueues;

    [SerializeField] protected float reloading;
    [SerializeField] protected float accelerate;
    [SerializeField] protected float minShootingDistanse;
    [SerializeField] protected float randomizedAngle;

    protected float reloadingTimeBetweenShoots;

    protected ReloadingEntity<InteractiveData> reloadingEntity;

    protected Vector3 position;
    protected Vector3 playerPosition;
    protected Vector3 distanceToPlayer;

    protected UnityEvent OnPlayerOutRadius { get; } = new UnityEvent();
    protected UnityEvent OnPlayerInRadius { get; } = new UnityEvent();
    protected UnityEvent OnReloadingStart { get; } = new UnityEvent();
    protected UnityEvent OnReloadingEnd => reloadingEntity.OnReloaded;

    protected new Transform transform;

    public Transform PlayerTransform { get; set; }
    public EffectShower EffectShower { get; set; }

    protected virtual void Awake()
    {
        transform = GetComponent<Transform>();

        var count = bulletCount - 1;
        var queueCount = count / countIn1Queue;
        var shoots = count - queueCount;

        reloadingEntity = new ReloadingEntity<InteractiveData>(x =>
        shoots * x.DamageTime + timeBetweenQueues * queueCount + reloading);
    }

    public virtual void SetData(InteractiveData interactiveData)
    {

        reloadingTimeBetweenShoots = interactiveData.DamageTime;
        interactiveData.SetTo(reloadingEntity);

        hittingEffectsPool = new Pool<Vector2>
        (
            InstansiateHittingEffect()
            .With(InstansiateHittingEffect())
        );

        bulletsPool = new Pool(InstansiateBullets(interactiveData));
    }

    private void FixedUpdate()
    {
        position = transform.position;
        playerPosition = PlayerTransform.position;
        distanceToPlayer = playerPosition - position;

        if (distanceToPlayer.magnitude < minShootingDistanse)
        {
            OnPlayerInRadius.Invoke();

            if (reloadingEntity.IsReloaded)
            {
                Shoot();
                reloadingEntity.StartReload(this);
            }
        }
        else
            OnPlayerOutRadius.Invoke();

        transform.rotation = distanceToPlayer.ToQuartetion();
    }

    private void Shoot()
    {
        var time = 0.0f;

        bulletsPool.PoolObject();
        for (int i = 1; i < bulletsPool.Length; i++)
        {
            if (i + 1 < bulletsPool.Length)
            {
                time += i % countIn1Queue == 0 ? timeBetweenQueues : reloadingTimeBetweenShoots;
            }
            else
                time += reloadingTimeBetweenShoots;

            CoroutineT.Single(bulletsPool.PoolObject, time).Start(this);
        }

        CoroutineT.Single(OnReloadingStart.Invoke, time).Start(this);
    }

    private float RandomizeAngle => UnityEngine.Random.Range(-randomizedAngle, randomizedAngle);

    private IEnumerable<IPoolable<Vector2>> InstansiateHittingEffect()
    {
        var hittingEffect = Instantiate(hittingEffectPrefab);
        hittingEffect.name = "HittingEffect";

        var particle = hittingEffect.GetComponent<ParticleSystem>();
        var poolable = new Poolable<Vector2>(hittingEffect);

        poolable.OnFloated.Subscribe
        (
            position =>
            {
                hittingEffect.transform.position = position;
                var angle = Mathf.Rad2Deg * Mathf.Atan2(distanceToPlayer.y, distanceToPlayer.x);
                hittingEffect.transform.eulerAngles = hittingEffect.transform.eulerAngles.Change(x: -angle);
                particle.Play();
            }
        );

        yield return poolable;
    }

    private IEnumerable<IPoolable> InstansiateBullets(InteractiveData interactiveData)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.name = BulletName;

            var bulleetLogic = bullet.GetComponent<Bullet>();

            bulleetLogic.OnInteract.Subscribe(hittingEffectsPool.PoolObject);

            bulleetLogic.SetData(interactiveData);
            bulleetLogic.EffectShower = EffectShower;

            var bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
            var poolable = new Poolable(bullet);

            poolable.OnFloated.Subscribe
            (
                () =>
                {
                    if (distanceToPlayer.magnitude < minShootingDistanse)
                    {
                        shootingExplosion.Play();
                        bullet.transform.position = shootingExplosion.transform.position;
                        bullet.transform.rotation = transform.rotation;

                        bulletRigidbody.velocity = Vector2.zero;

                        var angle = (PlayerTransform.position - transform.position).Atan2();
                        angle = Mathf.Deg2Rad * (angle * Mathf.Rad2Deg + RandomizeAngle);

                        bulletRigidbody.AddForce(accelerate * angle.ToVectorFromRad());
                    }
                    else
                        bullet.SetActive(false);
                }
            );

            yield return poolable;
        }
    }

    public void Repool()
    {
        reloadingEntity.FullReload();
    }

    public virtual void Dispose()
    {
        gameObject.SetActive(false);
        bulletsPool.HideAll();
        hittingEffectsPool.HideAll();
    }
}
