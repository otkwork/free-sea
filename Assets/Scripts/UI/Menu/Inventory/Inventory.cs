using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	static List<FishDataEntity> fishDataList = new List<FishDataEntity>();
	static FishDataEntity clickIconData;
	[SerializeField] GameObject[] fishDataObjects; // UI�ɕ\�����邽�߂̋��f�[�^�I�u�W�F�N�g  
	ClickIcon[] clickIcons = new ClickIcon[MaxInventorySize]; // �e���f�[�^�I�u�W�F�N�g�ɑΉ�����N���b�N�A�C�R��

	const int MaxInventorySize = 25; // �\���C���x���g���̍ő�T�C�Y  

	private int m_padIconIndex = 0; // �p�b�h�̎��I������Ă���A�C�R���̃C���f�b�N�X

	// Start is called before the first frame update  
	void Start()
	{
		clickIconData = null;
		for (int i = 0; i < MaxInventorySize; ++i)
		{
			clickIcons[i] = fishDataObjects[i].GetComponent<ClickIcon>();
		}
	}

	// Update is called once per frame  
	void Update()
	{
		// ���ɉ����ĕ\������摜��ύX
		for (int i = 0; i < MaxInventorySize; ++i)
		{
			// ���̃f�[�^������ꍇ�\������  
			if (i < fishDataList.Count)
			{
				// �N���b�N�A�C�R���Ƀf�[�^��ݒ肷��
				clickIcons[i].SetFishData(fishDataList[i]); // �N���b�N�A�C�R���ɋ��f�[�^��ݒ�
			}
			else
			{
                clickIcons[i].SetFishData(null);
			}
		}

		// �I�𒆂̃C���f�b�N�X�̃A�C�R���̐F��ς���
		for (int i = 0; i < MaxInventorySize; ++i)
		{
			clickIcons[i].SetOnMouse(i == m_padIconIndex);
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
			SetClickIcon(fishDataObjects[m_padIconIndex]); // �I�����ꂽ�A�C�R�����N���b�N��Ԃɂ���
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
			if (icon != null && fishDataObjects[i] == icon)
			{
				clickIcons[i].SetClick(true);
				if (icon.TryGetComponent(out ClickIcon clickIcon))
				{
					clickIconData = clickIcon.GetClickData();
                }
            }
			else
			{
				clickIcons[i].SetClick(false);
			}
		}
	}


	// �A�C�e����ǉ����郁�\�b�h  
	static public void AddItem(FishDataEntity item)
	{
		// �C���x���g���̃T�C�Y���ő�ɒB���Ă���ꍇ�͒ǉ����Ȃ�
		if (fishDataList.Count >= MaxInventorySize) return;
		fishDataList.Add(item);
	}
	// �A�C�e�����폜���郁�\�b�h  
	static public void SellItem()
	{
		if (clickIconData == null) return; // �C���x���g������̏ꍇ�͉������Ȃ�
		Money.AddMoney(clickIconData.price);

		fishDataList.Remove(clickIconData);
		clickIconData = null;
	}
}
