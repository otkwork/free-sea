using UnityEngine;
using System.Collections.Generic;
using System;

public class Inventory : MonoBehaviour
{
	static List<FishDataEntity> fishDataList = new List<FishDataEntity>();
	static FishDataEntity clickIconData;
	[SerializeField] GameObject[] fishDataObjects; // UIに表示するための魚データオブジェクト  
	ClickIcon[] clickIcons = new ClickIcon[MaxInventorySize]; // 各魚データオブジェクトに対応するクリックアイコン

	const int MaxInventorySize = 25; // 表示インベントリの最大サイズ  

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
