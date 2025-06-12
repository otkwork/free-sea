using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class VisualDictionary : MonoBehaviour
{
	[SerializeField] ExcelData excelData; // Excel����̋��f�[�^���擾���邽�߂�ExcelData�X�N���v�g

	static bool[] m_isGetFish = new bool[MaxInventorySize]; // �����擾�������ǂ����̃t���O
	[SerializeField] GameObject[] fishDataObjects; // UI�ɕ\�����邽�߂̋��f�[�^�I�u�W�F�N�g  
	VisualDictionaryIcon[] clickIcons = new VisualDictionaryIcon[MaxInventorySize]; // �e���f�[�^�I�u�W�F�N�g�ɑΉ�����N���b�N�A�C�R��
	Image[] iconFishImage = new Image[MaxInventorySize]; // �e�A�C�R���ɕ\�����鋛�̉摜

	const int MaxInventorySize = 25; // �\���C���x���g���̍ő�T�C�Y  

	// Start is called before the first frame update  
	void Start()
	{
		for (int i = 0; i < MaxInventorySize; ++i)
		{
			clickIcons[i] = fishDataObjects[i].GetComponent<VisualDictionaryIcon>();
			m_isGetFish[i] = false; // ������Ԃł͋����擾���Ă��Ȃ�
			
			// �A�C�R���Ƀf�[�^��ݒ肷��
			clickIcons[i].SetFishData(excelData.fish[i]); // �N���b�N�A�C�R���ɋ��f�[�^��ݒ�
			iconFishImage[i] = clickIcons[i].transform.GetChild(0).GetComponent<Image>();
			iconFishImage[i].color = Color.black; // �A�C�R���̐F��������
		}
	}

	// Update is called once per frame  
	void Update()
	{
		// ���ɉ����ĕ\������摜��ύX
		for (int i = 0; i < MaxInventorySize; ++i)
		{
			iconFishImage[i].color = m_isGetFish[i] ? Color.white : Color.black; // �����擾���Ă���ꍇ�͔��A���Ă��Ȃ��ꍇ�͍��ɐݒ�
		}
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

	static public bool IsGetFish(int id)
	{
		if (id < 0 || id >= MaxInventorySize) return false; // �͈͊O��ID�͖���
		return m_isGetFish[id]; // �����擾�������ǂ�����Ԃ�
	}
}

