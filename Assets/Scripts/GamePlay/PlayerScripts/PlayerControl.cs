using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerControl : MonoBehaviour
{
    public const float PI_2 = Mathf.PI / 2;

    public static readonly Quaternion Normal = Quaternion.Euler(0, 0, 0);
    public static readonly Quaternion Reverse = Quaternion.Euler(0, 180, 0);

    public float DefaultGravityScale => defaultGravityScale;
    [SerializeField] private float defaultGravityScale = 1;

    public float WallGravityScale
    {
        get => wallGravityScale;
        set => wallGravityScale = value;
    }
    [SerializeField] private float wallGravityScale = 0.2f;

    public float Accelerate
    {
        get => accelerate;
        set => accelerate = value;
    }

    [SerializeField] private float accelerate;
    [SerializeField] private MovingObject fireMoving;
    [SerializeField] private TouchDetector touchDetector;

    public bool OnWall => jumpCount == 0;

    public FlagEntity CanJump { get; private set; } = new FlagEntity();
    public FlagEntity CanAccelerateMultiply { get; private set; } = new FlagEntity();

    public bool SecondJump { get; private set; } = false;
    private bool firstClick = true;

    private int jumpCount = 0;

    public FlagEntity IsUnlock { get; private set; } = new FlagEntity();
    private Transform lockTransform;

    private IDisposableCollection subscribers = new Disposables();
    public UnityEvent<PlayerControl> OnFirstJump { get; } = new UnityEvent<PlayerControl>();
    public UnityEvent<PlayerControl> OnTrySecondJump { get; } = new UnityEvent<PlayerControl>();
    public UnityEvent<PlayerControl> OnJumpFromWall { get; } = new UnityEvent<PlayerControl>();
    public UnityEvent<PlayerControl> OnSecondJump { get; } = new UnityEvent<PlayerControl>();
    public UnityEvent<PlayerControl> OnSlimeToWall { get; } = new UnityEvent<PlayerControl>();
    public UnityEvent<PlayerControl> OnJump { get; } = new UnityEvent<PlayerControl>();

    private new Rigidbody2D rigidbody2D;
    private new Transform transform;

    private void Awake()
    {
        transform = GetComponent<Transform>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        touchDetector.OnDraged.Subscribe(TryJump).AddTo(subscribers);
        rigidbody2D.AddForce(accelerate * (Mathf.PI / 2).ToVectorFromRad());
    }

    public void AllowSecondJump(float reloadingTime)
    {
        SecondJump = true;

        OnTrySecondJump.Subscribe(x => CoroutineT.Single(() => x.SecondJump = true, reloadingTime)
        .Start(x))
        .AddTo(subscribers);
    }

    private void TryJump(DragedArgs args)
    {
        if (!Pause.IsPouse && CanJump)
        {
            float angle = Angle(args.StartPosition, args.EndPositon);

            if (TryFirstClick() || CanJumpFromWall(angle, 10 * Mathf.Deg2Rad) || TrySecondJump())
            {
                Jump(angle);
            }
        }
    }

    private bool CanJumpFromWall(float angle, float error)
    {
        return jumpCount == 0 && (transform.position.x < 0 && angle < PI_2 - error && angle > -PI_2 + error
           || transform.position.x > 0 && (angle > PI_2 + error || angle < -PI_2 - error));
    }

    private bool TryFirstClick()
    {
        if (firstClick)
        {
            OnFirstJump.Invoke(this);
            firstClick = false;
            return true;
        }

        return false;
    }

    private bool TrySecondJump()
    {
        if (SecondJump && jumpCount == 1)
        {
            OnTrySecondJump.Invoke(this);
            SecondJump = false;
            return true;
        }

        return false;
    }

    private void FixedUpdate()
    {
        if (rigidbody2D.velocity.x != 0)
        {
            transform.rotation = rigidbody2D.velocity.ToQuartetion();
        }

        if (!IsUnlock && lockTransform)
        {
            rigidbody2D.MovePosition(lockTransform.position);
        }
    }

    public void LockTransformBy(Transform lockTransform, float time)
    {
        this.lockTransform = lockTransform;

        IsUnlock.Update();
        IsUnlock.DenyAt(this, time);
    }

    public void MultiplyAccellerate(float multiply, float time)
    {
        if (CanAccelerateMultiply)
        {
            accelerate *= multiply;
            rigidbody2D.velocity *= multiply;
            CoroutineT.Single(() => accelerate /= multiply, time).Start(this);
        }
    }

    public void BlockPlayerAt(float time)
    {
        CanJump.DenyAt(this, time);
    }

    public void BlockAccelerateMultiplyAt(float time)
    {
        CanAccelerateMultiply.DenyAt(this, time);
    }

    public void AllowSecondJumpAfter(float time)
    {
        CoroutineT.Single(() => SecondJump = true, time).Start(this);
    }

    public void NullJumpCount()
    {
        jumpCount = 0;
    }

    public void AddJumpCount()
    {
        jumpCount++;
    }

    private float Angle(Vector3 point1, Vector3 point2)
    {
        return Mathf.Atan2(point2.y - point1.y, point2.x - point1.x);
    }

    private void Jump(float angle)
    {
        AddJumpCount();

        if (jumpCount == 1)
        {
            OnJumpFromWall.Invoke(this);
        }
        else if (jumpCount == 2)
        {
            OnSecondJump.Invoke(this);
        }
        else
        {
            return;
        }

        rigidbody2D.gravityScale = defaultGravityScale;
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.AddForce(accelerate * angle.ToVectorFromRad());
        OnJump.Invoke(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        NullJumpCount();

        if (rigidbody2D.gravityScale == defaultGravityScale)
        {
            rigidbody2D.gravityScale = WallGravityScale;
        }

        rigidbody2D.velocity = Vector2.zero;
        transform.rotation = transform.position.x < 0 ? Normal : Reverse;

        OnSlimeToWall.Invoke(this);
    }

    private void OnDestroy()
    {
        subscribers.Dispose();
    }
}