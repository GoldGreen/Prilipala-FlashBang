using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class SettingScrolling<T> : MonoBehaviour
where T : BaseObjectData<T>
{
    [SerializeField] private TouchDetector touchDetector;
    [SerializeField] private float speed = 66;
    [SerializeField] private float minLenToChangePage = 8;

    [SerializeField] private Transform targetTransform;
    [SerializeField] private ScaleAnimation informationPanel;
    [SerializeField] private ScaleAnimation openPanel;
    [SerializeField] private Button[] pagePuttons;

    [SerializeField] private Color pageButtonColor;
    [SerializeField] private Color currentPageButtonColor;

    protected abstract SettingsCreator<T> Creator { get; }

    private Vector3 nextPosition;

    private int currentIndex = 0;
    private int leftIndex = 0;
    public int Count => rightIndex + 1;
    private int rightIndex;

    private bool isMove = false;

    protected readonly UnityEvent<int> onPageChanging = new UnityEvent<int>();
    protected readonly UnityEvent<int> onPageChanged = new UnityEvent<int>();

    protected IDisposableCollection subscribers = new Disposables();

    protected virtual void Start()
    {
        rightIndex = Mathf.CeilToInt(Creator.Count / (float)Creator.ItemsInPage) - 1;
        nextPosition = targetTransform.position;

        for (int i = 0; i < pagePuttons.Length; i++)
        {
            int index = i;
            pagePuttons[i].onClick.Subscribe(() => MoveToPage(index)).AddTo(subscribers);
        }

        MoveToPage(0);

        touchDetector.OnDraging.Subscribe(DragingHandler).AddTo(subscribers);
        touchDetector.OnDraged.Subscribe(DragedHandler).AddTo(subscribers);
    }

    private void Update()
    {
        if (isMove)
        {
            targetTransform.position = Vector3.MoveTowards(targetTransform.position, nextPosition, speed * Time.deltaTime);

            if (Vector3.Distance(targetTransform.position, nextPosition) < 0.015f)
            {
                isMove = false;
                onPageChanged.Invoke(currentIndex);
            }
        }
    }

    public void DragingHandler(Vector2 delta)
    {
        if (!isMove && !informationPanel.IsShowing && !openPanel.IsShowing)
        {
            targetTransform.position = targetTransform.position
                .Delta(x: delta.x);
        }
    }

    public void DragedHandler()
    {
        if (!isMove && !informationPanel.IsShowing && !openPanel.IsShowing)
        {
            int nextIndex = currentIndex;

            if (nextPosition.x - targetTransform.position.x > minLenToChangePage
            && currentIndex < rightIndex)
            {
                nextIndex++;
            }
            else if (targetTransform.position.x - nextPosition.x > minLenToChangePage
            && currentIndex > leftIndex)
            {
                nextIndex--;
            }

            if (nextIndex != currentIndex)
            {
                MoveToPage(nextIndex);
            }

            isMove = true;
        }
    }

    public void MoveToPage(int newIndex)
    {
        if (!isMove && newIndex >= leftIndex && newIndex <= rightIndex)
        {
            onPageChanging.Invoke(newIndex);
            isMove = true;
            nextPosition.x += (currentIndex - newIndex) * Creator.PageSize;

            pagePuttons[currentIndex].image.color = pageButtonColor;
            currentIndex = newIndex;
            pagePuttons[currentIndex].image.color = currentPageButtonColor;
        }
    }

    protected virtual void OnDestroy()
    {
        subscribers.Dispose();
    }
}
