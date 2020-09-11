using UnityEngine;

public class ScaleAnimation : MonoBehaviour
{
    [SerializeField] private float time = 0.01f;
    [SerializeField] private float openTransition = 0.2f;
    [SerializeField] private float closeTransition = 0.15f;

    public Vector3 Scale { get; set; } = Vector3.one;
    private int count;
    private Vector3 vectorSpeed;

    public bool IsShowing { get; private set; }

    private new Transform transform;

    private void Awake()
    {
        transform = GetComponent<Transform>();
    }

    public void Show()
    {
        IsShowing = true;
        transform.localScale = Vector3.zero;

        count = (int)Mathf.Round(openTransition / time);
        vectorSpeed = Scale / count;
        gameObject.SetActive(true);
        CoroutineT.For(AddSpeed, count, time).Start(this);
    }

    public void Close()
    {
        IsShowing = false;

        transform.localScale = Scale;

        count = (int)Mathf.Round(closeTransition / time);
        vectorSpeed = Scale / count;
        CoroutineT.For(RemoveSpeed, count, time, () => gameObject.SetActive(false)).Start(this);
    }

    private void AddSpeed()
    {
        transform.localScale += vectorSpeed;

        if (transform.localScale.x > Scale.x)
            transform.localScale = Scale;
    }

    private void RemoveSpeed()
    {
        transform.localScale -= vectorSpeed;

        if (transform.localScale.x < 0)
            transform.localScale = Vector3.zero;
    }
}
