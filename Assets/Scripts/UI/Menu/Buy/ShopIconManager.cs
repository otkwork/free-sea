using System.Collections.Generic;
using UnityEngine;

public class ShopIconManager : MonoBehaviour
{
	[SerializeField] private GameObject[] m_shopIcon;
	[SerializeField] private GameObject m_confirmation;
	private ShopClick[] m_clickIcons = new ShopClick[ShopIconAmount]; // 各魚データオブジェクトに対応するクリックアイコン

	private const int ShopIconAmount = 3; // ショップアイコンの数

	private int m_padIconIndex = 0; // パッドの時選択されているアイコンのインデックス

	// Start is called before the first frame update  
	void Start()
	{
		for (int i = 0; i < ShopIconAmount; ++i)
		{
			m_clickIcons[i] = m_shopIcon[i].GetComponent<ShopClick>();
		}
	}

	// Update is called once per frame  
	void Update()
	{
		if (m_confirmation.activeSelf)
		{
			for (int i = 0; i < ShopIconAmount; ++i)
			{
				m_clickIcons[i].SetOnMouse(false); // 確認パネルが開いている場合はカーソルの色を変えない
			}
			return; // 確認パネルが開いている場合は何もしない
		}

		// 選択中のインデックスのアイコンの色を変える
		for (int i = 0; i < ShopIconAmount; ++i)
		{
			m_clickIcons[i].SetOnMouse(i == m_padIconIndex);
		}

		SelectIcon(); // パッドでアイコンを選択するメソッドを呼び出す
	}

	private void SelectIcon()
	{
		// パッドの入力によるアイコン選択
		if (InputSystem.GetInputMenuButtonDown("Left"))
		{
			m_padIconIndex = (m_padIconIndex - 1 + ShopIconAmount) % ShopIconAmount; // 左に移動
		}
		else if (InputSystem.GetInputMenuButtonDown("Right"))
		{
			m_padIconIndex = (m_padIconIndex + 1) % ShopIconAmount; // 右に移動
		}

		// 決定
		if (InputSystem.GetInputMenuButtonDown("Decide"))
		{
			SetClickIcon(m_shopIcon[m_padIconIndex]); // 選択されたアイコンをクリック状態にする
		}
	}

	public void SetClickIcon(GameObject icon)
	{
		for (int i = 0; i < ShopIconAmount; ++i)
		{
			// 押されたアイコンだけをクリック状態にする
			m_clickIcons[i].SetClick(icon != null && m_shopIcon[i] == icon);
		}
	}
}
