using UnityEngine;

public class Confirmation : MonoBehaviour
{
	[SerializeField] private GameObject m_confirmationPanel; // �m�F�p�l����GameObject

	private static bool m_isPanelOpen; // �p�l�����J���Ă��邩�ǂ����̃t���O

	// Start is called before the first frame update
	void Start()
    {
        m_isPanelOpen = false; // ������Ԃł̓p�l���͊J���Ă��Ȃ�
		m_confirmationPanel.SetActive(false); // �m�F�p�l�����\���ɂ���
	}

	public void SetPanel(bool setPanel)
	{
		m_isPanelOpen = setPanel; // �p�l����\��/��\��
	}

	// �N���b�N�����A�C�e�����p�l���̏����擾����
	public void SetConfirmationPanel(Sprite itemImage, int itemAmount, int itemPrice)
	{
		m_isPanelOpen = true; // �p�l�����J��
		m_confirmationPanel.SetActive(true);
		m_confirmationPanel.GetComponent<ConfirmationPanel>().SetInfomation(itemImage, itemAmount, itemPrice); // �p�l���ɃA�C�e������ݒ肷��
	}
}
