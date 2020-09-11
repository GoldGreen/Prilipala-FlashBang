using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenSceneScript : MonoBehaviour
{
    [SerializeField] private Scene scene;
    public void OpenScene() => SceneManager.LoadScene((int)scene);
}
