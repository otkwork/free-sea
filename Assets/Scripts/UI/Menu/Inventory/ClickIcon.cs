using UnityEngine;
using UnityEngine.EventSystems;

public class ClickIcon : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private DescriptionText m_descriptionText; // �����e�L�X�g��\�����邽�߂̃R���|�[�l���g
	private FishDataEntity m_fishData;          // �A�C�R���ɕ\������f�[�^
	private Inventory m_inventory;				// �A�C�R������������C���x���g��
	private UnityEngine.UI.Image m_image;		// �A�C�R���������ꂽ�Ƃ��ɐF��ς��邽�߂�Image
	private UnityEngine.UI.Image m_fishImage;	// �A�C�R���ɕ\�����鋛�̉摜
	
	private bool m_isOnClick;

    private void Awake()
    {
        m_inventory = GetComponentInParent<Inventory>(); // �e�̃C���x���g�����擾
		m_image = GetComponent<UnityEngine.UI.Image>();
		m_fishImage = transform.GetChild(0).GetComponent<UnityEngine.UI.Image>(); // �q�I�u�W�F�N�g��FishImage���擾
    }

    private void Start()
	{
		m_fishData = null; // ������Ԃł͋��̃f�[�^�͂Ȃ�
        m_isOnClick = false;
	}

	void Update()
	{
        // �N���b�N����Ă���Ƃ�
        if (m_isOnClick)
		{
			m_descriptionText.SetDescription(m_fishData); // �����e�L�X�g�ɋ��̃f�[�^��ݒ肷��
		}

		if (m_fishData != null)
		{
			m_fishImage.enabled = true; // ���̉摜��\������
			m_fishImage.sprite = ImageLoader.LoadSpriteAsync(m_fishData.fishName).Result; // ���̉摜�����[�h���ĕ\������
		}
		else
		{
			m_fishImage.enabled = false; // ���̉摜���\���ɂ���
		}
	}

	public void SetFishData(FishDataEntity fishData)
	{
		m_fishData = fishData;
	}

	public FishDataEntity GetClickData()
	{
		return m_fishData;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		SetOnMouse(true);
	}

	// �}�E�X�����ꂽ�Ƃ�
	public void OnPointerExit(PointerEventData eventData)
	{
		SetOnMouse(false);
	}

	// �N���b�N���ꂽ�Ƃ��ɌĂ΂��
	public void OnPointerClick(PointerEventData eventData)
	{
		m_inventory.SetClickIcon(gameObject); // �N���b�N���ꂽ�A�C�R�����C���x���g���ɐݒ肷��
	}

    public void OnDisable()
    {
        m_isOnClick = false;
        m_image.color = Color.white;
    }

    // �N���b�N���ꂽ�A�C�R���������N���b�N��Ԃɂ��Ă���ȊO����������
    public void SetClick(bool isClick)
	{
		// �������N���b�N���ꂽ�Ƃ�
		if (isClick)
		{
            m_isOnClick = !m_isOnClick; // �N���b�N��Ԃ��g�O������
			m_image.color = m_isOnClick ? Color.blue : Color.white; // �N���b�N���̏����i��: �F��ς���j

			// �������g���N���b�N���ăN���b�N��Ԃ����������ꍇ
			if (!m_isOnClick)
			{
				m_descriptionText.ReSetDescription(); // �����e�L�X�g�����Z�b�g
			}
		}
		// ���̃A�C�R�����N���b�N���ꂽ�Ƃ�
		else
		{
            m_isOnClick = false; // �N���b�N��Ԃ�����
			m_image.color = Color.white; // �F�����ɖ߂�
		}
	}


	public void SetOnMouse(bool onMouse)
	{
		if (m_isOnClick) return; // �N���b�N��Ԃ̂Ƃ��͉������Ȃ�

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