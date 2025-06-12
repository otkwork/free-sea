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
		// ���̃f�[�^��null�łȂ��ꍇ�̂ݕ\������
		if (fishData != null)
		{
			fishImage.enabled = true;
			fishImage.sprite = ImageLoader.LoadSpriteAsync(fishData.fishName).Result;
			if (!isGet)
			{
				fishImage.color = Color.black; // �����擾���Ă��Ȃ��ꍇ�͍�������
				fishNameText.text = "???";
				fishDescriptionText.text = "???"; // �����擾���Ă��Ȃ��ꍇ�͐������B��
			}
			else
			{
				fishImage.color = Color.white; // �����擾���Ă���ꍇ�͔�������
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
