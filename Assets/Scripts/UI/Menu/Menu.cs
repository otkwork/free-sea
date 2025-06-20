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

	private void Update()
	{
		if (InputSystem.GetInputMenuButtonDown("LB"))
		{
			// 左ボタンで前のページへ
			MenuType type = (MenuType)(((int)m_startMenu - 1 + (int)MenuType.Length) % (int)MenuType.Length);
			SetMenu(type);
			m_startMenu = type;
		}
		else if (InputSystem.GetInputMenuButtonDown("RB"))
		{
			// 右ボタンで次のページへ
			MenuType type = (MenuType)(((int)m_startMenu + 1) % (int)MenuType.Length);
			SetMenu(type);
			m_startMenu = type;
		}
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
