using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SettingsItemLogic : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent OnClick { get; private set; } = new UnityEvent();
    public UnityEvent OnTouchDown { get; private set; } = new UnityEvent();
    public UnityEvent OnTouchUp { get; private set; } = new UnityEvent();
    public UnityEvent OnDragingMuchTime { get; private set; } = new UnityEvent();

    [SerializeField] private float dragingTime;

    private bool draging;
    private bool clicked;
    private float time;

    public void OnPointerDown(PointerEventData eventData)
    {
        time = 0;
        clicked = true;
        draging = false;

        OnTouchDown?.Invoke();
    }

    private void Update()
    {
        if (clicked && !draging)
        {
            if (time >= dragingTime)
            {
                OnDragingMuchTime?.Invoke();
                draging = true;
            }
            else
            {
                time += Time.deltaTime;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (clicked)
        {
            if (!draging)
            {
                OnClick.Invoke();
            }

            OnTouchUp.Invoke();
            clicked = false;
        }
    }
}