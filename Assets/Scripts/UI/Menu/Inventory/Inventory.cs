using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	[SerializeField] private DescriptionText m_descriptionText;
	[SerializeField] private GameObject[] m_fishDataObjects; // UI�ɕ\�����邽�߂̋��f�[�^�I�u�W�F�N�g  
	private static List<FishDataEntity> m_fishDataList = new List<FishDataEntity>();
	private static FishDataEntity m_clickIconData;
	private ClickIcon[] m_clickIcons = new ClickIcon[MaxInventorySize]; // �e���f�[�^�I�u�W�F�N�g�ɑΉ�����N���b�N�A�C�R��
	
	private const int MaxInventorySize = 25; // �\���C���x���g���̍ő�T�C�Y  

	private int m_padIconIndex = 0; // �p�b�h�̎��I������Ă���A�C�R���̃C���f�b�N�X

	// Start is called before the first frame update  
	void Start()
	{
        m_clickIconData = null;
		for (int i = 0; i < MaxInventorySize; ++i)
		{
            m_clickIcons[i] = m_fishDataObjects[i].GetComponent<ClickIcon>();
		}
	}

	// Update is called once per frame  
	void Update()
	{
		// ���ɉ����ĕ\������摜��ύX
		for (int i = 0; i < MaxInventorySize; ++i)
		{
			// ���̃f�[�^������ꍇ�\������  
			if (i < m_fishDataList.Count)
			{
                // �N���b�N�A�C�R���Ƀf�[�^��ݒ肷��
                m_clickIcons[i].SetFishData(m_fishDataList[i]); // �N���b�N�A�C�R���ɋ��f�[�^��ݒ�
			}
			else
			{
                m_clickIcons[i].SetFishData(null);
			}
		}

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

		if (InputSystem.GetInputMenuButtonDown("Sell"))
		{
			SellItem(); // �A�C�e���𔄂�
		}
	}

	public void SetClickIcon(GameObject icon)
	{
		for (int i = 0; i < MaxInventorySize; ++i)
		{
			// �����ꂽ�A�C�R���������N���b�N��Ԃɂ���
			if (icon != null && m_fishDataObjects[i] == icon)
			{
				m_clickIcons[i].SetClick(true);
				if (icon.TryGetComponent(out ClickIcon clickIcon))
				{
                    m_clickIconData = clickIcon.GetClickData();
                }
            }
			else
			{
				m_clickIcons[i].SetClick(false);
			}
		}
	}

	// �A�C�e����ǉ����郁�\�b�h  
	static public void AddItem(FishDataEntity fish)
	{
		// �C���x���g���̃T�C�Y���ő�ɒB���Ă���ꍇ�͒ǉ����Ȃ�
		if (m_fishDataList.Count >= MaxInventorySize) return;
        m_fishDataList.Add(fish);
	}
	// �A�C�e�����폜���郁�\�b�h  
	public void SellItem()
	{
		if (m_clickIconData == null) return; // �C���x���g������̏ꍇ�͉������Ȃ�
		Money.AddMoney(m_clickIconData.price);

        m_fishDataList.Remove(m_clickIconData);
        m_clickIconData = null;

		SetClickIcon(null); // �I����Ԃ�S������
        m_descriptionText.ReSetDescription();	// ����������
    }
}
