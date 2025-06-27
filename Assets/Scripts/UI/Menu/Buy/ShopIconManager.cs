using System.Collections.Generic;
using UnityEngine;

public class ShopIconManager : MonoBehaviour
{
	[SerializeField] private GameObject[] m_shopIcon;
	[SerializeField] private GameObject m_confirmation;
	private ShopClick[] m_clickIcons = new ShopClick[ShopIconAmount]; // �e���f�[�^�I�u�W�F�N�g�ɑΉ�����N���b�N�A�C�R��

	private const int ShopIconAmount = 3; // �V���b�v�A�C�R���̐�

	private int m_padIconIndex = 0; // �p�b�h�̎��I������Ă���A�C�R���̃C���f�b�N�X

	// Start is called before the first frame update  
	void Start()
	{
		for (int i = 0; i < ShopIconAmount; ++i)
		{
			m_clickIcons[i] = m_shopIcon[i].GetComponent<ShopClick>();
		}
	}

	// Update is called once per frame  
	void Update()
	{
		if (m_confirmation.activeSelf)
		{
			for (int i = 0; i < ShopIconAmount; ++i)
			{
				m_clickIcons[i].SetOnMouse(false); // �m�F�p�l�����J���Ă���ꍇ�̓J�[�\���̐F��ς��Ȃ�
			}
			return; // �m�F�p�l�����J���Ă���ꍇ�͉������Ȃ�
		}

		// �I�𒆂̃C���f�b�N�X�̃A�C�R���̐F��ς���
		for (int i = 0; i < ShopIconAmount; ++i)
		{
			m_clickIcons[i].SetOnMouse(i == m_padIconIndex);
		}

		SelectIcon(); // �p�b�h�ŃA�C�R����I�����郁�\�b�h���Ăяo��
	}

	private void SelectIcon()
	{
		// �p�b�h�̓��͂ɂ��A�C�R���I��
		if (InputSystem.GetInputMenuButtonDown("Left"))
		{
			m_padIconIndex = (m_padIconIndex - 1 + ShopIconAmount) % ShopIconAmount; // ���Ɉړ�
		}
		else if (InputSystem.GetInputMenuButtonDown("Right"))
		{
			m_padIconIndex = (m_padIconIndex + 1) % ShopIconAmount; // �E�Ɉړ�
		}

		// ����
		if (InputSystem.GetInputMenuButtonDown("Decide"))
		{
			SetClickIcon(m_shopIcon[m_padIconIndex]); // �I�����ꂽ�A�C�R�����N���b�N��Ԃɂ���
		}
	}

	public void SetClickIcon(GameObject icon)
	{
		for (int i = 0; i < ShopIconAmount; ++i)
		{
			// �����ꂽ�A�C�R���������N���b�N��Ԃɂ���
			m_clickIcons[i].SetClick(icon != null && m_shopIcon[i] == icon);
		}
	}
}
