using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionText : MonoBehaviour
{
	[SerializeField] private Image m_fishImage;
	[SerializeField] private TextMeshProUGUI m_fishNameText;
	[SerializeField] private TextMeshProUGUI m_fishPriceText;
	[SerializeField] private TextMeshProUGUI m_fishDescriptionText;

	private void Start()
	{
		ReSetDescription();
	}

	public void ReSetDescription()
	{
        m_fishImage.enabled = false;
        m_fishNameText.text = string.Empty;
		m_fishPriceText.text = string.Empty; // 価格のテキストもリセット
		m_fishDescriptionText.text = string.Empty;
	}

	public void SetDescription(FishDataEntity fishData, bool isGet = true)
	{
		// 魚のデータがnullでない場合のみ表示する
		if (fishData != null)
		{
            m_fishImage.enabled = true;
            m_fishImage.sprite = ImageLoader.LoadSpriteAsync(fishData.fishName).Result;
			if (!isGet)
			{
                m_fishImage.color = Color.black; // 魚を取得していない場合は黒くする
                m_fishNameText.text = "???";
				m_fishPriceText.text = "???"; // 価格も隠す
				m_fishDescriptionText.text = "???"; // 魚を取得していない場合は説明を隠す
			}
			else
			{
                m_fishImage.color = Color.white; // 魚を取得している場合は白くする
                m_fishNameText.text = fishData.displayName;
				m_fishPriceText.text = "$" + fishData.price.ToString(); // 価格を表示
				m_fishDescriptionText.text = fishData.fishDescription;
			}
		}
		else
		{
			ReSetDescription();
		}
	}

	public void OnDisable()
	{
		ReSetDescription();
	}
}
