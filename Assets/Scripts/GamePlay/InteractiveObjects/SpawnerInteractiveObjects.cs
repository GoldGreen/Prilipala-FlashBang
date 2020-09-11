using System.Collections.Generic;
using UnityEngine;

public class SpawnerInteractiveObjects : MonoBehaviour
{
    private const int POOL_COUNT = 20;

    private static readonly Quaternion normal = Quaternion.Euler(0, 0, 0);
    private static readonly Quaternion reverse = Quaternion.Euler(0, 180, 0);

    private ICollection<(Pool pool, InteractiveData data)> poolsWithInteractive;
    private readonly IDisposableCollection subscribers = new Disposables();

    [SerializeField] private GameObject[] interactives;
    [SerializeField] private PlayerScoreLogic playerScoreLogic;
    [SerializeField] private EffectShower effectShower;
    [SerializeField] private TouchDetector touchDetector;

    private new Transform transform;

    private int index = 0;
    private int spawnIndex = 0;

    private float len = 15;//22;
    private float minLen = 5;//3;

    private float yPosition = 0;

    private void Awake()
    {
        transform = GetComponent<Transform>();
    }

    private void Start()
    {
        poolsWithInteractive = PrepareUsedObjects();

        CoroutineT.WhileBefore(DistinctLen, LenIsCorrect, 1, CorrectLen).Start(this);
    }

    private void DistinctLen()
    {
        len -= 0.03f;
    }

    private bool LenIsCorrect()
    {
        return len > minLen;
    }

    private void CorrectLen()
    {
        len = minLen;
    }

    private void FixedUpdate()
    {
        index = Mathf.RoundToInt(transform.position.y / len);

        if (spawnIndex <= index && poolsWithInteractive.Count > 0)
        {
            yPosition = spawnIndex * len;
            poolsWithInteractive.GetRandomElement().pool.PoolObject();
            spawnIndex++;
        }
    }

    private ICollection<(Pool, InteractiveData)> PrepareUsedObjects()
    {
        int selectableCount = 0;
        long allScore = 0;

        ICollection<(Pool, InteractiveData)> pools = new List<(Pool, InteractiveData)>();

        foreach (var interactive in interactives)
        {
            var haveIdCode = interactive.GetComponent<IHaveIdCode>();
            var interactiveData = DB.Data.Find<InteractiveData>(haveIdCode.IdCode);

            if (interactiveData.IsSelected)
            {
                selectableCount++;
                allScore += interactiveData.Score;

                var poolingObjects = new List<IPoolable>();

                for (int j = 0; j < POOL_COUNT; j++)
                {
                    var instInteractive = Instantiate(interactive);

                    instInteractive.GetComponent<ILinkWithShower>()
                    .NotNull(x => x.EffectShower = effectShower);

                    instInteractive.GetComponent<ILinkWithTouchDetector>()
                    .NotNull(x => x.TouchDetector = touchDetector);

                    instInteractive.GetComponent<ILinkWithPlayerTransform>()
                    .NotNull(x => x.PlayerTransform = transform);

                    instInteractive.GetComponent<ISetData<InteractiveData>>()
                    ?.SetData(interactiveData);

                    instInteractive.name = interactiveData.Name;

                    var movingObject = instInteractive.GetComponent<MovingObject>();

                    new Poolable(instInteractive)
                    .AddTo(poolingObjects)
                    .OnFloated.Subscribe
                    (
                        () =>
                        {
                            instInteractive.transform.rotation =
                            Random.Range(0, 2) % 2 == 0 || movingObject.StartDelta.x == 0 ?
                            normal : reverse;

                            instInteractive.transform.position = Vector3.zero.Change(y: yPosition);
                            movingObject.CorrectStartValues();
                        }
                    ).AddTo(subscribers);
                }

                (new Pool(poolingObjects), interactiveData).AddTo(pools);
            }
        }

        playerScoreLogic.ScoreBy1Time = 1 + (long)Mathf.Round(allScore / (1 + (float)selectableCount) * (1 + selectableCount * 0.1f));

        return pools;
    }

    private void OnDestroy()
    {
        subscribers.Dispose();
    }
}