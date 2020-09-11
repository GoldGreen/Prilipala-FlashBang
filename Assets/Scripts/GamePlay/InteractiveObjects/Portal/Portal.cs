using UnityEngine;
using static SharpExtensions;

public class Portal : MonoBehaviour, IHaveIdCode, IInteractWithPhysics, ILinkWithShower, IRepoolable
{
    public IdCode IdCode => IdCode.Portal;

    private static FlagEntity canTeleportate = new FlagEntity();

    [SerializeField] private Transform leftTransform;
    [SerializeField] private Transform rightTransform;

    [SerializeField] private Collider2D leftCollider;
    [SerializeField] private Collider2D rigthCollider;

    [SerializeField] private float maxRandomizedOffsetY;

    private float LeftOffsetY
    {
        set
        {
            leftTransform.localPosition = leftTransform.localPosition.Change(y: value);
            leftCollider.offset = leftCollider.offset.Change(y: value);
        }
    }

    private float RightOffsetY
    {
        set
        {
            rightTransform.localPosition = rightTransform.localPosition.Change(y: value);
            rigthCollider.offset = rigthCollider.offset.Change(y: value);
        }
    }

    [SerializeField] private GameObject inputEffectPrefab;
    [SerializeField] private GameObject outputEffectPrefab;

    [SerializeField] private Sprite effectIcon;
    [SerializeField] private float teleportDeltaX;
    [SerializeField] private float blocktTeleportingTime;

    public EffectShower EffectShower { get; set; }

    private ParticleSystem inputEffect;
    private ParticleSystem outputEffect;

    private Transform inputTransform;
    private Transform outputTransform;

    private void Awake()
    {
        inputEffect = Instantiate(inputEffectPrefab).GetComponent<ParticleSystem>();
        outputEffect = Instantiate(outputEffectPrefab).GetComponent<ParticleSystem>();

        inputEffect.name = "Input";
        outputEffect.name = "Effect";

        inputTransform = inputEffect.transform;
        outputTransform = outputEffect.transform;
    }

    public void Interact(Transform playerTransform, Rigidbody2D playerRigitBody, PlayerControl control)
    {
        if (!control.OnWall && canTeleportate && PlayerPositionAndVelocityAreCorrect(playerTransform.position, playerRigitBody.velocity))
        {
            canTeleportate.DenyAt(control, blocktTeleportingTime);

            EffectShower.AddOrUpdate(effectIcon, EffectType.Single, blocktTeleportingTime);

            var newPosition =
            (
                playerTransform.position.x < 0 ?
                rightTransform :
                leftTransform
            ).position;

            newPosition.x += playerTransform.position.x > 0 ? teleportDeltaX : -teleportDeltaX;
            playerTransform.position = newPosition;

            var inputEuler = inputTransform.eulerAngles;
            var outputEuler = outputTransform.eulerAngles;

            if (playerTransform.position.x > 0)
            {
                outputTransform.position = leftTransform.position;
                inputTransform.position = rightTransform.position;

                inputTransform.eulerAngles = inputEuler.Change(x: 180);
                outputTransform.eulerAngles = outputEuler.Change(x: 0);
            }
            else
            {
                outputTransform.position = rightTransform.position;
                inputTransform.position = leftTransform.position;

                inputTransform.eulerAngles = inputEuler.Change(x: 0);
                outputTransform.eulerAngles = outputEuler.Change(x: 180);
            }

            outputEffect.Play();
            inputEffect.Play();

            gameObject.SetActive(false);
        }
    }

    private bool PlayerPositionAndVelocityAreCorrect(Vector2 position, Vector2 velocity)
    => AllIs(vector => vector.x > 0, position, velocity) || AllIs(vector => vector.x < 0, position, velocity);

    public void Repool()
    {
        float randomizedOffsetY = Random.Range(-maxRandomizedOffsetY, maxRandomizedOffsetY);

        if (Random.Range(0, 2) == 0)
        {
            LeftOffsetY = randomizedOffsetY;
            RightOffsetY = 0;
        }
        else
        {
            LeftOffsetY = 0;
            RightOffsetY = randomizedOffsetY;
        }
    }
}