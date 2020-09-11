using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreGraphics : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    public void SetData(object score)
    {
        scoreText.text = score.ToString();
    }
}
