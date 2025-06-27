using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopingButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private ConfirmationPanel m_confirmationPanel; // �m�F�p�l���ւ̎Q��
	[SerializeField] private GameObject m_panel; // �m�F�p�l����GameObject�ւ̎Q��
	[SerializeField] private ButtonType m_buttonType; // �{�^���̎�ށiYes/No�j
	private Image m_image; // �A�C�R����Image�R���|�[�l���g�ւ̎Q��

	private bool m_isOnClick = false; // �N���b�N��Ԃ��Ǘ�����t���O

	public enum ButtonType
	{
		Yes,
		No,

		Length
	}

	private void Awake()
	{
		m_image = GetComponent<Image>(); // �A�C�R����Image�R���|�[�l���g���擾
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		SetOnMouse(true);
	}

	// �}�E�X�����ꂽ�Ƃ�
	public void OnPointerExit(PointerEventData eventData)
	{
		SetOnMouse(false);
	}

	// �N���b�N���ꂽ�Ƃ��ɌĂ΂��
	public void OnPointerClick(PointerEventData eventData)
	{
		m_confirmationPanel.SetClickIcon(gameObject); // �N���b�N���ꂽ�A�C�R�����C���x���g���ɐݒ肷��
	}

	public void OnDisable()
	{
		m_isOnClick = false;
		m_image.color = Color.white;
	}

	// �N���b�N���ꂽ�A�C�R���������N���b�N��Ԃɂ��Ă���ȊO����������
	public void SetClick(bool isClick)
	{
		// �������N���b�N���ꂽ�Ƃ�
		if (isClick)
		{
			m_isOnClick = !m_isOnClick; // �N���b�N��Ԃ��g�O������
			OnClick(); // �N���b�N���̏��������s
		}
		// ���̃A�C�R�����N���b�N���ꂽ�Ƃ�
		else
		{
			m_isOnClick = false; // �N���b�N��Ԃ�����
			m_image.color = Color.white; // �F�����ɖ߂�
		}
	}

	public void SetOnMouse(bool onMouse)
	{
		if (m_isOnClick) return; // �N���b�N��Ԃ̂Ƃ��͉������Ȃ�

		// �}�E�X���A�C�R���̏�ɂ���Ƃ�
		if (onMouse)
		{
			m_image.color = Color.yellow;
		}
		else
		{
			m_image.color = Color.white;
		}
	}

	public void OnClick()
	{
		// �w��
		if (m_buttonType == ButtonType.Yes)
		{
			if (Money.GetMoney() < m_confirmationPanel.GetPrice()) return; // ������������Ȃ��ꍇ�͉������Ȃ�
			Money.UseMoney(m_confirmationPanel.GetPrice()); // �m�F�p�l�����牿�i���擾���Ă������g�p
			RayGround.AddGround(m_confirmationPanel.GetAmount()); // �m�F�p�l�����琔�ʂ��擾���Ēn�ʂ�ǉ�
		}
		m_panel.SetActive(false); // �m�F�p�l�������
	}
}
