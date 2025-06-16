using UnityEngine;
using UnityEngine.UI;

public class VisualDictionary : MonoBehaviour
{
	[SerializeField] ExcelData excelData; // Excel����̋��f�[�^���擾���邽�߂�ExcelData�X�N���v�g

	static bool[] m_isGetFish; // �����擾�������ǂ����̃t���O
	[SerializeField] GameObject[] fishDataObjects; // UI�ɕ\�����邽�߂̋��f�[�^�I�u�W�F�N�g  
	[SerializeField] VisualDictionaryPage m_page; // �y�[�W�Ǘ��̂��߂�VisualDictionaryPage�X�N���v�g
	VisualDictionaryIcon[] clickIcons = new VisualDictionaryIcon[MaxInventorySize]; // �e���f�[�^�I�u�W�F�N�g�ɑΉ�����N���b�N�A�C�R��
	Image[] iconFishImage = new Image[MaxInventorySize]; // �e�A�C�R���ɕ\�����鋛�̉摜

	public const int MaxInventorySize = 25; // �\���C���x���g���̍ő�T�C�Y
	private int m_prevStartNum; // �J�n�ԍ�

	// Start is called before the first frame update  
	void Start()
	{
		m_isGetFish = new bool[excelData.fish.Count]; // ���̐������擾�t���O��������
		m_prevStartNum = m_page.PageIndex; // ��t���[���O�̃y�[�W��

		for (int i = 0; i < m_isGetFish.Length; ++i)
		{
			m_isGetFish[i] = true; // ������Ԃł͋����擾���Ă��Ȃ�
		}

		for (int i = 0; i < MaxInventorySize; ++i)
		{
			clickIcons[i] = fishDataObjects[i].GetComponent<VisualDictionaryIcon>();
			
			// �A�C�R���Ƀf�[�^��ݒ肷��
			clickIcons[i].SetFishData(excelData.fish[i]); // �N���b�N�A�C�R���ɋ��f�[�^��ݒ�
			iconFishImage[i] = clickIcons[i].transform.GetChild(0).GetComponent<Image>();
			iconFishImage[i].color = Color.black; // �A�C�R���̐F��������
		}
	}

	// Update is called once per frame  
	void Update()
	{
		bool isPageChanged = m_prevStartNum != m_page.PageIndex; // �y�[�W���ς�������ǂ����̃t���O
		int pageLastNum = m_page.PageIndex * MaxInventorySize; // ���݂̃y�[�W�̍Ō�̋��f�[�^�̃C���f�b�N�X

		// ���ɉ����ĕ\������摜�̕ύX
		for (int i = 0, j = pageLastNum - MaxInventorySize; i < MaxInventorySize; ++i, ++j)
		{
			// �y�[�W���ύX����Ă�����
			if (isPageChanged)
			{
				clickIcons[i].SetFishData(excelData.fish[j]); // �V�������f�[�^��ݒ�
				iconFishImage[i].sprite = ImageLoader.LoadSpriteAsync(excelData.fish[j].fishName).Result; // ���̉摜�����[�h���ĕ\��
			}
			iconFishImage[i].color = m_isGetFish[j] ? Color.white : Color.black; // �����擾���Ă���ꍇ�͔��A���Ă��Ȃ��ꍇ�͍��ɐݒ�
		}
		m_prevStartNum = m_page.PageIndex; // ��t���[���O�̃y�[�W�����X�V
	}

	public void SetClickIcon(GameObject icon)
	{
		for (int i = 0; i < MaxInventorySize; ++i)
		{
			// �����ꂽ�A�C�R���������N���b�N��Ԃɂ���
			clickIcons[i].SetClick(fishDataObjects[i] == icon);
		}
	}


	// �A�C�e����ǉ����郁�\�b�h  
	static public void AddItem(FishDataEntity fish)
	{
		m_isGetFish[fish.id] = true; // �����擾�����t���O�𗧂Ă�
	}

	public bool IsGetFish(int id)
	{
		if (id < 0 || id >= excelData.fish.Count) return false; // �͈͊O��ID�͖���
		return m_isGetFish[id]; // �����擾�������ǂ�����Ԃ�
	}
}

