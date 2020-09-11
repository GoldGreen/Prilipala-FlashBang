using UnityEngine;
using UnityEngine.Events;

public class MenuControl : MonoBehaviour
{
    private struct Progress
    {
        public Vector2 Max { get; }
        public readonly UnityEvent<(float game, float interactive, float equip)> OnProgressChanged;
        public float Game => game > 0 ? game / Max.y : 0;
        private float game;

        public float Interactive => interactive > 0 ? interactive / Max.x : 0;
        private float interactive;

        public float Equip => interactive < 0 ? -interactive / Max.x : 0;
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
    }

    [SerializeField] private TouchDetector touchDetector;
    [SerializeField] private RectTransform moving;
    [SerializeField] private MenuControlVizualazer menuControlVizualazer;

    [SerializeField] private UnityEvent onGameProgressFillUp;
    [SerializeField] private UnityEvent onInteractiveProgressFillUp;
    [SerializeField] private UnityEvent onEquipProgressFillUp;

    [SerializeField] private float lenToOpenScene;

    [SerializeField] private float childScale;
    [SerializeField] private float selfScale;

    private IDisposableCollection subscribers = new Disposables();
    private Progress progress;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        progress = new Progress(0.6f * lenToOpenScene, lenToOpenScene);
    }

    private void Start()
    {
        touchDetector.OnDraging.Subscribe(MoveCircle).AddTo(subscribers);
        touchDetector.OnDraged.Subscribe(Draged).AddTo(subscribers);

        progress.OnProgressChanged.Subscribe(ProgressChangedHandler).AddTo(subscribers);

        progress.OnProgressChanged
            .Subscribe(progress => menuControlVizualazer
                .SetProgress(progress.game, progress.interactive, progress.equip))
            .AddTo(subscribers);

        progress.Null();
    }

    private void MoveCircle(Vector2 delta)
    {
        moving.anchoredPosition += delta * childScale;
        rectTransform.anchoredPosition += delta * selfScale;

        progress.AddProgress(delta.y, delta.x, -delta.x);
    }

    private void FixedUpdate()
    {
        var angle = Mathf.Rad2Deg * moving.anchoredPosition.Atan2();

        if (angle != 0)
            angle -= 90;

        moving.eulerAngles = Vector3.zero.Change(z: angle);
    }

    private void Draged(DragedArgs args)
    {
        moving.anchoredPosition = Vector2.zero;
        rectTransform.anchoredPosition = Vector2.zero;
        progress.Null();
    }

    private void ProgressChangedHandler((float game, float interactive, float equip) arg)
    {
        if (arg.game >= 0.999f)
            onGameProgressFillUp.Invoke();
        else if (arg.interactive >= 0.999f)
            onInteractiveProgressFillUp.Invoke();
        else if (arg.equip >= 0.999f)
            onEquipProgressFillUp.Invoke();
    }

    private void OnDestroy()
    {
        subscribers.Dispose();
    }
}
