using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
	static List<FishDataEntity> fishDataList = new List<FishDataEntity>();
	[SerializeField] GameObject[] fishDataObjects; // UIに表示するための魚データオブジェクト  
	ClickIcon[] clickIcons = new ClickIcon[MaxInventorySize]; // 各魚データオブジェクトに対応するクリックアイコン

	const int MaxInventorySize = 25; // 表示インベントリの最大サイズ  

	// Start is called before the first frame update  
	void Start()
	{
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
		}
	}

	public void SetClickIcon(GameObject icon)
	{
		for (int i = 0; i < MaxInventorySize; ++i)
		{
			// 押されたアイコンだけをクリック状態にする
			clickIcons[i].SetClick(fishDataObjects[i] == icon);
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
	static public void RemoveItem(FishDataEntity item)
	{
		if (fishDataList.Count <= 0) return; // インベントリが空の場合は何もしない
		fishDataList.Remove(item);
	}
	// アイテムを取得するメソッド  
	static public FishDataEntity GetItems(int index)
	{
		return fishDataList[index];
	}
}
