using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	static List<FishDataEntity> fishDataList = new List<FishDataEntity>();
	static FishDataEntity clickIconData;
	[SerializeField] GameObject[] fishDataObjects; // UIに表示するための魚データオブジェクト  
	ClickIcon[] clickIcons = new ClickIcon[MaxInventorySize]; // 各魚データオブジェクトに対応するクリックアイコン

	const int MaxInventorySize = 25; // 表示インベントリの最大サイズ  

	private int m_padIconIndex = 0; // パッドの時選択されているアイコンのインデックス

	// Start is called before the first frame update  
	void Start()
	{
		clickIconData = null;
		for (int i = 0; i < MaxInventorySize; ++i)
		{
			clickIcons[i] = fishDataObjects[i].GetComponent<ClickIcon>();
		}
	}

	// Update is called once per frame  
	void Update()
	{
		// 情報に応じて表示する画像を変更
		for (int i = 0; i < MaxInventorySize; ++i)
		{
			// 魚のデータがある場合表示する  
			if (i < fishDataList.Count)
			{
				// クリックアイコンにデータを設定する
				clickIcons[i].SetFishData(fishDataList[i]); // クリックアイコンに魚データを設定
			}
			else
			{
                clickIcons[i].SetFishData(null);
			}
		}

		// 選択中のインデックスのアイコンの色を変える
		for (int i = 0; i < MaxInventorySize; ++i)
		{
			clickIcons[i].SetOnMouse(i == m_padIconIndex);
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
			SetClickIcon(fishDataObjects[m_padIconIndex]); // 選択されたアイコンをクリック状態にする
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
			if (icon != null && fishDataObjects[i] == icon)
			{
				clickIcons[i].SetClick(true);
				if (icon.TryGetComponent(out ClickIcon clickIcon))
				{
					clickIconData = clickIcon.GetClickData();
                }
            }
			else
			{
				clickIcons[i].SetClick(false);
			}
		}
	}


	// アイテムを追加するメソッド  
	static public void AddItem(FishDataEntity item)
	{
		// インベントリのサイズが最大に達している場合は追加しない
		if (fishDataList.Count >= MaxInventorySize) return;
		fishDataList.Add(item);
	}
	// アイテムを削除するメソッド  
	static public void SellItem()
	{
		if (clickIconData == null) return; // インベントリが空の場合は何もしない
		Money.AddMoney(clickIconData.price);

		fishDataList.Remove(clickIconData);
		clickIconData = null;
	}
}
