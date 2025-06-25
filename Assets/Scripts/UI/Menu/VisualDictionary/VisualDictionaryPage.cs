using TMPro;
using UnityEngine;

public class VisualDictionaryPage : MonoBehaviour
{
	[SerializeField] ExcelData excelData; // Excelからの魚データを取得するためのExcelData
	[SerializeField] TextMeshProUGUI pageText; // ページ数を表示するためのテキスト

	const int StartPage = 1; // ページの開始番号
	int m_fishAmount; // 魚の総数
	private int m_pageIndex; // 現在のページ数

	private void Awake()
	{
		m_pageIndex = StartPage; // 初期ページは1
		m_fishAmount = excelData.fish.Count - 1; // ハンマーの分を除いた魚の総数
	}

	private void Update()
	{
		// ページ数を更新する
		// ハンマーの分を考慮して-1する
		int maxPage = m_fishAmount % VisualDictionary.MaxInventorySize == 0 ?
			m_fishAmount / VisualDictionary.MaxInventorySize :
			m_fishAmount / VisualDictionary.MaxInventorySize + 1;

		pageText.text = $"{m_pageIndex} / {maxPage}";
	}

	public void NextPage()
	{
		// 今のページ数が最大ページより小さいならページを進める
		if (excelData.fish.Count / VisualDictionary.MaxInventorySize > m_pageIndex)
		{
			m_pageIndex++;
		}
	}

	public void PrevPage()
	{
		// 今のページ数がStartPageより大きいならページを戻す
		if (m_pageIndex > StartPage)
		{
			m_pageIndex--;
		}
	}

	public int PageIndex
	{
		get { return m_pageIndex; }
	}
}
