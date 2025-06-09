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

    public void SetMenu(MenuType type)
    {
        for (int i = 0; i < (int)MenuType.Length; ++ i)
        {
            // 選択したページ以外をfalseにする
            m_menuPage[i].SetActive(type == (MenuType)i);
        }
    }
}
