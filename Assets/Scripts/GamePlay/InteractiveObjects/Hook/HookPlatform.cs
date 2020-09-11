using System;
using UnityEngine;

public class HookPlatform : MonoBehaviour, IHaveIdCode, ILinkWithPlayerTransform, IRepoolable, IDisposable
{
    public IdCode IdCode => IdCode.Hook;

    [SerializeField] private GameObject hook;
    private Hook hookComponent;
    [SerializeField] private float minDistance;
    [SerializeField] private float accelerate;
    [SerializeField] private float reloading;
    [SerializeField] private int randomizingAngle;
    [SerializeField] private float speed;

    private ReloadingEntity reloadingEntity;

    private LineRenderer lineToHook;
    private MovingObject movingObject;
    private float ySpeed;

    private Collider2D hookCollider;
    private Transform hookTransform;
    private Rigidbody2D hookRigidBody;

    private new Transform transform;

    private Vector3 position;
    private Vector3 hookPosition;
    private Vector3 playerPosition;
    private Vector3 distancePlayer;
    private Vector3 distanceHook;
    private Vector3 distanceHookPlayer;

    public Transform PlayerTransform { get; set; }

    private void Awake()
    {
        reloadingEntity = new ReloadingEntity(reloading);

        lineToHook = GetComponent<LineRenderer>();
        transform = GetComponent<Transform>();
        movingObject = GetComponent<MovingObject>();
        ySpeed = movingObject.Speed.y;

        hookTransform = hook.GetComponent<Transform>();
        hookRigidBody = hook.GetComponent<Rigidbody2D>();
        hookCollider = hook.GetComponent<Collider2D>();
        hookComponent = hook.GetComponent<Hook>();
    }

    private void Start()
    {
        hookComponent.OnHitted.Subscribe(MoveBack);
    }

    private void FixedUpdate()
    {
        position = transform.position;
        hookPosition = hookTransform.position;
        playerPosition = PlayerTransform.position;

        distanceHook = hookPosition - position;
        distancePlayer = playerPosition - position;
        distanceHookPlayer = playerPosition - hookPosition;

        lineToHook.SetPosition(0, hookPosition);
        lineToHook.SetPosition(1, position);

        if (distancePlayer.magnitude <= minDistance && reloadingEntity.IsReloaded)
        {
            Shoot();
        }

        if (distanceHook.magnitude > minDistance)
        {
            MoveBack();
        }
    }

    private void Shoot()
    {
        hookCollider.enabled = true;
        reloadingEntity.StartReload(this);

        float angle = distanceHookPlayer.Atan2();
        angle = Mathf.Deg2Rad * (Mathf.Rad2Deg * angle + UnityEngine.Random.Range(-randomizingAngle, randomizingAngle));

        var force = new Vector2(accelerate * Mathf.Cos(angle), accelerate * Mathf.Sin(angle));

        hookRigidBody.AddForce(force);
        hookTransform.rotation = force.ToQuartetion();

        movingObject.Speed = movingObject.Speed.Change(y: 0);
    }

    private void MoveBack()
    {
        hookCollider.enabled = false;
        hookRigidBody.velocity = Vector2.zero;

        CoroutineT.WhileBefore
        (
            () => hookTransform.position = Vector3.MoveTowards(hookPosition, position, speed),
            () => distanceHook.magnitude > 0.1f,
            0,
            () =>
            {
                hookTransform.localEulerAngles = Vector3.zero;

                if (hookComponent.Hitted)
                {
                    gameObject.SetActive(false);
                }

                movingObject.Speed = movingObject.Speed.Change(y: ySpeed);
            }
        ).Start(this);
    }

    public void Repool()
    {
        reloadingEntity.FullReload();
        hookComponent.Hitted = false;
        hookCollider.enabled = false;
    }

    public void Dispose()
    {
        gameObject.SetActive(false);
    }
}