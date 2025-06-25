using UnityEngine;

public class SellButton : MonoBehaviour
{
    [SerializeField] private Inventory m_inventory;
    public void Sell()
    {
        m_inventory.SellItem();
    }
}
