using UnityEngine;

public class ShopingButton : MonoBehaviour
{
	[SerializeField] private GameObject m_confirmationPanel; // 確認パネルのGameObjectへの参照
	[SerializeField] private ButtonType m_buttonType; // ボタンの種類（Yes/No）

	private enum ButtonType
	{
		Yes,
		No
	}

	public void OnClick()
	{
		// 購入
		if (m_buttonType == ButtonType.Yes)
		{
			Money.UseMoney(ConfirmationPanel.GetPrice()); // 確認パネルから価格を取得してお金を使用
			RayGround.AddGround(ConfirmationPanel.GetAmount()); // 確認パネルから数量を取得して地面を追加
		}
		m_confirmationPanel.SetActive(false); // 確認パネルを閉じる
	}
}
