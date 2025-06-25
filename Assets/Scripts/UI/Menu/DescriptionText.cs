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
		// ���̃f�[�^��null�łȂ��ꍇ�̂ݕ\������
		if (fishData != null)
		{
            m_fishImage.enabled = true;
            m_fishImage.sprite = ImageLoader.LoadSpriteAsync(fishData.fishName).Result;
			if (!isGet)
			{
                m_fishImage.color = Color.black; // �����擾���Ă��Ȃ��ꍇ�͍�������
                m_fishNameText.text = "???";
                m_fishDescriptionText.text = "???"; // �����擾���Ă��Ȃ��ꍇ�͐������B��
			}
			else
			{
                m_fishImage.color = Color.white; // �����擾���Ă���ꍇ�͔�������
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
