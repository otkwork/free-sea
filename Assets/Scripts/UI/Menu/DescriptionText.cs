using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionText : MonoBehaviour
{
	[SerializeField] Image m_fishImage;
	[SerializeField] TextMeshProUGUI m_fishNameText;
	[SerializeField] TextMeshProUGUI m_fishDescriptionText;

	private void Start()
	{
		ReSetDescription();
	}

	public void ReSetDescription()
	{
        m_fishImage.enabled = false;
        m_fishNameText.text = string.Empty;
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
                m_fishDescriptionText.text = "???"; // 魚を取得していない場合は説明を隠す
			}
			else
			{
                m_fishImage.color = Color.white; // 魚を取得している場合は白くする
                m_fishNameText.text = fishData.displayName;
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
