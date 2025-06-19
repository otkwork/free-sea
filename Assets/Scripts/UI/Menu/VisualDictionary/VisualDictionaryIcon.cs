using UnityEngine;
using UnityEngine.EventSystems;

public class VisualDictionaryIcon : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private DescriptionText m_descriptionText; // �����e�L�X�g��\�����邽�߂̃R���|�[�l���g
	FishDataEntity m_fishData;          // �A�C�R���ɕ\������f�[�^
	VisualDictionary m_visualDictionary;// �A�C�R������������}��
	UnityEngine.UI.Image m_image;       // �A�C�R���������ꂽ�Ƃ��ɐF��ς��邽�߂�Image
	UnityEngine.UI.Image m_fishImage;   // �A�C�R���ɕ\�����鋛�̉摜

	bool isOnClick;

    private void Awake()
    {
        m_visualDictionary = GetComponentInParent<VisualDictionary>(); // �e�̃C���x���g�����擾
		m_image = GetComponent<UnityEngine.UI.Image>();
		m_fishImage = transform.GetChild(0).GetComponent<UnityEngine.UI.Image>(); // �q�I�u�W�F�N�g��FishImage���擾
		isOnClick = false;
    }


	void Update()
	{
		// �N���b�N����Ă���Ƃ�
		if (isOnClick)
		{
			m_descriptionText.SetDescription(m_fishData, m_visualDictionary.IsGetFish(m_fishData.id)); // �����e�L�X�g�ɋ��̃f�[�^��ݒ肷��
		}

		if (m_fishData != null)
		{
			m_fishImage.enabled = true; // ���̉摜��\������
			m_fishImage.sprite = ImageLoader.LoadSpriteAsync(m_fishData.fishName).Result; // ���̉摜�����[�h���ĕ\������
		}
	}

	public void SetFishData(FishDataEntity fishData)
	{
		m_fishData = fishData;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		SetOnMouse(true); // �}�E�X���A�C�R���̏�ɂ���Ƃ��͐F��ς���
	}

	// �}�E�X�����ꂽ�Ƃ�
	public void OnPointerExit(PointerEventData eventData)
	{
		SetOnMouse(false);
	}

	// �N���b�N���ꂽ�Ƃ��ɌĂ΂��
	public void OnPointerClick(PointerEventData eventData)
	{
		m_visualDictionary.SetClickIcon(gameObject); // �N���b�N���ꂽ�A�C�R����}�ӂɐݒ肷��
	}
    public void OnDisable()
    {
        isOnClick = false;
		m_image.color = Color.white;
    }

    // �N���b�N���ꂽ�A�C�R���������N���b�N��Ԃɂ��Ă���ȊO����������
    public void SetClick(bool isClick)
	{
		// �������N���b�N���ꂽ�Ƃ�
		if (isClick)
		{
			isOnClick = !isOnClick; // �N���b�N��Ԃ��g�O������
			m_image.color = isOnClick ? Color.red : Color.white; // �N���b�N���̏����i��: �F��ς���j

			// �������g���N���b�N���ăN���b�N��Ԃ����������ꍇ
			if (!isOnClick)
			{
				m_descriptionText.ReSetDescription(); // �����e�L�X�g�����Z�b�g
			}
		}
		// ���̃A�C�R�����N���b�N���ꂽ�Ƃ�
		else
		{
			isOnClick = false; // �N���b�N��Ԃ�����
			m_image.color = Color.white; // �F�����ɖ߂�
		}
	}

	public void SetOnMouse(bool onMouse)
	{
		if (isOnClick) return; // �N���b�N��Ԃ̂Ƃ��͉������Ȃ�

		// �}�E�X���A�C�R���̏�ɂ���Ƃ�
		if (onMouse)
		{
			m_image.color = Color.yellow;
		}
		else
		{
			m_image.color = Color.white;
		}
	}
}