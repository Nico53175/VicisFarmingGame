using UnityEngine;


public class ShopTrigger : MonoBehaviour
{
    public GameObject shopUI;
    public ShopManager shopManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OpenShop();
            shopManager.UpdateCurrencyText();
        }
    }

    private void OpenShop()
    {
        shopUI.SetActive(true);
    }

    public void CloseShop()
    {
        shopUI.SetActive(false);
    }
}