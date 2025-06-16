using UnityEngine;
using UnityEngine.UI;

public class VisualDictionary : MonoBehaviour
{
	[SerializeField] ExcelData excelData; // Excelからの魚データを取得するためのExcelDataスクリプト

	static bool[] m_isGetFish; // 魚を取得したかどうかのフラグ
	[SerializeField] GameObject[] fishDataObjects; // UIに表示するための魚データオブジェクト  
	[SerializeField] VisualDictionaryPage m_page; // ページ管理のためのVisualDictionaryPageスクリプト
	VisualDictionaryIcon[] clickIcons = new VisualDictionaryIcon[MaxInventorySize]; // 各魚データオブジェクトに対応するクリックアイコン
	Image[] iconFishImage = new Image[MaxInventorySize]; // 各アイコンに表示する魚の画像

	public const int MaxInventorySize = 25; // 表示インベントリの最大サイズ
	private int m_prevStartNum; // 開始番号

	// Start is called before the first frame update  
	void Start()
	{
		m_isGetFish = new bool[excelData.fish.Count]; // 魚の数だけ取得フラグを初期化
		m_prevStartNum = m_page.PageIndex; // 一フレーム前のページ数

		for (int i = 0; i < m_isGetFish.Length; ++i)
		{
			m_isGetFish[i] = true; // 初期状態では魚を取得していない
		}

		for (int i = 0; i < MaxInventorySize; ++i)
		{
			clickIcons[i] = fishDataObjects[i].GetComponent<VisualDictionaryIcon>();
			
			// アイコンにデータを設定する
			clickIcons[i].SetFishData(excelData.fish[i]); // クリックアイコンに魚データを設定
			iconFishImage[i] = clickIcons[i].transform.GetChild(0).GetComponent<Image>();
			iconFishImage[i].color = Color.black; // アイコンの色を初期化
		}
	}

	// Update is called once per frame  
	void Update()
	{
		bool isPageChanged = m_prevStartNum != m_page.PageIndex; // ページが変わったかどうかのフラグ
		int pageLastNum = m_page.PageIndex * MaxInventorySize; // 現在のページの最後の魚データのインデックス

		// 情報に応じて表示する画像の変更
		for (int i = 0, j = pageLastNum - MaxInventorySize; i < MaxInventorySize; ++i, ++j)
		{
			// ページが変更されていたら
			if (isPageChanged)
			{
				clickIcons[i].SetFishData(excelData.fish[j]); // 新しい魚データを設定
				iconFishImage[i].sprite = ImageLoader.LoadSpriteAsync(excelData.fish[j].fishName).Result; // 魚の画像をロードして表示
			}
			iconFishImage[i].color = m_isGetFish[j] ? Color.white : Color.black; // 魚を取得している場合は白、していない場合は黒に設定
		}
		m_prevStartNum = m_page.PageIndex; // 一フレーム前のページ数を更新
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

	public bool IsGetFish(int id)
	{
		if (id < 0 || id >= excelData.fish.Count) return false; // 範囲外のIDは無効
		return m_isGetFish[id]; // 魚を取得したかどうかを返す
	}
}

