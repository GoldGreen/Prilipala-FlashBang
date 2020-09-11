using UnityEngine;

public class WallsCollider : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    private new Transform transform;

    private void Awake()
    {
        transform = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        transform.position = playerTransform.position.Change(x: 0);
    }
}
