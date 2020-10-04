using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    private struct Progress
    {
        public Vector2 Max { get; }
        public readonly UnityEvent<(float game, float interactive, float equip)> OnProgressChanged;
        public float Game => game > 0 ? CheckProgress(game / Max.y) : 0;
        private float game;

        public float Interactive => interactive > 0 ? CheckProgress(interactive / Max.x) : 0;
        private float interactive;

        public float Equip => interactive < 0 ? CheckProgress(-interactive / Max.x) : 0;
        private float equip;

        public Progress(float maxX, float maxY)
        {
            Max = new Vector2(maxX, maxY);
            game = interactive = equip = 0;
            OnProgressChanged = new UnityEvent<(float game, float interactive, float equip)>();
        }

        public void Null()
        {
            game = interactive = equip = 0;
            OnProgressChanged.Invoke((Game, Interactive, Equip));
        }

        public void SetProgress(float game, float interactive, float equip)
        {
            this.game = game;
            this.interactive = interactive;
            this.equip = equip;

            OnProgressChanged.Invoke((Game, Interactive, Equip));
        }

        public void AddProgress(float game, float interactive, float equip)
        {
            SetProgress(this.game + game, this.interactive + interactive, this.equip + equip);
        }

        private float CheckProgress(float value)
        {
            return value > 1.0f ? 1.0f : value;
        }

        public bool HaveFullProgress => new[] { Game, Interactive, Equip }.Any(x => x > 0.99f);
    }

    [SerializeField] private TouchDetector touchDetector;
    [SerializeField] private RectTransform movingTransform;
    private Vector2 defaultPosition;

    [SerializeField] private UnityEvent onGameProgressFillUp;
    [SerializeField] private UnityEvent onInteractiveProgressFillUp;
    [SerializeField] private UnityEvent onEquipProgressFillUp;
    [SerializeField] private UnityEvent<(float game, float interactive, float equip)> onProgressChanged;
    [SerializeField] private UnityEvent<float> onProgressGameChanged;
    [SerializeField] private UnityEvent<float> onProgressInreractiveChanged;
    [SerializeField] private UnityEvent<float> onProgressEquipChanged;

    [SerializeField] private float lenToOpenScene;
    [SerializeField] private float scale;

    private IDisposableCollection subscribers = new Disposables();
    private Progress progress;

    private bool isDragging = false;
    private bool progressFilledUp = false;

    private void Awake()
    {
        progress = new Progress(lenToOpenScene, lenToOpenScene);
        defaultPosition = movingTransform.anchoredPosition;
    }

    private void Start()
    {
        touchDetector.OnStartDraged.Subscribe(StartDraggingHandler);
        touchDetector.OnDraging.Subscribe(DraggingHandler).AddTo(subscribers);
        touchDetector.OnDraged.Subscribe(DragedHandler).AddTo(subscribers);

        progress.OnProgressChanged.Subscribe(ProgressChangedHandler).AddTo(subscribers);
        progress.OnProgressChanged.Subscribe(onProgressChanged.Invoke).AddTo(subscribers);

        progress.OnProgressChanged.Subscribe(progress => onProgressGameChanged.Invoke(progress.game)).AddTo(subscribers);
        progress.OnProgressChanged.Subscribe(progress => onProgressInreractiveChanged.Invoke(progress.interactive)).AddTo(subscribers);
        progress.OnProgressChanged.Subscribe(progress => onProgressEquipChanged.Invoke(progress.equip)).AddTo(subscribers);

        progress.Null();
    }

    private void StartDraggingHandler(Vector2 position)
    {
        if (!progressFilledUp)
        {
            movingTransform.anchoredPosition = defaultPosition;
            isDragging = true;
        }
    }

    private void DraggingHandler(Vector2 delta)
    {
        if (!progressFilledUp)
        {
            movingTransform.anchoredPosition += delta * scale;
            progress.AddProgress(delta.y, delta.x, -delta.x);
        }
    }

    private void Update()
    {
        if (!isDragging)
        {
            movingTransform.anchoredPosition = Vector2.Lerp(movingTransform.anchoredPosition, defaultPosition, 20 * Time.deltaTime);
            if ((movingTransform.anchoredPosition - defaultPosition).magnitude < 0.1f)
            {
                movingTransform.anchoredPosition = defaultPosition;
                isDragging = true;
            }
        }

        float angle = Mathf.Rad2Deg * (movingTransform.anchoredPosition - defaultPosition).Atan2();

        if (angle == 0)
        {
            angle += 90;
        }

        movingTransform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void DragedHandler(DragedArgs args)
    {
        if (!progressFilledUp)
        {
            progress.Null();
            isDragging = false;
        }
    }

    private void ProgressChangedHandler((float game, float interactive, float equip) arg)
    {
        if (!progressFilledUp)
        {
            if (arg.game > 0.999f)
            {
                onGameProgressFillUp.Invoke();
                progressFilledUp = true;
            }
            else if (arg.interactive > 0.999f)
            {
                onInteractiveProgressFillUp.Invoke();
                progressFilledUp = true;
            }
            else if (arg.equip > 0.999f)
            {
                onEquipProgressFillUp.Invoke();
                progressFilledUp = true;
            }
        }
    }

    private void OnDestroy()
    {
        subscribers.Dispose();
    }
}
