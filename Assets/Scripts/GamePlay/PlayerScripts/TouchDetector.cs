using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TouchDetector : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] private new Camera camera;
    [SerializeField] private float clickDelay = 0.3f;

    public UnityEvent<Vector2> OnStartDraged = new UnityEvent<Vector2>();
    public UnityEvent<DragedArgs> OnDraged = new UnityEvent<DragedArgs>();
    public UnityEvent<Vector2> OnDraging = new UnityEvent<Vector2>();
    public UnityEvent OnClick = new UnityEvent();
    public UnityEvent OnDoubleClick = new UnityEvent();

    private int clickedCount = 0;

    private Vector2 startPosition;
    private Vector2 lastPosition;

    private Vector2 endPosition;
    private bool draged = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = camera.ScreenToWorldPoint(eventData.position);
        lastPosition = startPosition;
        draged = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var newPosition = (Vector2)camera.ScreenToWorldPoint(eventData.position);
        OnDraging.Invoke(newPosition - lastPosition);
        lastPosition = newPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draged = false;
        endPosition = camera.ScreenToWorldPoint(eventData.position);

        OnDraged.Invoke
        (
            new DragedArgs
            {
                StartPosition = startPosition,
                EndPositon = endPosition
            }
        );
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!draged)
        {
            clickedCount++;

            if (clickedCount <= 2)
            {
                CoroutineT.Single(CheckClick, clickDelay).Start(this);
            }
        }
    }

    private void CheckClick()
    {
        StopAllCoroutines();

        if (clickedCount == 1)
            OnClick.Invoke();

        if (clickedCount == 2)
            OnDoubleClick.Invoke();

        clickedCount = 0;
    }
}
