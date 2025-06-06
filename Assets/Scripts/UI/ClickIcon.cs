using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// ���̃X�N���v�g��UI�I�u�W�F�N�g�iImage��Text�Ȃǁj�ɃA�^�b�`����ƁA�N���b�N�����m�ł��܂��B
/// </summary>
public class ClickIcon : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	UnityEngine.UI.Image image;

	bool isOnClick;

	private void Start()
	{
		isOnClick = false;
		image = GetComponent<UnityEngine.UI.Image>();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!isOnClick) image.color = Color.yellow;
	}

	// �}�E�X�����ꂽ�Ƃ�
	public void OnPointerExit(PointerEventData eventData)
	{
		if (!isOnClick) image.color = Color.white;
	}

	// �N���b�N���ꂽ�Ƃ��ɌĂ΂��
	public void OnPointerClick(PointerEventData eventData)
	{
		isOnClick = !isOnClick; // �N���b�N��Ԃ��g�O������
		image.color = isOnClick ? Color.red : Color.white; // �N���b�N���̏����i��: �F��ς���j
	}
}