using UnityEngine;

public class ShopManager : MonoBehaviour {

    public void EnterSystem()
    {
        ShopController.Instance.EnterSystem();
    }

    public void ShowUpgrades()
    {
        ShopController.Instance.ShowUpgrades();
    }

    public void ExitShop()
    {
        ShopController.Instance.ExitShop();
    }
}
