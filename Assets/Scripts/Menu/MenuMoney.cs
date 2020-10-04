using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMoney : MonoBehaviour
{
    [SerializeField] private MoneyGraphics graphics;

    private void Start()
    {
        var money = DB.Data.Money;
        money.OnDataChanged.Subscribe(ShowMoney);
        money.Update();
    }

    private void ShowMoney(Money money)
    {
        graphics.SetMoney(money.Level, money.Interactive, money.Equip);
    }
}
