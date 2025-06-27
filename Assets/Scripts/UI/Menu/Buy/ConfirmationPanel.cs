using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationPanel : MonoBehaviour
{
	[SerializeField] private Image m_image; // アイテムの画像  
	[SerializeField] private TextMeshProUGUI m_amountText; // アイテムの数量  
	[SerializeField] private TextMeshProUGUI m_priceText; // アイテムの価格  
	[SerializeField] private TextMeshProUGUI m_descriptionText;
	[SerializeField] private TextMeshProUGUI m_confirmationText; // 確認メッセージのテキスト
	[SerializeField] private ShopIconManager m_shopManager; // ショップアイコンマネージャーへの参照

	[SerializeField] private GameObject[] m_shopingButton; // ショッピングボタンの配列
	private ShopingButton[] m_button; // クリックアイコンの配列

	// 文字列の中に変数を埋め込むためのフォーマット文字列  
	private readonly string FormatString = "地面を拡張するいかだ\nお得{0}枚セット";
	private const int MaxButtonCount = 2; // ショッピングボタンの最大数

	private int m_price; // アイテムの価格
	private int m_amount; // アイテムの数量
	private int m_padIconIndex = 0; // パッドの時選択されているアイコンのインデックス

	private void Start()
	{
		m_button = new ShopingButton[MaxButtonCount]; // ショッピングボタンの配列を初期化
		for (int i = 0; i < MaxButtonCount; ++i)
		{
			m_button[i] = m_shopingButton[i].GetComponent<ShopingButton>(); // 各ボタンのコンポーネントを取得
		}
	}

	private void Update()
	{
		// 選択中のインデックスのアイコンの色を変える
		for (int i = 0; i < MaxButtonCount; ++i)
		{
			m_button[i].SetOnMouse(i == m_padIconIndex);
		}

		SelectIcon(); // パッドでアイコンを選択するメソッドを呼び出す
	}

	private void SelectIcon()
	{
		if (InputSystem.GetInputMenuButtonDown("Left"))
		{
			m_padIconIndex = (m_padIconIndex - 1 + MaxButtonCount) % MaxButtonCount; // 左に移動
		}
		else if (InputSystem.GetInputMenuButtonDown("Right"))
		{
			m_padIconIndex = (m_padIconIndex + 1) % MaxButtonCount; // 右に移動
		}

		// 決定
		if (InputSystem.GetInputMenuButtonDown("Decide"))
		{
			SetClickIcon(m_shopingButton[m_padIconIndex]); // 選択されたアイコンをクリック状態にする
		}
	}


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

	private void OnDisable()
	{
		m_shopManager.SetClickIcon(null); // 確認パネルが閉じられたときにクリック状態を解除
		m_padIconIndex = 0;
	}

	public void SetClickIcon(GameObject icon)
	{
		for (int i = 0; i < MaxButtonCount; ++i)
		{
			// 押されたアイコンだけをクリック状態にする
			m_button[i].SetClick((icon != null && m_shopingButton[i] == icon));
		}
	}

	public int GetPrice()
	{
		return m_price; // アイテムの価格を返す
	}

	public int GetAmount()
	{
		return m_amount; // アイテムの数量を返す
	}
}
