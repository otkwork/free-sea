using TMPro;
using UnityEngine;

public class VisualDictionaryPage : MonoBehaviour
{
	[SerializeField] ExcelData excelData; // Excel����̋��f�[�^���擾���邽�߂�ExcelData
	[SerializeField] TextMeshProUGUI pageText; // �y�[�W����\�����邽�߂̃e�L�X�g

	const int StartPage = 1; // �y�[�W�̊J�n�ԍ�
	int m_fishAmount; // ���̑���
	private int m_pageIndex; // ���݂̃y�[�W��

	private void Awake()
	{
		m_pageIndex = StartPage; // �����y�[�W��1
		m_fishAmount = excelData.fish.Count - 1; // �n���}�[�̕������������̑���
	}

	private void Update()
	{
		// �y�[�W�����X�V����
		// �n���}�[�̕����l������-1����
		int maxPage = m_fishAmount % VisualDictionary.MaxInventorySize == 0 ?
			m_fishAmount / VisualDictionary.MaxInventorySize :
			m_fishAmount / VisualDictionary.MaxInventorySize + 1;

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
