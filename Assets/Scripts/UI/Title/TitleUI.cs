using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
	[SerializeField] private float m_fadeInDuration = 3.0f; // フェードインの時間
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

		Cursor.lockState = CursorLockMode.Locked; // カーソルをロック
		Cursor.visible = false; // カーソルを非表示にする
	}

	private void Update()
	{
		// サウンドがなってからサウンドが消えたら
		if (m_isPlaying && m_audioSource == null) SceneManager.LoadScene("Game"); // ゲームシーンに遷移

        if (InputSystem.GetInputMenuButtonDown("Any") || Input.GetMouseButtonDown(0) && !m_isPlaying)
		{
			m_isPlaying = true;
			m_audioSource = SoundEffect.Play2D(m_startSe);
		}

		// フェードインの処理
		m_uiGroup.alpha += Time.deltaTime / m_fadeInDuration;
    }
}
