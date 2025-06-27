using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopingButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private ConfirmationPanel m_confirmationPanel; // 確認パネルへの参照
	[SerializeField] private GameObject m_panel; // 確認パネルのGameObjectへの参照
	[SerializeField] private ButtonType m_buttonType; // ボタンの種類（Yes/No）
	private Image m_image; // アイコンのImageコンポーネントへの参照

	private bool m_isOnClick = false; // クリック状態を管理するフラグ

	public enum ButtonType
	{
		Yes,
		No,

		Length
	}

	private void Awake()
	{
		m_image = GetComponent<Image>(); // アイコンのImageコンポーネントを取得
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
		m_confirmationPanel.SetClickIcon(gameObject); // クリックされたアイコンをインベントリに設定する
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
			m_isOnClick = !m_isOnClick; // クリック状態をトグルする
			OnClick(); // クリック時の処理を実行
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

	public void OnClick()
	{
		// 購入
		if (m_buttonType == ButtonType.Yes)
		{
			if (Money.GetMoney() < m_confirmationPanel.GetPrice()) return; // 所持金が足りない場合は何もしない
			Money.UseMoney(m_confirmationPanel.GetPrice()); // 確認パネルから価格を取得してお金を使用
			RayGround.AddGround(m_confirmationPanel.GetAmount()); // 確認パネルから数量を取得して地面を追加
		}
		m_panel.SetActive(false); // 確認パネルを閉じる
	}
}
