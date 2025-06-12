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

	public void SetDescription(FishDataEntity fishData, bool isGet = true)
	{
		// 魚のデータがnullでない場合のみ表示する
		if (fishData != null)
		{
			fishImage.enabled = true;
			fishImage.sprite = ImageLoader.LoadSpriteAsync(fishData.fishName).Result;
			if (!isGet)
			{
				fishImage.color = Color.black; // 魚を取得していない場合は黒くする
				fishNameText.text = "???";
				fishDescriptionText.text = "???"; // 魚を取得していない場合は説明を隠す
			}
			else
			{
				fishImage.color = Color.white; // 魚を取得している場合は白くする
				fishNameText.text = fishData.displayName;
				fishDescriptionText.text = fishData.fishDescription;
			}
		}
		else
		{
			ReSetDescription();
		}
	}
}
