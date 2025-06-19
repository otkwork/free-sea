using UnityEngine;

public class MenuChangeButton : MonoBehaviour
{
    [SerializeField] Menu m_menu;
    [SerializeField] Menu.MenuType m_type;

	// クリックされたときに呼ばれるメソッド
	public void OnClick()
    {
        m_menu.SetMenu(m_type);
    }
}
