using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationPanel : MonoBehaviour
{
	[SerializeField] private Image m_image; // �A�C�e���̉摜  
	[SerializeField] private TextMeshProUGUI m_amountText; // �A�C�e���̐���  
	[SerializeField] private TextMeshProUGUI m_priceText; // �A�C�e���̉��i  
	[SerializeField] private TextMeshProUGUI m_descriptionText;
	[SerializeField] private TextMeshProUGUI m_confirmationText; // �m�F���b�Z�[�W�̃e�L�X�g

	// ������̒��ɕϐ��𖄂ߍ��ނ��߂̃t�H�[�}�b�g������  
	private readonly string FormatString = "�n�ʂ��g�����邢����\n����{0}���Z�b�g";

	static private int m_price; // �A�C�e���̉��i
	static private int m_amount; // �A�C�e���̐���

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

	static public int GetPrice()
	{
		return m_price; // �A�C�e���̉��i��Ԃ�
	}

	static public int GetAmount()
	{
		return m_amount; // �A�C�e���̐��ʂ�Ԃ�
	}
}
