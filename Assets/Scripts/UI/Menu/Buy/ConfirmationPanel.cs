using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationPanel : MonoBehaviour
{
	[SerializeField] private Image m_image; // �A�C�e���̉摜  
	[SerializeField] private TextMeshProUGUI m_amountText; // �A�C�e���̐���  
	[SerializeField] private TextMeshProUGUI m_priceText; // �A�C�e���̉��i  
	[SerializeField] private TextMeshProUGUI m_descriptionText;
	[SerializeField] private TextMeshProUGUI m_confirmationText; // �m�F���b�Z�[�W�̃e�L�X�g
	[SerializeField] private ShopIconManager m_shopManager; // �V���b�v�A�C�R���}�l�[�W���[�ւ̎Q��

	[SerializeField] private GameObject[] m_shopingButton; // �V���b�s���O�{�^���̔z��
	private ShopingButton[] m_button; // �N���b�N�A�C�R���̔z��

	// ������̒��ɕϐ��𖄂ߍ��ނ��߂̃t�H�[�}�b�g������  
	private readonly string FormatString = "�n�ʂ��g�����邢����\n����{0}���Z�b�g";
	private const int MaxButtonCount = 2; // �V���b�s���O�{�^���̍ő吔

	private int m_price; // �A�C�e���̉��i
	private int m_amount; // �A�C�e���̐���
	private int m_padIconIndex = 0; // �p�b�h�̎��I������Ă���A�C�R���̃C���f�b�N�X

	private void Start()
	{
		m_button = new ShopingButton[MaxButtonCount]; // �V���b�s���O�{�^���̔z���������
		for (int i = 0; i < MaxButtonCount; ++i)
		{
			m_button[i] = m_shopingButton[i].GetComponent<ShopingButton>(); // �e�{�^���̃R���|�[�l���g���擾
		}
	}

	private void Update()
	{
		// �I�𒆂̃C���f�b�N�X�̃A�C�R���̐F��ς���
		for (int i = 0; i < MaxButtonCount; ++i)
		{
			m_button[i].SetOnMouse(i == m_padIconIndex);
		}

		SelectIcon(); // �p�b�h�ŃA�C�R����I�����郁�\�b�h���Ăяo��
	}

	private void SelectIcon()
	{
		if (InputSystem.GetInputMenuButtonDown("Left"))
		{
			m_padIconIndex = (m_padIconIndex - 1 + MaxButtonCount) % MaxButtonCount; // ���Ɉړ�
		}
		else if (InputSystem.GetInputMenuButtonDown("Right"))
		{
			m_padIconIndex = (m_padIconIndex + 1) % MaxButtonCount; // �E�Ɉړ�
		}

		// ����
		if (InputSystem.GetInputMenuButtonDown("Decide"))
		{
			SetClickIcon(m_shopingButton[m_padIconIndex]); // �I�����ꂽ�A�C�R�����N���b�N��Ԃɂ���
		}
	}


	public void SetInfomation(Sprite sprite, int amount, int price)
	{
		m_image.sprite = sprite; // �A�C�e���̉摜��ݒ�
		m_amountText.text = "�~" + amount.ToString(); // �A�C�e���̐��ʂ�ݒ�
		m_priceText.text = "$" + price.ToString(); // �A�C�e���̉��i��ݒ�
		m_descriptionText.text = string.Format(FormatString, amount); // �A�C�e���̐�����ݒ�
		m_price = price; // �A�C�e���̉��i��ۑ�
		m_amount = amount; // �A�C�e���̐��ʂ�ۑ�

		// �������ɂ���ĕ\�����b�Z�[�W��ύX
		m_confirmationText.text = Money.GetMoney() >= price ?
			"�{���ɍw�����܂����H" :
			"���������s�����Ă��܂��B"; // �w���m�F���b�Z�[�W��ݒ�
	}

	private void OnDisable()
	{
		m_shopManager.SetClickIcon(null); // �m�F�p�l��������ꂽ�Ƃ��ɃN���b�N��Ԃ�����
		m_padIconIndex = 0;
	}

	public void SetClickIcon(GameObject icon)
	{
		for (int i = 0; i < MaxButtonCount; ++i)
		{
			// �����ꂽ�A�C�R���������N���b�N��Ԃɂ���
			m_button[i].SetClick((icon != null && m_shopingButton[i] == icon));
		}
	}

	public int GetPrice()
	{
		return m_price; // �A�C�e���̉��i��Ԃ�
	}

	public int GetAmount()
	{
		return m_amount; // �A�C�e���̐��ʂ�Ԃ�
	}
}
