using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuChangeButton : MonoBehaviour
{
    [SerializeField] Menu m_menu;
    [SerializeField] Menu.MenuType m_type;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        m_menu.SetMenu(m_type);
    }
}
