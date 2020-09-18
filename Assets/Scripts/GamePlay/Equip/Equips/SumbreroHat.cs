using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

public class SumbreroHat : Hat, IHave<IdCode>, IChange, IChangePhysics, ILinkWithTouchDetector, ILinkWithShower
{
    public IdCode Item => IdCode.Submrero;

    private ReloadingEntity<EquipData> reloadingEntity;

    [SerializeField] private Sprite destroyIcon;

    [SerializeField] private GameObject sightPrefab;

    private GameObject sight;
    private SightLogic sightLogic;

    public ICollection<Vector2> directions = new List<Vector2>();
    private int difference = 15;

    private IDisposableCollection subscribers = new Disposables();

    private Rigidbody2D playerRigidbody;

    public EffectShower EffectShower { get; set; }
    public TouchDetector TouchDetector { get; set; }

    private void Awake()
    {
        reloadingEntity = new ReloadingEntity<EquipData>(x => x.ReloadingDestroyTime);
    }

    public override void SetData(EquipData equipData)
    {
        base.SetData(equipData);
        equipData.SetTo(reloadingEntity);
    }

    private void Start()
    {
        sight = Instantiate(sightPrefab);
        sight.name = "Sight";

        sightLogic = sight.GetComponent<SightLogic>();
        sight.SetActive(false);

        for (int degree = -difference; degree < 180 + difference; degree++)
        {
            float angle = degree * Deg2Rad;
            new Vector2(Cos(angle), Sin(angle)).AddTo(directions);
        }

        EffectShower.AddOrUpdate(destroyIcon, EffectType.Reloadable, reloadingEntity.ReloadingTime);

        reloadingEntity.OnReloaded.Subscribe(FindAim).AddTo(subscribers);
    }

    private void FixedUpdate()
    {
        FindAim();
    }

    private void FindAim()
    {
        if (reloadingEntity.IsReloaded)
        {
            GameObject aim = null;
            float minDistanse = float.MaxValue;

            foreach (var direction in directions)
            {
                var ray = Physics2D.Raycast(playerRigidbody.position, direction, 50);

                if (ray && ray.transform.gameObject.activeSelf
                && ray.distance < minDistanse)
                {
                    minDistanse = ray.distance;
                    aim = ray.transform.gameObject;
                }
            }

            if (minDistanse < float.MaxValue)
            {
                sight.SetActive(true);
                sightLogic.SetAim(aim);
                return;
            }
        }

        sight.SetActive(false);
    }

    public void Change(Transform playerTransform, Rigidbody2D playerRigitBody, PlayerControl control)
    {
        playerRigidbody = playerRigitBody;
    }

    public void Change()
    {
        TouchDetector.OnClick.Subscribe(TryUseEffect).AddTo(subscribers);
    }

    private void TryUseEffect()
    {
        if (reloadingEntity.IsReloaded && sight.activeSelf)
        {
            sightLogic.DestroyAim();
            reloadingEntity.StartReload(this);
            EffectShower.AddOrUpdate(destroyIcon, EffectType.Reloadable, reloadingEntity.ReloadingTime);
        }
    }

    private void OnDestroy()
    {
        subscribers.Dispose();
    }
}