using UnityEngine;

public class ShopingButton : MonoBehaviour
{
	[SerializeField] private GameObject m_confirmationPanel; // �m�F�p�l����GameObject�ւ̎Q��
	[SerializeField] private ButtonType m_buttonType; // �{�^���̎�ށiYes/No�j

	private enum ButtonType
	{
		Yes,
		No
	}

	public void OnClick()
	{
		// �w��
		if (m_buttonType == ButtonType.Yes)
		{
			Money.UseMoney(ConfirmationPanel.GetPrice()); // �m�F�p�l�����牿�i���擾���Ă������g�p
			RayGround.AddGround(ConfirmationPanel.GetAmount()); // �m�F�p�l�����琔�ʂ��擾���Ēn�ʂ�ǉ�
		}
		m_confirmationPanel.SetActive(false); // �m�F�p�l�������
	}
}
