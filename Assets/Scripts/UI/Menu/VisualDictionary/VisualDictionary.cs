using UnityEngine;
using UnityEngine.UI;

public class VisualDictionary : MonoBehaviour
{
	[SerializeField] private ExcelData m_excelData; // Excel����̋��f�[�^���擾���邽�߂�ExcelData�X�N���v�g
	[SerializeField] private GameObject[] m_fishDataObjects; // UI�ɕ\�����邽�߂̋��f�[�^�I�u�W�F�N�g  
	[SerializeField] private VisualDictionaryPage m_page; // �y�[�W�Ǘ��̂��߂�VisualDictionaryPage�X�N���v�g
	[SerializeField] private DescriptionText m_descriptionText; // �����e�L�X�g��\�����邽�߂�DescriptionText�X�N���v�g
	[SerializeField] private bool m_isDebugFishData = false; // �f�o�b�O���[�h�̃t���O
	private VisualDictionaryIcon[] m_clickIcons = new VisualDictionaryIcon[MaxInventorySize]; // �e���f�[�^�I�u�W�F�N�g�ɑΉ�����N���b�N�A�C�R��
	private Image[] m_iconFishImage = new Image[MaxInventorySize]; // �e�A�C�R���ɕ\�����鋛�̉摜

	public const int MaxInventorySize = 25; // �\���C���x���g���̍ő�T�C�Y

	private static bool[] m_isGetFish; // �����擾�������ǂ����̃t���O
	private int m_prevStartNum; // �y�[�W�̊J�n�ԍ�
	private int m_padIconIndex = 0; // �p�b�h�̎��I������Ă���A�C�R���̃C���f�b�N�X

	// Start is called before the first frame update  
	void Start()
	{
		m_isGetFish = new bool[m_excelData.fish.Count]; // ���̐������擾�t���O��������
		m_prevStartNum = m_page.PageIndex; // ��t���[���O�̃y�[�W��

		for (int i = 0; i < m_isGetFish.Length; ++i)
		{
			m_isGetFish[i] = m_isDebugFishData; // ������Ԃł͋����擾���Ă��Ȃ�
		}

		for (int i = 0; i < MaxInventorySize; ++i)
		{
            m_clickIcons[i] = m_fishDataObjects[i].GetComponent<VisualDictionaryIcon>();

            // �A�C�R���Ƀf�[�^��ݒ肷��
            m_clickIcons[i].SetFishData(m_excelData.fish[i]); // �N���b�N�A�C�R���ɋ��f�[�^��ݒ�
            m_iconFishImage[i] = m_clickIcons[i].transform.GetChild(0).GetComponent<Image>();
            m_iconFishImage[i].color = Color.black; // �A�C�R���̐F��������
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
                m_clickIcons[i].SetFishData(m_excelData.fish[j]); // �V�������f�[�^��ݒ�
                m_iconFishImage[i].sprite = ImageLoader.LoadSpriteAsync(m_excelData.fish[j].fishName).Result; // ���̉摜�����[�h���ĕ\��
				SetClickIcon(null); // �N���b�N��Ԃ����Z�b�g
			}
            m_iconFishImage[i].color = m_isGetFish[j] ? Color.white : Color.black; // �����擾���Ă���ꍇ�͔��A���Ă��Ȃ��ꍇ�͍��ɐݒ�
		}
		m_prevStartNum = m_page.PageIndex; // ��t���[���O�̃y�[�W�����X�V


		// �I�𒆂̃C���f�b�N�X�̃A�C�R���̐F��ς���
		for (int i = 0; i < MaxInventorySize; ++i)
		{
            m_clickIcons[i].SetOnMouse(i == m_padIconIndex);
		}

		SelectIcon(); // �p�b�h�ŃA�C�R����I�����郁�\�b�h���Ăяo��
	}

	private void SelectIcon()
	{
		// �p�b�h�̓��͂ɂ��A�C�R���I��
		if (InputSystem.GetInputMenuButtonDown("Up"))
		{
			m_padIconIndex = (m_padIconIndex - 5 + MaxInventorySize) % MaxInventorySize; // ��Ɉړ�
		}
		else if (InputSystem.GetInputMenuButtonDown("Down"))
		{
			m_padIconIndex = (m_padIconIndex + 5) % MaxInventorySize; // ���Ɉړ�
		}
		else if (InputSystem.GetInputMenuButtonDown("Left"))
		{
			m_padIconIndex = (m_padIconIndex - 1 + MaxInventorySize) % MaxInventorySize; // ���Ɉړ�
		}
		else if (InputSystem.GetInputMenuButtonDown("Right"))
		{
			m_padIconIndex = (m_padIconIndex + 1) % MaxInventorySize; // �E�Ɉړ�
		}

		// ����
		if (InputSystem.GetInputMenuButtonDown("Decide"))
		{
			SetClickIcon(m_fishDataObjects[m_padIconIndex]); // �I�����ꂽ�A�C�R�����N���b�N��Ԃɂ���
		}

		// ���̃y�[�W��
		if (InputSystem.GetInputMenuButtonDown("Next"))
		{
			m_page.NextPage(); // ���̃y�[�W�ֈړ�
			m_padIconIndex = 0; // �A�C�R���I�������Z�b�g
		}

		// �O�̃y�[�W��
		if (InputSystem.GetInputMenuButtonDown("Prev"))
		{
			m_page.PrevPage(); // �O�̃y�[�W�ֈړ�
			m_padIconIndex = 0; // �A�C�R���I�������Z�b�g
		}
	}

	public void SetClickIcon(GameObject icon)
	{
		for (int i = 0; i < MaxInventorySize; ++i)
		{
            // �����ꂽ�A�C�R���������N���b�N��Ԃɂ���
            m_clickIcons[i].SetClick(icon != null && m_fishDataObjects[i] == icon);
		}
		if (icon == null) m_descriptionText.ReSetDescription();
	}


	// �A�C�e����ǉ����郁�\�b�h  
	static public void AddItem(FishDataEntity fish)
	{
		m_isGetFish[fish.id] = true; // �����擾�����t���O�𗧂Ă�
	}

	public bool IsGetFish(int id)
	{
		if (id < 0 || id >= m_excelData.fish.Count) return false; // �͈͊O��ID�͖���
		return m_isGetFish[id]; // �����擾�������ǂ�����Ԃ�
	}
}

