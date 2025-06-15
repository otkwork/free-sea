using UnityEngine;

public class Menu : MonoBehaviour
{
    public enum MenuType
    {
        Visualdictionary,   // 図鑑
        Inventory,          // インベントリ

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
            // 選択したページ以外をfalseにする
            m_menuPage[i].SetActive(type == (MenuType)i);
        }
    }
}
