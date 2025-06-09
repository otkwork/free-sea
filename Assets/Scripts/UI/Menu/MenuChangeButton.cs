using UnityEngine;

public class MenuChangeButton : MonoBehaviour
{
    [SerializeField] Menu m_menu;
    [SerializeField] Menu.MenuType m_type;

    public void OnClick()
    {
        m_menu.SetMenu(m_type);
    }
}
