using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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

    public void SetProgress((float game, float interactive, float equip) progress)
    {
        backGround.color = defaultColor;

        Lerp(gameColor, progress.game);
        Lerp(interactiveColor, progress.interactive);
        Lerp(equipColor, progress.equip);

        openGameProgres.text = FormatProgress(progress.game);

        openInteractiveProgres.text = FormatProgress(progress.interactive);
        openEquipProgres.text = FormatProgress(progress.equip);

        float max = new[] { progress.game, progress.interactive, progress.equip }.Max();

        if (max == progress.game)
        {
            ShowNextSceneText(Scene.gamePlay, LerpNormilizeFunc(progress.game));
        }
        else if (max == progress.interactive)
        {
            ShowNextSceneText(Scene.interactiveSetting, LerpNormilizeFunc(progress.interactive));
        }
        else if (max == progress.equip)
        {
            ShowNextSceneText(Scene.equipSetting, LerpNormilizeFunc(progress.equip));
        }
    }

    private void Lerp(Color color, float value)
    {
        backGround.color = Color.Lerp(backGround.color, color, value);
    }

    private float LerpNormilizeFunc(float value)
    {
        return Mathf.Sqrt(value);
    }

    private void ShowNextSceneText(Scene nextScene, float lerp)
    {
        nextScenePresenter.text = sceneNames[nextScene];
        var color = nextScenePresenter.color;
        color.a = lerp;
        nextScenePresenter.color = color;
    }

    private string FormatProgress(float progress)
    {
        return $"{Mathf.RoundToInt(progress * 100)} %";
    }
}
