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
        // 表示金額と実際の金額が違う場合
        if (m_displayMoney != m_money)
        {
            // 表示金額と実際の差額を埋める
            // 差額の大きさによって加算量を変える
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
