using UnityEngine;
using UnityEngine.UI;

public class PressAnyKey : MonoBehaviour
{
	// “_–Å‚·‚éŽžŠÔŠÔŠu
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
			// ‰æ‘œ‚Ì•\Ž¦/”ñ•\Ž¦‚ðØ‚è‘Ö‚¦‚é
			m_image.enabled = !m_image.enabled;
		}
	}
}
