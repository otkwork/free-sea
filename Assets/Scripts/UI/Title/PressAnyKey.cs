using UnityEngine;
using UnityEngine.UI;

public class PressAnyKey : MonoBehaviour
{
	// 点滅する時間間隔
	[SerializeField] private float blinkInterval = 0.5f;

	private float m_elapsedTime = 0f;
	private Image m_image;

	private void Start()
	{
		m_image = GetComponent<Image>();
	}

	// Update is called once per frame
	void Update()
    {
		m_elapsedTime += Time.deltaTime;
		if (blinkInterval < m_elapsedTime)
		{
			m_elapsedTime = 0f;
			// 画像の表示/非表示を切り替える
			m_image.enabled = !m_image.enabled;
		}
	}
}
