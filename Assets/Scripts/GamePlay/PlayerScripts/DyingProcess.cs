using UnityEngine;

public class DyingProcess : MonoBehaviour
{
    [SerializeField] private PlayerScoreGraphics scoreGraphics;
    [SerializeField] private MoneyGraphics moneyGraphics;

    [SerializeField] private PlayerScoreLogic scoreLogic;
    [SerializeField] private PlayerMoneyLogic moneyLogic;

    public void DetectDie()
    {
        DB.Data.Score.ToMoney(out long levelMoney, out long interactiveMoney, out long equipMoney);
        moneyLogic.AddMoney(levelMoney, interactiveMoney, equipMoney);

        moneyGraphics.SetMoney(levelMoney, interactiveMoney, equipMoney);
        scoreGraphics.SetData(scoreLogic.Score);
    }
}
