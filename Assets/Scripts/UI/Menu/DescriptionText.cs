using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionText : MonoBehaviour
{
	[SerializeField] Image fishImage;
	[SerializeField] TextMeshProUGUI fishNameText;
	[SerializeField] TextMeshProUGUI fishDescriptionText;

	private void Start()
	{
		ReSetDescription();
	}

	public void ReSetDescription()
	{
		fishImage.enabled = false;
		fishNameText.text = string.Empty;
		fishDescriptionText.text = string.Empty;
	}

	public void SetDescription(FishDataEntity fishData)
	{
		// 魚のデータがnullでない場合のみ表示する
		if (fishData != null)
		{
			fishImage.enabled = true;
			fishImage.sprite = ImageLoader.LoadSpriteAsync(fishData.fishName).Result;
			fishNameText.text = fishData.displayName;
			fishDescriptionText.text = fishData.fishDescription;
		}
		else
		{
			ReSetDescription();
		}
	}
}
