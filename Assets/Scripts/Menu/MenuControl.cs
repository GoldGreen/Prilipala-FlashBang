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
    }

    [SerializeField] private TouchDetector touchDetector;
    [SerializeField] private RectTransform movingTransform;

    [SerializeField] private UnityEvent onGameProgressFillUp;
    [SerializeField] private UnityEvent onInteractiveProgressFillUp;
    [SerializeField] private UnityEvent onEquipProgressFillUp;
    [SerializeField] private UnityEvent<(float game, float interactive, float equip)> onProgressChanged;
    [SerializeField] private UnityEvent<float> onProgressGameChanged;
    [SerializeField] private UnityEvent<float> onProgressInreractiveChanged;
    [SerializeField] private UnityEvent<float> onProgressEquipChanged;

    [SerializeField] private float lenToOpenScene;

    [SerializeField] private float scale;
    [SerializeField] private float progressInteractiveAndEquipScale;

    private IDisposableCollection subscribers = new Disposables();
    private Progress progress;

    private void Awake()
    {
        progress = new Progress(progressInteractiveAndEquipScale * lenToOpenScene, lenToOpenScene);
    }

    private void Start()
    {
        touchDetector.OnDraging.Subscribe(DraggingHandler).AddTo(subscribers);
        touchDetector.OnDraged.Subscribe(DragedHandler).AddTo(subscribers);

        progress.OnProgressChanged.Subscribe(ProgressChangedHandler).AddTo(subscribers);
        progress.OnProgressChanged.Subscribe(onProgressChanged.Invoke).AddTo(subscribers);

        progress.OnProgressChanged.Subscribe(progress => onProgressGameChanged.Invoke(progress.game)).AddTo(subscribers);
        progress.OnProgressChanged.Subscribe(progress => onProgressInreractiveChanged.Invoke(progress.interactive)).AddTo(subscribers);
        progress.OnProgressChanged.Subscribe(progress => onProgressEquipChanged.Invoke(progress.equip)).AddTo(subscribers);

        progress.Null();
    }

    private void DraggingHandler(Vector2 delta)
    {
        movingTransform.anchoredPosition += delta * scale;
        progress.AddProgress(delta.y, delta.x, -delta.x);
    }

    private void Update()
    {
        float angle = Mathf.Rad2Deg * movingTransform.anchoredPosition.Multiply(x: 1 / progressInteractiveAndEquipScale).Atan2();

        if (angle == 0)
        {
            angle += 90;
        }

        movingTransform.eulerAngles = Vector3.zero.Change(z: angle);
    }

    private void DragedHandler(DragedArgs args)
    {
        movingTransform.anchoredPosition = Vector2.zero;
        progress.Null();
    }

    private void ProgressChangedHandler((float game, float interactive, float equip) arg)
    {
        if (arg.game >= 0.999f)
        {
            onGameProgressFillUp.Invoke();
        }
        else if (arg.interactive >= 0.999f)
        {
            onInteractiveProgressFillUp.Invoke();
        }
        else if (arg.equip >= 0.999f)
        {
            onEquipProgressFillUp.Invoke();
        }
    }

    private void OnDestroy()
    {
        subscribers.Dispose();
    }
}
