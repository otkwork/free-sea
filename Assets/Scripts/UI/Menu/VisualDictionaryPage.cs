using TMPro;
using UnityEngine;

public class VisualDictionaryPage : MonoBehaviour
{
	[SerializeField] ExcelData excelData; // Excelからの魚データを取得するためのExcelData
	[SerializeField] TextMeshProUGUI pageText; // ページ数を表示するためのテキスト

	const int StartPage = 1; // ページの開始番号
	private int m_pageIndex; // 現在のページ数

	private void Awake()
	{
		m_pageIndex = StartPage; // 初期ページは1
	}

	private void Update()
	{
		// ページ数を更新する
		int maxPage = excelData.fish.Count % VisualDictionary.MaxInventorySize == 0 ?
			excelData.fish.Count / VisualDictionary.MaxInventorySize :
			excelData.fish.Count / VisualDictionary.MaxInventorySize + 1;

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
