using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenSceneScript : MonoBehaviour
{
    [SerializeField] private Scene scene;
    public void OpenScene()
    {
        SceneManager.LoadScene((int)scene);
    }

    public void AsyncOpenScene(float delay)
    {
        AsyncLoadScene(delay).Start(this);
    }

    public void AsyncOpenScene()
    {
        AsyncLoadScene(.0f).Start(this);
    }

    private IEnumerator AsyncLoadScene(float delay)
    {
        var asyncOperator = SceneManager.LoadSceneAsync((int)scene);
        asyncOperator.allowSceneActivation = false;
        while (asyncOperator.progress < .9f)
        {
            yield return null;
        }
        CoroutineT.Single(() => asyncOperator.allowSceneActivation = true, delay).Start(this);
    }
}
