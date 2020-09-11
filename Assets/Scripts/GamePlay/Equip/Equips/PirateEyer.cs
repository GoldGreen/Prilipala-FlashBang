using System.Collections.Generic;
using UnityEngine;

public class PirateEyer : Eyer, IHaveIdCode, ILinkWithShower, ILinkWithTouchDetector, IChangePhysics
{
    public IdCode IdCode => IdCode.PirateEyer;
    private ReloadingEntity<EquipData> waveReloadingEntity;
    private float waveLivingTime;

    [SerializeField] private GameObject wavePrefab;
    private Pool wavesPool;
    private Transform waveTransform;

    [SerializeField] private Sprite waveSprite;

    private IDisposableCollection subscribers = new Disposables();

    private Transform playerTransform;

    public EffectShower EffectShower { get; set; }
    public TouchDetector TouchDetector { get; set; }

    private void Awake()
    => waveReloadingEntity = new ReloadingEntity<EquipData>(x => x.WaveReloadingTime);

    public override void SetData(EquipData equipData)
    {
        base.SetData(equipData);
        equipData.SetTo(waveReloadingEntity);
        waveLivingTime = equipData.WaveLivingTime;
    }

    private void Start()
    {
        wavesPool = new Pool(InstansiateWave());

        TouchDetector.OnDoubleClick.Subscribe(() =>
        {
            if (waveReloadingEntity.IsReloaded)
            {
                waveReloadingEntity.StartReload(this);

                wavesPool.PoolObject();
                EffectShower.AddOrUpdate(waveSprite, EffectType.Reloadable, waveReloadingEntity.ReloadingTime);
            }
        }).AddTo(subscribers);

        EffectShower.AddOrUpdate(waveSprite, EffectType.Reloadable, waveReloadingEntity.ReloadingTime);
    }

    private IEnumerable<IPoolable> InstansiateWave()
    {
        var wave = Instantiate(wavePrefab);
        wave.name = "Wave";

        waveTransform = wave.GetComponent<Transform>();

        var poolable = new Poolable(wave);

        poolable.OnFloated
        .Subscribe
        (
            () => CoroutineT.Single(() => wave.SetActive(false), waveLivingTime)
            .Start(this)
        );

        yield return poolable;
    }

    private void FixedUpdate() => waveTransform.position = playerTransform.position;

    public void Change(Transform playerTransform, Rigidbody2D playerRigitBody, PlayerControl control)
    => this.playerTransform = playerTransform;

    private void OnDestroy() => subscribers.Dispose();
}