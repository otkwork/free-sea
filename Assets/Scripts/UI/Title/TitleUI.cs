using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
	[SerializeField] private float m_fadeInDuration = 3.0f; // フェードインの時間
	private CanvasGroup m_uiGroup;

	private static bool m_isActive = false; // UIがアクティブかどうか
	private float m_alpha = 0.0f; // UIの透明度

	private void Start()
	{
		m_uiGroup = GetComponent<CanvasGroup>();
		m_alpha = 0.0f; // 初期透明度
		m_uiGroup.alpha = m_alpha;

		Cursor.lockState = CursorLockMode.Locked; // カーソルをロック
		Cursor.visible = false; // カーソルを非表示にする
	}

	private void Update()
	{
		if (InputSystem.GetInputMenuButtonDown("Any"))
		{
			if (m_isActive)
			{
				SceneManager.LoadScene("Game"); // ゲームシーンに遷移
			}
			else
			{
				m_isActive = true; // UIがアクティブになる
			}
		}

		// フェードインの処理
		m_alpha += Time.deltaTime / m_fadeInDuration;
		if (m_isActive)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true; // UIがアクティブな場合はカーソルを表示
			m_alpha = 1.0f; // アクティブな場合は最大値に設定
		}

		// 1.0を超えている場合は1.0に制限
		if (m_alpha >= 1.0f)
		{
			m_isActive = true; // UIが完全に表示されるまでは操作不可
			m_alpha = 1.0f; // 最大値を超えないようにする
		}
		m_uiGroup.alpha = m_alpha;
	}
}
