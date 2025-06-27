using UnityEngine;

public class Confirmation : MonoBehaviour
{
	[SerializeField] private GameObject m_confirmationPanel; // 確認パネルのGameObject

	private static bool m_isPanelOpen; // パネルが開いているかどうかのフラグ

	// Start is called before the first frame update
	void Start()
    {
        m_isPanelOpen = false; // 初期状態ではパネルは開いていない
		m_confirmationPanel.SetActive(false); // 確認パネルを非表示にする
	}

	public void SetPanel(bool setPanel)
	{
		m_isPanelOpen = setPanel; // パネルを表示/非表示
	}

	// クリックしたアイテムをパネルの情報を取得する
	public void SetConfirmationPanel(Sprite itemImage, int itemAmount, int itemPrice)
	{
		m_isPanelOpen = true; // パネルを開く
		m_confirmationPanel.SetActive(true);
		m_confirmationPanel.GetComponent<ConfirmationPanel>().SetInfomation(itemImage, itemAmount, itemPrice); // パネルにアイテム情報を設定する
	}
}
