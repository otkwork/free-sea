using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationPanel : MonoBehaviour
{
	[SerializeField] private Image m_image; // アイテムの画像  
	[SerializeField] private TextMeshProUGUI m_amountText; // アイテムの数量  
	[SerializeField] private TextMeshProUGUI m_priceText; // アイテムの価格  
	[SerializeField] private TextMeshProUGUI m_descriptionText;
	[SerializeField] private TextMeshProUGUI m_confirmationText; // 確認メッセージのテキスト

	// 文字列の中に変数を埋め込むためのフォーマット文字列  
	private readonly string FormatString = "地面を拡張するいかだ\nお得{0}枚セット";

	static private int m_price; // アイテムの価格
	static private int m_amount; // アイテムの数量

	public void SetInfomation(Sprite sprite, int amount, int price)
	{
		m_image.sprite = sprite; // アイテムの画像を設定
		m_amountText.text = "×" + amount.ToString(); // アイテムの数量を設定
		m_priceText.text = "$" + price.ToString(); // アイテムの価格を設定
		m_descriptionText.text = string.Format(FormatString, amount); // アイテムの説明を設定
		m_price = price; // アイテムの価格を保存
		m_amount = amount; // アイテムの数量を保存

		// 所持金によって表示メッセージを変更
		m_confirmationText.text = Money.GetMoney() >= price ?
			"本当に購入しますか？" :
			"所持金が不足しています。"; // 購入確認メッセージを設定
	}

	static public int GetPrice()
	{
		return m_price; // アイテムの価格を返す
	}

	static public int GetAmount()
	{
		return m_amount; // アイテムの数量を返す
	}
}
