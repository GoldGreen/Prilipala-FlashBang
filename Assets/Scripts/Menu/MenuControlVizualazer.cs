using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuControlVizualazer : MonoBehaviour
{
    private const string GameName = "Prilipala!";
    [SerializeField] private Text openGameProgres;
    [SerializeField] private Text openInteractiveProgres;
    [SerializeField] private Text openEquipProgres;

    [SerializeField] private Text nextScenePresenter;

    [SerializeField] private UIGradient backGround;

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

    private void Start()
    {
        backGround.Color2 = defaultColor;
        backGround.Color1 = defaultColor;
    }

    public void SetProgress((float game, float interactive, float equip) progress)
    {
        var source = Lerp(defaultColor, gameColor, progress.game);
        source = Lerp(source, interactiveColor, progress.interactive);
        source = Lerp(source, equipColor, progress.equip);
        backGround.Color1 = source;
        backGround.Angle = Mathf.Atan2(-progress.game, progress.interactive - progress.equip) * Mathf.Rad2Deg + 90;

        openGameProgres.text = FormatProgress(progress.game);

        openInteractiveProgres.text = FormatProgress(progress.interactive);
        openEquipProgres.text = FormatProgress(progress.equip);

        float max = new[] { progress.game, progress.interactive, progress.equip }.Max();

        if (max != 0)
        {

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
        else
        {
            nextScenePresenter.text = GameName;
            var color = nextScenePresenter.color;
            color.a = 1;
            nextScenePresenter.color = color;
        }


        backGround.UpdateGradient();
    }

    private Color Lerp(Color source, Color color, float value)
    {
        return Color.Lerp(source, color, value);
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
