using UnityEngine;

public class Menu : MonoBehaviour
{
    public enum MenuType
    {
        Visualdictionary,   // �}��
        Inventory,          // �C���x���g��

        Length
    }

    [SerializeField] GameObject[] m_menuPage = new GameObject[(int)MenuType.Length];
    [SerializeField] MenuType m_startMenu;

    private void Awake()
    {
        for (int i = 0; i < (int)MenuType.Length; ++i)
        {
            m_menuPage[i].SetActive(false);
        }
        m_menuPage[(int)m_startMenu].SetActive(true);
    }

	private void Update()
	{
		if (InputSystem.GetInputMenuButtonDown("LB"))
		{
			// ���{�^���őO�̃y�[�W��
			MenuType type = (MenuType)(((int)m_startMenu - 1 + (int)MenuType.Length) % (int)MenuType.Length);
			SetMenu(type);
			m_startMenu = type;
		}
		else if (InputSystem.GetInputMenuButtonDown("RB"))
		{
			// �E�{�^���Ŏ��̃y�[�W��
			MenuType type = (MenuType)(((int)m_startMenu + 1) % (int)MenuType.Length);
			SetMenu(type);
			m_startMenu = type;
		}
	}

    public void SetMenu(MenuType type)
    {
        for (int i = 0; i < (int)MenuType.Length; ++ i)
        {
            // �I�������y�[�W�ȊO��false�ɂ���
            m_menuPage[i].SetActive(type == (MenuType)i);
        }
    }
}
