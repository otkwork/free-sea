using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	[SerializeField] private DescriptionText m_descriptionText;
	[SerializeField] private GameObject[] m_fishDataObjects; // UIに表示するための魚データオブジェクト  
	private static List<FishDataEntity> m_fishDataList = new List<FishDataEntity>();
	private static FishDataEntity m_clickIconData;
	private ClickIcon[] m_clickIcons = new ClickIcon[MaxInventorySize]; // 各魚データオブジェクトに対応するクリックアイコン
	
	private const int MaxInventorySize = 25; // 表示インベントリの最大サイズ  

	private int m_padIconIndex = 0; // パッドの時選択されているアイコンのインデックス

	// Start is called before the first frame update  
	void Start()
	{
        m_clickIconData = null;
		for (int i = 0; i < MaxInventorySize; ++i)
		{
            m_clickIcons[i] = m_fishDataObjects[i].GetComponent<ClickIcon>();
		}
	}

	// Update is called once per frame  
	void Update()
	{
		// 情報に応じて表示する画像を変更
		for (int i = 0; i < MaxInventorySize; ++i)
		{
			// 魚のデータがある場合表示する  
			if (i < m_fishDataList.Count)
			{
                // クリックアイコンにデータを設定する
                m_clickIcons[i].SetFishData(m_fishDataList[i]); // クリックアイコンに魚データを設定
			}
			else
			{
                m_clickIcons[i].SetFishData(null);
			}
		}

		// 選択中のインデックスのアイコンの色を変える
		for (int i = 0; i < MaxInventorySize; ++i)
		{
            m_clickIcons[i].SetOnMouse(i == m_padIconIndex);
		}

		SelectIcon(); // パッドでアイコンを選択するメソッドを呼び出す
	}

	private void SelectIcon()
	{
		// パッドの入力によるアイコン選択
		if (InputSystem.GetInputMenuButtonDown("Up"))
		{
			m_padIconIndex = (m_padIconIndex - 5 + MaxInventorySize) % MaxInventorySize; // 上に移動
		}
		else if (InputSystem.GetInputMenuButtonDown("Down"))
		{
			m_padIconIndex = (m_padIconIndex + 5) % MaxInventorySize; // 下に移動
		}
		else if (InputSystem.GetInputMenuButtonDown("Left"))
		{
			m_padIconIndex = (m_padIconIndex - 1 + MaxInventorySize) % MaxInventorySize; // 左に移動
		}
		else if (InputSystem.GetInputMenuButtonDown("Right"))
		{
			m_padIconIndex = (m_padIconIndex + 1) % MaxInventorySize; // 右に移動
		}

		// 決定
		if (InputSystem.GetInputMenuButtonDown("Decide"))
		{
			SetClickIcon(m_fishDataObjects[m_padIconIndex]); // 選択されたアイコンをクリック状態にする
		}

		if (InputSystem.GetInputMenuButtonDown("Sell"))
		{
			SellItem(); // アイテムを売る
		}
	}

	public void SetClickIcon(GameObject icon)
	{
		for (int i = 0; i < MaxInventorySize; ++i)
		{
			// 押されたアイコンだけをクリック状態にする
			if (icon != null && m_fishDataObjects[i] == icon)
			{
				m_clickIcons[i].SetClick(true);
				if (icon.TryGetComponent(out ClickIcon clickIcon))
				{
                    m_clickIconData = clickIcon.GetClickData();
                }
            }
			else
			{
				m_clickIcons[i].SetClick(false);
			}
		}
	}

	// アイテムを追加するメソッド  
	static public void AddItem(FishDataEntity fish)
	{
		// インベントリのサイズが最大に達している場合は追加しない
		if (m_fishDataList.Count >= MaxInventorySize) return;
        m_fishDataList.Add(fish);
	}
	// アイテムを削除するメソッド  
	public void SellItem()
	{
		if (m_clickIconData == null) return; // インベントリが空の場合は何もしない
		Money.AddMoney(m_clickIconData.price);

        m_fishDataList.Remove(m_clickIconData);
        m_clickIconData = null;

		SetClickIcon(null); // 選択状態を全部消す
        m_descriptionText.ReSetDescription();	// 説明文消す
    }
}
