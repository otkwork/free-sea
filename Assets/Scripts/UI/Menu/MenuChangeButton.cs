using UnityEngine;

public class MenuChangeButton : MonoBehaviour
{
    [SerializeField] private Menu m_menu;
    [SerializeField] private Menu.MenuType m_type;

	// �N���b�N���ꂽ�Ƃ��ɌĂ΂�郁�\�b�h
	public void OnClick()
    {
        m_menu.SetMenu(m_type);
    }
}
