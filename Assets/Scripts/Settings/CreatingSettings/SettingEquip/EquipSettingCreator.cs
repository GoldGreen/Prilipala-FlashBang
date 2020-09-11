using System.Linq;
using UnityEngine;

public class EquipSettingCreator : SettingsCreator<EquipData>
{
    [SerializeField] private EquipPanel infoPanel;
    protected override InformationPanel<EquipData> InfoPanelData => infoPanel;

    [SerializeField] private ScaleAnimation infoAnimation;
    protected override ScaleAnimation InfoAnimation => infoAnimation;

    [SerializeField] OpenEquipPanel openPanel;
    protected override OpenPanel<EquipData> OpenPanelData => openPanel;

    [SerializeField] private ScaleAnimation openAnimation;
    protected override ScaleAnimation OpenAnimation => openAnimation;

    protected override (GameObject gameObject, EquipData data)[] SortIcons
    => icons.OrderBy(x => x.data.Set)
    .ThenBy(x => x.data.TypeOfEquip)
    .ToArray();
}