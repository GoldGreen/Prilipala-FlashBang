using UnityEngine;

public class VisibleObject : MonoBehaviour
{
    private new Transform transform;
    private Vector3 scale = Vector3.one;

    private void Awake()
    {
        transform = GetComponent<Transform>();
    }

    public void UpdateScale() => scale = transform.localScale;
    public void UpdateScale(Vector3 newScale) => scale = newScale;

    public void SetVisible(bool visible) => transform.localScale = visible ? scale : Vector3.zero;
}