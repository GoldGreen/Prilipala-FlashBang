using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveSettingCreator : SettingsCreator<InteractiveData>
{
    [SerializeField] private InteractivePanel infoPanel;
    protected override InformationPanel<InteractiveData> InfoPanelData => infoPanel;

    [SerializeField] private ScaleAnimation infoAnimation;
    protected override ScaleAnimation InfoAnimation => infoAnimation;

    [SerializeField] OpenInteractivePanel openPanel;
    protected override OpenPanel<InteractiveData> OpenPanelData => openPanel;

    [SerializeField] private ScaleAnimation openAnimation;
    protected override ScaleAnimation OpenAnimation => openAnimation;
       
    protected override (GameObject gameObject, InteractiveData data)[] SortIcons
    => icons.OrderBy(x => x.data.Tier)
       .ThenBy(x => x.data.DamageEntityExist ? (int)x.data.DamageType : (int)DamageType.physical + 1)
       .ThenBy(x => x.data.Name)
       .ToArray();
}