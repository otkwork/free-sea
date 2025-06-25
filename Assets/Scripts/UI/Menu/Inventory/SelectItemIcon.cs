using UnityEngine;
using UnityEngine.EventSystems;

public class SelectItemIcon : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] SelectItem.ItemType m_itemType; // �A�C�R���̎��
	SelectItem m_selectItem;            // �A�C�R������������C���x���g��
	UnityEngine.UI.Image m_image;       // �A�C�R���������ꂽ�Ƃ��ɐF��ς��邽�߂�Image
	UnityEngine.UI.Image m_itemImage;   // �A�C�R���ɕ\�����鋛�̉摜

	bool isOnClick;

	private void Awake()
	{
		m_selectItem = GetComponentInParent<SelectItem>(); // �e�̃C���x���g�����擾
		m_image = GetComponent<UnityEngine.UI.Image>();
		m_itemImage = transform.GetChild(0).GetComponent<UnityEngine.UI.Image>(); // �q�I�u�W�F�N�g��ItemImage���擾
	}

	private void Start()
	{
		isOnClick = false;
	}

	void Update()
	{ 
		if (m_itemType == SelectItem.ItemType.Hammer) m_itemImage.color = SelectItem.GetIsHaveHammer() ? Color.white : Color.black; // �n���}�[�������Ă��Ȃ��ꍇ�̓A�C�R������������
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
		m_selectItem.Select(gameObject); // �N���b�N���ꂽ�A�C�R����ݒ肷��
	}

	// �N���b�N���ꂽ�A�C�R���������N���b�N��Ԃɂ��Ă���ȊO����������
	public void SetClick(bool isClick)
	{
		// �������N���b�N���ꂽ�Ƃ�
		if (isClick)
		{
			isOnClick = true; // �N���b�N��Ԃ�ݒ�
			m_image.color = isOnClick ? Color.red : Color.white; // �N���b�N���̏����i��: �F��ς���j
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

	public SelectItem.ItemType GetItemType()
	{
		return m_itemType; // �A�C�R���̎�ނ�Ԃ�
	}
}