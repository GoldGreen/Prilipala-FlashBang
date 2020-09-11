using UnityEngine;
using System.Collections.Generic;

public class EquipWearing : MonoBehaviour
{
    [SerializeField] private GameObject[] equipPrefabs;
    [SerializeField] private EffectShower effectShower;
    [SerializeField] private TouchDetector touchDetector;

    public IEnumerable<GameObject> GetPreparedEquips()
    {
        foreach (var equip in equipPrefabs)
        {
            var equipData = DB.Data.Find<EquipData>(equip.GetComponent<IHaveIdCode>().IdCode);

            if (equipData.IsSelected)
            {
                var instEquip = Instantiate(equip);
                instEquip.name = equipData.Name;

                instEquip.GetComponent<ILinkWithShower>().NotNull(x => x.EffectShower = effectShower);
                instEquip.GetComponent<ILinkWithTouchDetector>().NotNull(x => x.TouchDetector = touchDetector);
                instEquip.GetComponent<ISetData<EquipData>>()?.SetData(equipData);

                yield return instEquip;
            }
        }
    }
}
