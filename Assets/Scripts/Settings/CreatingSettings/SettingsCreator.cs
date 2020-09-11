using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class SettingsCreator<T> : MonoBehaviour
where T : BaseObjectData
{
    public float SettingItemSize => settingItemSize;
    [SerializeField] private float settingItemSize;

    public int ItemsInRow => itemsInRow;
    [SerializeField] private int itemsInRow = 2;

    public int PageSize => pageSize;
    [SerializeField] private int pageSize = 22;

    public int ItemsInPage => itemsInPage;
    [SerializeField] private int itemsInPage = 6;

    public float Margin => itemsInPage;
    [SerializeField] private float margin = 1.0f;

    public float Padding => padding;
    [SerializeField] private float padding = 1.0f;
    private Vector2 paddingVector;

    private GridPositions[] grids;

    public int Count { get; private set; }
    public float FullIconSize { get; private set; } = 0;

    [SerializeField] private GameObject[] iconsPrefabs;

    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private MoneyGraphics moneyGraphics;

    [SerializeField] private UnityEvent OnDragging;

    protected (GameObject gameObject, T data)[] icons;

    protected abstract InformationPanel<T> InfoPanelData { get; }
    protected abstract ScaleAnimation InfoAnimation { get; }

    protected abstract OpenPanel<T> OpenPanelData { get; }
    protected abstract ScaleAnimation OpenAnimation { get; }

    private IDisposableCollection subscribers = new Disposables();
    private readonly ICollection<IUpdatable> updatables = new List<IUpdatable>();

    private Money money;

    private new Transform transform;
    private VisibleObject visible;

    protected virtual void Awake()
    {
        transform = GetComponent<Transform>();
        visible = GetComponent<VisibleObject>();

        paddingVector = new Vector2(padding, padding);

        FullIconSize = settingItemSize + margin;

        Count = iconsPrefabs.Length;
        icons = new (GameObject gameObject, T data)[Count];
        grids = new GridPositions[Count / itemsInPage + (Count % itemsInPage == 0 ? 0 : 1)];

        money = DB.Data.Money;
        money.AddTo(updatables);

        money.OnDataChanged
        .Subscribe(x => moneyGraphics.SetMoney(x.Level, x.Interactive, x.Equip))
        .AddTo(subscribers);
    }

    protected virtual void Start()
    {
        CreateSettingItems();
        updatables.ForEach(x => x.Update());
    }

    protected virtual (GameObject gameObject, T data)[] SortIcons => icons;

    private void CreateSettingItems()
    {
        for (int i = 0; i < Count; i++)
        {
            var dataBaseObject = DB.Data.Find<T>(iconsPrefabs[i].GetComponent<IHaveIdCode>().IdCode);
            icons[i] = (CreateIcon(iconsPrefabs[i], dataBaseObject), dataBaseObject);

            dataBaseObject.AddTo(updatables);
        }

        icons = SortIcons;

        var allItemsCount = Count;
        var started = Vector2.zero;
        var size = new Vector2(FullIconSize, FullIconSize);

        for (int i = 0; allItemsCount > 0; i++)
        {
            int cols = allItemsCount >= itemsInRow ? itemsInRow : allItemsCount;
            int rows = 1;
            allItemsCount -= cols;

            //Пока можно добавлять строки - добавляем ¯\_(ツ)_/¯
            for (; allItemsCount > 0 && rows < itemsInPage / itemsInRow; rows++)
                allItemsCount -= cols;

            grids[i] = new GridPositions(rows, cols, size, started);
            started.x += pageSize;
        }

        for (int i = 0; i < Count; i++)
            icons[i].gameObject.transform.position = grids[i / itemsInPage][i % itemsInPage];
    }

    private GameObject CreateIcon(GameObject iconPrefab, T dataBaseObject)
    {
        var cell = Instantiate(cellPrefab, transform, true);
        cell.name = dataBaseObject.Name;

        var cellRectTransform = cell.GetComponent<RectTransform>();
        cellRectTransform.sizeDelta = Vector2.one * settingItemSize;

        var icon = Instantiate(iconPrefab, cell.transform);
        icon.name = "Icon";

        var iconRect = icon.GetComponent<RectTransform>();

        iconRect.offsetMin = paddingVector;
        iconRect.offsetMax = -paddingVector;

        var settingItemLogic = cell.GetComponent<SettingsItemLogic>();
        var settingItemGraphics = cell.GetComponent<SettingItemGraphics>();

        var iconImage = icon.GetComponent<Image>();
        settingItemGraphics.Icon = iconImage;

        settingItemLogic.OnDragingMuchTime.Subscribe
        (
            () =>
            {
                OnDragging.Invoke();

                InfoPanelData.SetInfo(dataBaseObject);
                InfoPanelData.Icon = iconImage.sprite;

                OpenPanelData.SetInfo(dataBaseObject);
                OpenPanelData.Icon = iconImage.sprite;

                visible.SetVisible(false);

                (dataBaseObject.IsOpened ? InfoAnimation : OpenAnimation).Show();
            }
        ).AddTo(subscribers);

        settingItemLogic.OnDown.Subscribe(settingItemGraphics.Down).AddTo(subscribers);
        settingItemLogic.OnUp.Subscribe(settingItemGraphics.Up).AddTo(subscribers);
        settingItemLogic.OnClick.Subscribe(dataBaseObject.ReverseSelection).AddTo(subscribers);

        dataBaseObject.OnDataChanged
        .Subscribe(x => InfoPanelData.SetInfo(x as T))
        .AddTo(subscribers);

        dataBaseObject.OnDataChanged
        .Subscribe(x => OpenPanelData.SetInfo(x as T))
        .AddTo(subscribers);

        dataBaseObject.OnDataChanged
        .Subscribe(x => settingItemGraphics.SetData(x.IsOpened, x.IsSelected))
        .AddTo(subscribers);

        return cell;
    }

    protected virtual void OnDestroy()
    {
        subscribers.Dispose();
    }
}