using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
	[SerializeField] private float m_fadeInDuration = 3.0f; // �t�F�[�h�C���̎���
	[SerializeField] private AudioClip m_bgm;
	[SerializeField] private AudioClip m_startSe;
	private CanvasGroup m_uiGroup;
	private AudioSource m_audioSource;

	private bool m_isPlaying;

	private void Start()
	{
		SoundEffect.Play2D(m_bgm, true);
		m_uiGroup = GetComponent<CanvasGroup>();
		m_uiGroup.alpha = 0.0f;
		m_audioSource = null;
		m_isPlaying = false;

		Cursor.lockState = CursorLockMode.Locked; // �J�[�\�������b�N
		Cursor.visible = false; // �J�[�\�����\���ɂ���
	}

	private void Update()
	{
		// �T�E���h���Ȃ��Ă���T�E���h����������
		if (m_isPlaying && m_audioSource == null) SceneManager.LoadScene("Game"); // �Q�[���V�[���ɑJ��

        if (InputSystem.GetInputMenuButtonDown("Any") || Input.GetMouseButtonDown(0) && !m_isPlaying)
		{
			m_isPlaying = true;
			m_audioSource = SoundEffect.Play2D(m_startSe);
		}

		// �t�F�[�h�C���̏���
		m_uiGroup.alpha += Time.deltaTime / m_fadeInDuration;
    }
}
