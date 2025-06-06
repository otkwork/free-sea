using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// このスクリプトをUIオブジェクト（ImageやTextなど）にアタッチすると、クリックを検知できます。
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

	// マウスが離れたとき
	public void OnPointerExit(PointerEventData eventData)
	{
		if (!isOnClick) image.color = Color.white;
	}

	// クリックされたときに呼ばれる
	public void OnPointerClick(PointerEventData eventData)
	{
		isOnClick = !isOnClick; // クリック状態をトグルする
		image.color = isOnClick ? Color.red : Color.white; // クリック時の処理（例: 色を変える）
	}
}