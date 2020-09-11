using UnityEngine;

public class PlayerMoneyLogic : MonoBehaviour
{
    public void AddMoney(long levelMoney, long interactiveMoney, long equipMoney)
    => DB.Data.Money.AddMoney(levelMoney, interactiveMoney, equipMoney);
}
