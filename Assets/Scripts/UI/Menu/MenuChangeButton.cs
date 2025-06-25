using UnityEngine;

public class MenuChangeButton : MonoBehaviour
{
    [SerializeField] private Menu m_menu;
    [SerializeField] private Menu.MenuType m_type;

	// クリックされたときに呼ばれるメソッド
	public void OnClick()
    {
        m_menu.SetMenu(m_type);
    }
}
