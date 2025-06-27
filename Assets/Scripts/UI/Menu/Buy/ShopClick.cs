using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopClick : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private Confirmation m_confirmation; // 確認パネルのスクリプトへの参照
	[SerializeField] private Image m_itemImage; // アイテムの画像を表示するImage
	[SerializeField] private TextMeshProUGUI m_amountText; // アイテムの数量を表示するText
	[SerializeField] private TextMeshProUGUI m_priceText; // アイテムの価格を表示するText

	[SerializeField] private Sprite m_sprite; // アイテムの画像
	[SerializeField] private int m_amount; // アイテムの数量
	[SerializeField] private int m_price; // アイテムの価格

	private ShopIconManager m_iconManager;
	private Image m_image;

	private bool m_isOnClick; // アイコンがクリックされているかどうか

	void Start()
	{
		m_isOnClick = false;
		m_iconManager = GetComponentInParent<ShopIconManager>(); // 親のShopIconManagerを取得
		m_image = GetComponent<Image>(); // アイコンのImageコンポーネントを取得
		m_itemImage.sprite = m_sprite; // アイテムの画像を設定
		m_amountText.text = "×" + m_amount.ToString(); // アイテムの数量を設定
		m_priceText.text = "$" + m_price.ToString(); // アイテムの価格を設定
	}

	public void OnClickIcon()
	{
		m_confirmation.SetPanel(true); // 確認パネルを開く
		m_confirmation.SetConfirmationPanel(m_sprite, m_amount, m_price);
	}


	public void OnPointerEnter(PointerEventData eventData)
	{
		SetOnMouse(true);
	}

	// マウスが離れたとき
	public void OnPointerExit(PointerEventData eventData)
	{
		SetOnMouse(false);
	}

	// クリックされたときに呼ばれる
	public void OnPointerClick(PointerEventData eventData)
	{
		m_iconManager.SetClickIcon(gameObject); // クリックされたアイコンの画面にする
	}

	public void OnDisable()
	{
		m_isOnClick = false;
		m_image.color = Color.white;
	}

	// クリックされたアイコンだけをクリック状態にしてそれ以外を解除する
	public void SetClick(bool isClick)
	{
		// 自分がクリックされたとき
		if (isClick)
		{
			m_isOnClick = true; // クリック状態にする
			m_image.color = m_isOnClick ? Color.blue : Color.white; // クリック時の処理（例: 色を変える）
			OnClickIcon();
		}
		// 他のアイコンがクリックされたとき
		else
		{
			m_isOnClick = false; // クリック状態を解除
			m_image.color = Color.white; // 色を元に戻す
		}
	}


	public void SetOnMouse(bool onMouse)
	{
		if (m_isOnClick) return; // クリック状態のときは何もしない

		// マウスがアイコンの上にあるとき
		if (onMouse)
		{
			m_image.color = Color.yellow;
		}
		else
		{
			m_image.color = Color.white;
		}
	}
}
