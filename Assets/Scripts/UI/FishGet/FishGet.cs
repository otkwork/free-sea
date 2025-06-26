using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class FishGet : MonoBehaviour
{
	[SerializeField] private GameObject m_backGround; // ”wŒi
	[SerializeField] private Image m_fishImage;
	[SerializeField] private TextMeshProUGUI m_fishName;

	private const float ActiveTime = 3.0f; // •\Ž¦‚·‚éŽžŠÔ

	private bool m_isActive;
	private float m_elapsedTime;

	private void Start()
	{
		m_isActive = false;
		m_elapsedTime = 0.0f;
		m_backGround.SetActive(false);
	}

	private void Update()
	{
		if (m_isActive)
		{
			m_elapsedTime += Time.deltaTime;
			// ActiveTime‚ÉÝ’è‚µ‚½ŽžŠÔ•\Ž¦‚µ‘±‚¯‚é
			if (m_elapsedTime >= ActiveTime)
			{
				m_isActive = false;
				m_elapsedTime = 0.0f;
				m_backGround.SetActive(false);
			}
        }
    }

	public void FishingEnd(FishDataEntity fishData)
	{
        m_isActive = true;
		m_backGround.SetActive(true);
		ImageLoader.LoadSpriteAsync(fishData.fishName).Completed += op =>
        m_fishImage.sprite = op.Result;
		m_fishName.text = fishData.displayName;
	}
}
