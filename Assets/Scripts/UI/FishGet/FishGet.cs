using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishGet : MonoBehaviour
{
	[SerializeField] private GameObject m_backGround; // 背景
	[SerializeField] private Image m_fishImage;
	[SerializeField] private TextMeshProUGUI m_fishName;
	[SerializeField] private AudioClip m_getFishSe;

	private const float ActiveTime = 3.0f; // 表示する時間

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
			// ActiveTimeに設定した時間表示し続ける
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
		SoundEffect.Play2D(m_getFishSe);
	}
}
