using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] private Vector3 speed;
    [SerializeField] private Vector3 startDelta;

    public Vector3 Speed { get; set; }
    public Vector3 StartDelta { get; set; }

    private new Transform transform;

    private void Awake()
    {
        transform = GetComponent<Transform>();

        CorrectStartValues();
    }

    public void CorrectStartValues()
    {
        Speed = speed;
        StartDelta = startDelta;
        UpdateSpeedAndDelta();
        transform.position += StartDelta;
    }

    public void UpdateSpeedAndDelta()
    {
        int direction = 1;

        if (transform.rotation.y < -0.5f || transform.rotation.y > 0.5f)
            direction = -1;

        Speed = Speed.Change(x: Speed.x * direction);
        StartDelta = StartDelta.Change(x: StartDelta.x * direction);
    }

    private void FixedUpdate()
    {
        transform.position += (Speed * Time.fixedDeltaTime);
    }
}
