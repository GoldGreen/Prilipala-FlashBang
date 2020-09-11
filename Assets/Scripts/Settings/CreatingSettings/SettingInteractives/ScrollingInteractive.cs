using UnityEngine;
using UnityEngine.UI;

public class ScrollingInteractive : SettingScrolling<InteractiveData>
{
    [SerializeField] private InteractiveSettingCreator InteractiveCreator;
    protected override SettingsCreator<InteractiveData> Creator => InteractiveCreator;

    [SerializeField] private Text tierPresenter;

    protected override void Start()
    {
        base.Start();
        onPageChanged.Subscribe(PageChangedHandler).AddTo(subscribers);
    }

    private void PageChangedHandler(int index)
    {
        tierPresenter.text = $"{index + 1} Page";
    }
}