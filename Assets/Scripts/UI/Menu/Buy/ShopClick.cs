using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopClick : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private Confirmation m_confirmation; // �m�F�p�l���̃X�N���v�g�ւ̎Q��
	[SerializeField] private Image m_itemImage; // �A�C�e���̉摜��\������Image
	[SerializeField] private TextMeshProUGUI m_amountText; // �A�C�e���̐��ʂ�\������Text
	[SerializeField] private TextMeshProUGUI m_priceText; // �A�C�e���̉��i��\������Text

	[SerializeField] private Sprite m_sprite; // �A�C�e���̉摜
	[SerializeField] private int m_amount; // �A�C�e���̐���
	[SerializeField] private int m_price; // �A�C�e���̉��i

	private ShopIconManager m_iconManager;
	private Image m_image;

	private bool m_isOnClick; // �A�C�R�����N���b�N����Ă��邩�ǂ���

	void Start()
	{
		m_isOnClick = false;
		m_iconManager = GetComponentInParent<ShopIconManager>(); // �e��ShopIconManager���擾
		m_image = GetComponent<Image>(); // �A�C�R����Image�R���|�[�l���g���擾
		m_itemImage.sprite = m_sprite; // �A�C�e���̉摜��ݒ�
		m_amountText.text = "�~" + m_amount.ToString(); // �A�C�e���̐��ʂ�ݒ�
		m_priceText.text = "$" + m_price.ToString(); // �A�C�e���̉��i��ݒ�
	}

	public void OnClickIcon()
	{
		m_confirmation.SetPanel(true); // �m�F�p�l�����J��
		m_confirmation.SetConfirmationPanel(m_sprite, m_amount, m_price);
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
		m_iconManager.SetClickIcon(gameObject); // �N���b�N���ꂽ�A�C�R���̉�ʂɂ���
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
			m_isOnClick = true; // �N���b�N��Ԃɂ���
			m_image.color = m_isOnClick ? Color.blue : Color.white; // �N���b�N���̏����i��: �F��ς���j
			OnClickIcon();
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
}
