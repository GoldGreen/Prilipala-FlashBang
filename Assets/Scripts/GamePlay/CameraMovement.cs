using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject target;

    [SerializeField] private float deltaY;
    [SerializeField] private float speed;

    private float targetPositionY;
    private Vector3 position;

    private new Transform transform;
    private Transform targetTransform;

    private void Awake()
    {
        transform = GetComponent<Transform>();
        targetTransform = target.GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        targetPositionY = targetTransform.position.y + deltaY;

        position = transform.position;
        position.y += (targetPositionY - position.y) * speed * Time.deltaTime;

        transform.position = position;
    }
}