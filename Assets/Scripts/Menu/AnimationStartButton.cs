using UnityEngine;

public class AnimationStartButton : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] sprites;

    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color animatedColor = Color.black;

    [SerializeField] private float animatedTime = 0.5f;

    private int currentColoredIndex = 0;

    private void Awake()
    {
        foreach (var sprite in sprites)
            sprite.color = defaultColor;
    }

    private void Start()
    {
        CoroutineT.InfiniteBefore(Animate, animatedTime).Start(this);
    }

    private void Animate()
    {
        sprites[currentColoredIndex].color = defaultColor;
        currentColoredIndex = (currentColoredIndex + 1) % sprites.Length;
        sprites[currentColoredIndex].color = animatedColor;
    }
}