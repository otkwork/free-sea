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

    private void Start()
    {
        
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
