using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopClick : MonoBehaviour
{
	[SerializeField] private Confirmation m_confirmation; // 確認パネルのスクリプトへの参照
	[SerializeField] private Image m_image; // アイテムの画像を表示するImage
	[SerializeField] private TextMeshProUGUI m_amountText; // アイテムの数量を表示するText
	[SerializeField] private TextMeshProUGUI m_priceText; // アイテムの価格を表示するText

	[SerializeField] private Sprite m_sprite; // アイテムの画像
	[SerializeField] private int m_amount; // アイテムの数量
	[SerializeField] private int m_price; // アイテムの価格


	void Start()
	{
		m_image.sprite = m_sprite; // アイテムの画像を設定
		m_amountText.text = "×" + m_amount.ToString(); // アイテムの数量を設定
		m_priceText.text = "$" + m_price.ToString(); // アイテムの価格を設定
	}

	public void OnClickIcon()
	{
		m_confirmation.SetPanel(true); // 確認パネルを開く
		m_confirmation.SetConfirmationPanel(m_sprite, m_amount, m_price);
	}
}
