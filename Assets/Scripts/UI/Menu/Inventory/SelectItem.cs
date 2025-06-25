using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectItem : MonoBehaviour
{
	public enum ItemType
	{
		FishingRod, // �ނ��
		Hammer,     // �n���}�[

		Length, // �A�C�e���̐�
	}
	
	[SerializeField] private SelectItemIcon[] m_selectItem; // �A�C�e���̉摜���i�[����z��

	static private int m_selectItemIndex;
	static private bool m_isHaveHammer; // �n���}�[�������Ă��邩�ǂ���

	private void Start()
	{
		m_selectItemIndex = (int)ItemType.FishingRod; // �����I���A�C�e����ނ�Ƃɐݒ�
		m_isHaveHammer = false;

		for (int i = 0; i < (int)ItemType.Length; i++)
		{
			m_selectItem[i].SetClick(m_selectItem[i].GetItemType() == (ItemType)m_selectItemIndex); // ������ԂŒނ�Ƃ�I����Ԃɂ���
		}
	}

	public void Select(GameObject icon)
	{
		for (int i = 0; i < (int)ItemType.Length; i++)
		{
			// �I�������A�C�R����ݒ�
			if (icon == m_selectItem[i].gameObject)
			{
				if (m_selectItem[i].GetItemType() == ItemType.Hammer && !m_isHaveHammer) return; // �n���}�[�������Ă��Ȃ��ꍇ�͑I��s��
				m_selectItemIndex = (int)m_selectItem[i].GetItemType(); // �A�C�R�����N���b�N���ꂽ�炻�̃A�C�e����I��
				break;
			}
		}

		for (int i = 0; i < (int)ItemType.Length; i++)
		{
			m_selectItem[i].SetClick(m_selectItem[i].GetItemType() == (ItemType)m_selectItemIndex); // �I������Ă���A�C�R�����n�C���C�g
		}
	}

	static public void SetHammer()
	{
		m_isHaveHammer = true; // �n���}�[�������Ă����Ԃɂ���
	}

	static public bool GetIsHaveHammer()
	{
		return m_isHaveHammer; // �n���}�[�������Ă��邩�ǂ�����Ԃ�
	}

	static public ItemType GetItemType()
	{
		return (ItemType)m_selectItemIndex;
	}
}
