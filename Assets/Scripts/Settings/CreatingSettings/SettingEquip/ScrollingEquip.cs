using UnityEngine;

public class ScrollingEquip : SettingScrolling<EquipData>
{
    [SerializeField] private EquipSettingCreator equipCreator;
    protected override SettingsCreator<EquipData> Creator => equipCreator;
}