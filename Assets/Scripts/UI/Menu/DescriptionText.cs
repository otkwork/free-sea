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
		m_fishPriceText.text = string.Empty; // ���i�̃e�L�X�g�����Z�b�g
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
				m_fishPriceText.text = "???"; // ���i���B��
				m_fishDescriptionText.text = "???"; // �����擾���Ă��Ȃ��ꍇ�͐������B��
			}
			else
			{
                m_fishImage.color = Color.white; // �����擾���Ă���ꍇ�͔�������
                m_fishNameText.text = fishData.displayName;
				m_fishPriceText.text = "$" + fishData.price.ToString(); // ���i��\��
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
