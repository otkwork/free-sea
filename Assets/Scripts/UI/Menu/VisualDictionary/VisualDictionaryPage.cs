using TMPro;
using UnityEngine;

public class VisualDictionaryPage : MonoBehaviour
{
	[SerializeField] ExcelData excelData; // Excel����̋��f�[�^���擾���邽�߂�ExcelData
	[SerializeField] TextMeshProUGUI pageText; // �y�[�W����\�����邽�߂̃e�L�X�g

	const int StartPage = 1; // �y�[�W�̊J�n�ԍ�
	private int m_pageIndex; // ���݂̃y�[�W��

	private void Awake()
	{
		m_pageIndex = StartPage; // �����y�[�W��1
	}

	private void Update()
	{
		// �y�[�W�����X�V����
		int maxPage = excelData.fish.Count % VisualDictionary.MaxInventorySize == 0 ?
			excelData.fish.Count / VisualDictionary.MaxInventorySize :
			excelData.fish.Count / VisualDictionary.MaxInventorySize + 1;

		pageText.text = $"{m_pageIndex} / {maxPage}";
	}

	public void NextPage()
	{
		// ���̃y�[�W�����ő�y�[�W��菬�����Ȃ�y�[�W��i�߂�
		if (excelData.fish.Count / VisualDictionary.MaxInventorySize > m_pageIndex)
		{
			m_pageIndex++;
		}
	}

	public void PrevPage()
	{
		// ���̃y�[�W����StartPage���傫���Ȃ�y�[�W��߂�
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
