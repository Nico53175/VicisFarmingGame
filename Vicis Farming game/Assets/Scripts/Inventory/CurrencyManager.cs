using UnityEngine;



public class CurrencyManager : MonoBehaviour
{
    public int startingCurrency = 100;
    private int currentCurrency;

    private void Awake()
    {
        currentCurrency = startingCurrency;
    }

    public int GetCurrentCurrency()
    {
        return currentCurrency;
    }

    public void EarnCurrency(int amount)
    {
        currentCurrency += amount;
    }

    public bool SpendCurrency(int amount)
    {
        if (currentCurrency >= amount)
        {
            currentCurrency -= amount;
            return true; // Purchase successful
        }
        else
        {
            return false; // Insufficient currency
        }
    }
}