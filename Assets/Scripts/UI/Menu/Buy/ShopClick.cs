using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopClick : MonoBehaviour
{
	[SerializeField] private Confirmation m_confirmation; // �m�F�p�l���̃X�N���v�g�ւ̎Q��
	[SerializeField] private Image m_image; // �A�C�e���̉摜��\������Image
	[SerializeField] private TextMeshProUGUI m_amountText; // �A�C�e���̐��ʂ�\������Text
	[SerializeField] private TextMeshProUGUI m_priceText; // �A�C�e���̉��i��\������Text

	[SerializeField] private Sprite m_sprite; // �A�C�e���̉摜
	[SerializeField] private int m_amount; // �A�C�e���̐���
	[SerializeField] private int m_price; // �A�C�e���̉��i


	void Start()
	{
		m_image.sprite = m_sprite; // �A�C�e���̉摜��ݒ�
		m_amountText.text = "�~" + m_amount.ToString(); // �A�C�e���̐��ʂ�ݒ�
		m_priceText.text = "$" + m_price.ToString(); // �A�C�e���̉��i��ݒ�
	}

	public void OnClickIcon()
	{
		m_confirmation.SetPanel(true); // �m�F�p�l�����J��
		m_confirmation.SetConfirmationPanel(m_sprite, m_amount, m_price);
	}
}
