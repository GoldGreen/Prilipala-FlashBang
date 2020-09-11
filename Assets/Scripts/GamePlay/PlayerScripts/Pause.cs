using UnityEngine;
using UnityEngine.Events;

public class Pause : MonoBehaviour
{
    [SerializeField] private UnityEvent pauseStart;
    [SerializeField] private UnityEvent pauseEnd;

    public static bool IsPouse { get; private set; } = false;

    public void ReflectPause()
    {
        if (IsPouse)
        {
            Time.timeScale = 1;
            StartCoroutine(CoroutineT.Single
            (
                () =>
                {
                    IsPouse = false;
                    PauseChanged();
                }, 0.1f
            ));
        }
        else
        {
            Time.timeScale = 0;
            IsPouse = true;
            PauseChanged();
        }
    }

    public void StartPouse()
    {
        IsPouse = false;
        ReflectPause();
    }

    private void PauseChanged()
    {
        (IsPouse ? pauseStart : pauseEnd)?.Invoke();
    }

    private void OnDestroy()
    {
        IsPouse = false;
        Time.timeScale = 1;
    }
}
