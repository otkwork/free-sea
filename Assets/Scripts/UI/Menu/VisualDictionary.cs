using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class VisualDictionary : MonoBehaviour
{
	[SerializeField] ExcelData excelData; // Excelからの魚データを取得するためのExcelDataスクリプト

	static bool[] m_isGetFish = new bool[MaxInventorySize]; // 魚を取得したかどうかのフラグ
	[SerializeField] GameObject[] fishDataObjects; // UIに表示するための魚データオブジェクト  
	VisualDictionaryIcon[] clickIcons = new VisualDictionaryIcon[MaxInventorySize]; // 各魚データオブジェクトに対応するクリックアイコン
	Image[] iconFishImage = new Image[MaxInventorySize]; // 各アイコンに表示する魚の画像

	const int MaxInventorySize = 25; // 表示インベントリの最大サイズ  

	// Start is called before the first frame update  
	void Start()
	{
		for (int i = 0; i < MaxInventorySize; ++i)
		{
			clickIcons[i] = fishDataObjects[i].GetComponent<VisualDictionaryIcon>();
			m_isGetFish[i] = false; // 初期状態では魚を取得していない
			
			// アイコンにデータを設定する
			clickIcons[i].SetFishData(excelData.fish[i]); // クリックアイコンに魚データを設定
			iconFishImage[i] = clickIcons[i].transform.GetChild(0).GetComponent<Image>();
			iconFishImage[i].color = Color.black; // アイコンの色を初期化
		}
	}

	// Update is called once per frame  
	void Update()
	{
		// 情報に応じて表示する画像を変更
		for (int i = 0; i < MaxInventorySize; ++i)
		{
			iconFishImage[i].color = m_isGetFish[i] ? Color.white : Color.black; // 魚を取得している場合は白、していない場合は黒に設定
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
	static public void AddItem(FishDataEntity fish)
	{
		m_isGetFish[fish.id] = true; // 魚を取得したフラグを立てる
	}

	static public bool IsGetFish(int id)
	{
		if (id < 0 || id >= MaxInventorySize) return false; // 範囲外のIDは無効
		return m_isGetFish[id]; // 魚を取得したかどうかを返す
	}
}

