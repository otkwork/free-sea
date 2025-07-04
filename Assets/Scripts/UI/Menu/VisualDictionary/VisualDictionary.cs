using UnityEngine;
using UnityEngine.UI;

public class VisualDictionary : MonoBehaviour
{
	[SerializeField] private ExcelData m_excelData; // Excelからの魚データを取得するためのExcelDataスクリプト
	[SerializeField] private GameObject[] m_fishDataObjects; // UIに表示するための魚データオブジェクト  
	[SerializeField] private VisualDictionaryPage m_page; // ページ管理のためのVisualDictionaryPageスクリプト
	[SerializeField] private DescriptionText m_descriptionText; // 説明テキストを表示するためのDescriptionTextスクリプト
	[SerializeField] private bool m_isDebugFishData = false; // デバッグモードのフラグ
	[SerializeField] private AudioClip m_pageChange;
    [SerializeField] private AudioClip m_selectIconSe;
    [SerializeField] private AudioClip m_clickIconSe;
    private VisualDictionaryIcon[] m_clickIcons = new VisualDictionaryIcon[MaxInventorySize]; // 各魚データオブジェクトに対応するクリックアイコン
	private Image[] m_iconFishImage = new Image[MaxInventorySize]; // 各アイコンに表示する魚の画像

	public const int MaxInventorySize = 25; // 表示インベントリの最大サイズ

	private static bool[] m_isGetFish; // 魚を取得したかどうかのフラグ
	private int m_prevStartNum; // ページの開始番号
	private int m_padIconIndex = 0; // パッドの時選択されているアイコンのインデックス

	// Start is called before the first frame update  
	void Start()
	{
		m_isGetFish = new bool[m_excelData.fish.Count]; // 魚の数だけ取得フラグを初期化
		m_prevStartNum = m_page.PageIndex; // 一フレーム前のページ数

		for (int i = 0; i < m_isGetFish.Length; ++i)
		{
			m_isGetFish[i] = m_isDebugFishData; // 初期状態では魚を取得していない
		}

		for (int i = 0; i < MaxInventorySize; ++i)
		{
            m_clickIcons[i] = m_fishDataObjects[i].GetComponent<VisualDictionaryIcon>();

            // アイコンにデータを設定する
            m_clickIcons[i].SetFishData(m_excelData.fish[i]); // クリックアイコンに魚データを設定
            m_iconFishImage[i] = m_clickIcons[i].transform.GetChild(0).GetComponent<Image>();
            m_iconFishImage[i].color = Color.black; // アイコンの色を初期化
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
                m_clickIcons[i].SetFishData(m_excelData.fish[j]); // 新しい魚データを設定
                m_iconFishImage[i].sprite = ImageLoader.LoadSpriteAsync(m_excelData.fish[j].fishName).Result; // 魚の画像をロードして表示
				SetClickIcon(null); // クリック状態をリセット
			}
            m_iconFishImage[i].color = m_isGetFish[j] ? Color.white : Color.black; // 魚を取得している場合は白、していない場合は黒に設定
		}
		m_prevStartNum = m_page.PageIndex; // 一フレーム前のページ数を更新


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
			SoundEffect.Play2D(m_selectIconSe);
			m_padIconIndex = (m_padIconIndex - 5 + MaxInventorySize) % MaxInventorySize; // 上に移動
		}
		else if (InputSystem.GetInputMenuButtonDown("Down"))
        {
            SoundEffect.Play2D(m_selectIconSe);
            m_padIconIndex = (m_padIconIndex + 5) % MaxInventorySize; // 下に移動
		}
		else if (InputSystem.GetInputMenuButtonDown("Left"))
        {
            SoundEffect.Play2D(m_selectIconSe);
            m_padIconIndex = (m_padIconIndex - 1 + MaxInventorySize) % MaxInventorySize; // 左に移動
		}
		else if (InputSystem.GetInputMenuButtonDown("Right"))
        {
            SoundEffect.Play2D(m_selectIconSe);
            m_padIconIndex = (m_padIconIndex + 1) % MaxInventorySize; // 右に移動
		}

		// 決定
		if (InputSystem.GetInputMenuButtonDown("Decide"))
		{
			SetClickIcon(m_fishDataObjects[m_padIconIndex]); // 選択されたアイコンをクリック状態にする
		}

		// 次のページへ
		if (InputSystem.GetInputMenuButtonDown("Next"))
        {
            SoundEffect.Play2D(m_pageChange);
            m_page.NextPage(); // 次のページへ移動
			m_padIconIndex = 0; // アイコン選択をリセット
		}

		// 前のページへ
		if (InputSystem.GetInputMenuButtonDown("Prev"))
        {
            SoundEffect.Play2D(m_pageChange);
            m_page.PrevPage(); // 前のページへ移動
			m_padIconIndex = 0; // アイコン選択をリセット
		}
	}

	public void SetClickIcon(GameObject icon)
	{
		for (int i = 0; i < MaxInventorySize; ++i)
		{
            // 押されたアイコンだけをクリック状態にする
            m_clickIcons[i].SetClick(icon != null && m_fishDataObjects[i] == icon);
		}
		if (icon == null)
		{
			m_descriptionText.ReSetDescription();
			return;
		}
		SoundEffect.Play2D(m_clickIconSe);
    }


	// アイテムを追加するメソッド  
	static public void AddItem(FishDataEntity fish)
	{
		m_isGetFish[fish.id] = true; // 魚を取得したフラグを立てる
	}

	public bool IsGetFish(int id)
	{
		if (id < 0 || id >= m_excelData.fish.Count) return false; // 範囲外のIDは無効
		return m_isGetFish[id]; // 魚を取得したかどうかを返す
	}
}

