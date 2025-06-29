using TMPro;
using UnityEngine;

public class Money : MonoBehaviour
{
    private TextMeshProUGUI m_moneyText;
    
    private const int StartMoney = 100;

    static private int m_money;
    private int m_displayMoney;
    // Start is called before the first frame update
    void Start()
    {
        m_money = StartMoney;
        m_displayMoney = m_money;
        m_moneyText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // �\�����z�Ǝ��ۂ̋��z���Ⴄ�ꍇ
        if (m_displayMoney != m_money)
        {
            // �\�����z�Ǝ��ۂ̍��z�𖄂߂�
            // ���z�̑傫���ɂ���ĉ��Z�ʂ�ς���
            m_displayMoney += m_displayMoney < m_money ? 1 : -1;
        }
        m_moneyText.text = "$" + m_displayMoney.ToString();
    }

    private void OnDisable()
    {
        m_displayMoney = m_money;
    }

    static public void AddMoney(int value)
    {
        m_money += value;
    }

    static public void UseMoney(int value)
    {
        m_money -= value;
    }

    static public int GetMoney()
	{
        return m_money;
	}
}
