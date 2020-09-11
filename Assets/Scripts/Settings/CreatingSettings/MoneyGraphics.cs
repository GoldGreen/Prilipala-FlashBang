using UnityEngine;
using UnityEngine.UI;

public class MoneyGraphics : MonoBehaviour
{
    [SerializeField] private Text levelMoneyText;
    [SerializeField] private Text interactiveMoneyText;
    [SerializeField] private Text equipMoneyText;

    public void SetMoney(float levelMoney, float interactiveMoney, float equipMoney)
    {
        levelMoneyText.text = levelMoney.Format();
        interactiveMoneyText.text = interactiveMoney.Format();
        equipMoneyText.text = equipMoney.Format();
    }
}
