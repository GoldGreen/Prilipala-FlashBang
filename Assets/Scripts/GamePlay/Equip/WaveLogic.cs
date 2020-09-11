using UnityEngine;

public class WaveLogic : MonoBehaviour, IRepoolable
{
    private ScaleAnimation scaleAnimation;

    private void Awake()
    {
        scaleAnimation = GetComponent<ScaleAnimation>();
        scaleAnimation.Scale = transform.localScale;
    }

    public void Repool()
    {
        scaleAnimation.Show();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        collider.GetComponent<IDestroyedByWave>()?.Destroy();
    }
}
