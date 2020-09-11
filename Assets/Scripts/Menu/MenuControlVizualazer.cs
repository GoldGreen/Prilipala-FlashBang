using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class MenuControlVizualazer : MonoBehaviour
{
    [SerializeField] private Text openGameProgres;
    [SerializeField] private Text openInteractiveProgres;
    [SerializeField] private Text openEquipProgres;

    [SerializeField] private Text nextScenePresenter;

    [SerializeField] private Image backGround;

    [SerializeField] private Color defaultColor;
    [SerializeField] private Color gameColor;
    [SerializeField] private Color interactiveColor;
    [SerializeField] private Color equipColor;

    private readonly IReadOnlyDictionary<Scene, string> sceneNames = new Dictionary<Scene, string>
    {
        { Scene.gamePlay , "Play!" },
        { Scene.interactiveSetting , "Interactive" },
        { Scene.equipSetting , "Equip" }
    };

    public void SetProgress(float gameProgress, float interactiveProgress, float equipProgress)
    {
        backGround.color = defaultColor;

        Lerp(gameColor, gameProgress);
        Lerp(interactiveColor, interactiveProgress);
        Lerp(equipColor, equipProgress);

        openGameProgres.text = FormatProgress(gameProgress);

        openInteractiveProgres.text = FormatProgress(interactiveProgress);
        openEquipProgres.text = FormatProgress(equipProgress);

        var max = new[] { gameProgress, interactiveProgress, equipProgress }.Max();

        if (max == gameProgress)
            ShowNextSceneText(Scene.gamePlay, LerpNormilizeFunc(gameProgress));
        else if (max == interactiveProgress)
            ShowNextSceneText(Scene.interactiveSetting, LerpNormilizeFunc(interactiveProgress));
        else if (max == equipProgress)
            ShowNextSceneText(Scene.equipSetting, LerpNormilizeFunc(equipProgress));
    }

    private void Lerp(Color color, float value)
    {
        backGround.color = Color.Lerp(backGround.color, color, value);
    }

    private float LerpNormilizeFunc(float value) => Mathf.Sqrt(value);

    private void ShowNextSceneText(Scene nextScene, float lerp)
    {
        nextScenePresenter.text = sceneNames[nextScene];
        var color = nextScenePresenter.color;
        color.a = lerp;
        nextScenePresenter.color = color;
    }

    private string FormatProgress(float progress) => $"{Mathf.RoundToInt(progress * 100)} %";
}
