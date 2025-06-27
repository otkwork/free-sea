using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
	[SerializeField] private float m_fadeInDuration = 3.0f; // �t�F�[�h�C���̎���
	private CanvasGroup m_uiGroup;

	private static bool m_isActive = false; // UI���A�N�e�B�u���ǂ���
	private float m_alpha = 0.0f; // UI�̓����x

	private void Start()
	{
		m_uiGroup = GetComponent<CanvasGroup>();
		m_alpha = 0.0f; // ���������x
		m_uiGroup.alpha = m_alpha;

		Cursor.lockState = CursorLockMode.Locked; // �J�[�\�������b�N
		Cursor.visible = false; // �J�[�\�����\���ɂ���
	}

	private void Update()
	{
		if (InputSystem.GetInputMenuButtonDown("Any"))
		{
			if (m_isActive)
			{
				SceneManager.LoadScene("Game"); // �Q�[���V�[���ɑJ��
			}
			else
			{
				m_isActive = true; // UI���A�N�e�B�u�ɂȂ�
			}
		}

		// �t�F�[�h�C���̏���
		m_alpha += Time.deltaTime / m_fadeInDuration;
		if (m_isActive)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true; // UI���A�N�e�B�u�ȏꍇ�̓J�[�\����\��
			m_alpha = 1.0f; // �A�N�e�B�u�ȏꍇ�͍ő�l�ɐݒ�
		}

		// 1.0�𒴂��Ă���ꍇ��1.0�ɐ���
		if (m_alpha >= 1.0f)
		{
			m_isActive = true; // UI�����S�ɕ\�������܂ł͑���s��
			m_alpha = 1.0f; // �ő�l�𒴂��Ȃ��悤�ɂ���
		}
		m_uiGroup.alpha = m_alpha;
	}
}
