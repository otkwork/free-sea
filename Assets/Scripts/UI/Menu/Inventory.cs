using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
	static List<FishDataEntity> fishDataList = new List<FishDataEntity>();
	[SerializeField] GameObject[] fishDataObjects; // UI�ɕ\�����邽�߂̋��f�[�^�I�u�W�F�N�g  
	ClickIcon[] clickIcons = new ClickIcon[MaxInventorySize]; // �e���f�[�^�I�u�W�F�N�g�ɑΉ�����N���b�N�A�C�R��

	const int MaxInventorySize = 25; // �\���C���x���g���̍ő�T�C�Y  

	// Start is called before the first frame update  
	void Start()
	{
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
	static public void AddItem(FishDataEntity item)
	{
		// �C���x���g���̃T�C�Y���ő�ɒB���Ă���ꍇ�͒ǉ����Ȃ�
		if (fishDataList.Count >= MaxInventorySize) return;
		fishDataList.Add(item);
	}
	// �A�C�e�����폜���郁�\�b�h  
	static public void RemoveItem(FishDataEntity item)
	{
		if (fishDataList.Count <= 0) return; // �C���x���g������̏ꍇ�͉������Ȃ�
		fishDataList.Remove(item);
	}
	// �A�C�e�����擾���郁�\�b�h  
	static public FishDataEntity GetItems(int index)
	{
		return fishDataList[index];
	}
}
