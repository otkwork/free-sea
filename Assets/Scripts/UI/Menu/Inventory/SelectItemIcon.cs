using UnityEngine;
using UnityEngine.EventSystems;

public class SelectItemIcon : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] SelectItem.ItemType m_itemType; // アイコンの種類
	SelectItem m_selectItem;            // アイコンが所属するインベントリ
	UnityEngine.UI.Image m_image;       // アイコンを押されたときに色を変えるためのImage
	UnityEngine.UI.Image m_itemImage;   // アイコンに表示する魚の画像

	bool isOnClick;

	private void Awake()
	{
		m_selectItem = GetComponentInParent<SelectItem>(); // 親のインベントリを取得
		m_image = GetComponent<UnityEngine.UI.Image>();
		m_itemImage = transform.GetChild(0).GetComponent<UnityEngine.UI.Image>(); // 子オブジェクトのItemImageを取得
	}

	private void Start()
	{
		isOnClick = false;
	}

	void Update()
	{ 
		if (m_itemType == SelectItem.ItemType.Hammer) m_itemImage.color = SelectItem.GetIsHaveHammer() ? Color.white : Color.black; // ハンマーを持っていない場合はアイコンを黒くする
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		SetOnMouse(true);
	}

	// マウスが離れたとき
	public void OnPointerExit(PointerEventData eventData)
	{
		SetOnMouse(false);
	}

	// クリックされたときに呼ばれる
	public void OnPointerClick(PointerEventData eventData)
	{
		m_selectItem.Select(gameObject); // クリックされたアイコンを設定する
	}

	// クリックされたアイコンだけをクリック状態にしてそれ以外を解除する
	public void SetClick(bool isClick)
	{
		// 自分がクリックされたとき
		if (isClick)
		{
			isOnClick = true; // クリック状態を設定
			m_image.color = isOnClick ? Color.red : Color.white; // クリック時の処理（例: 色を変える）
		}
		// 他のアイコンがクリックされたとき
		else
		{
			isOnClick = false; // クリック状態を解除
			m_image.color = Color.white; // 色を元に戻す
		}
	}


	public void SetOnMouse(bool onMouse)
	{
		if (isOnClick) return; // クリック状態のときは何もしない

		// マウスがアイコンの上にあるとき
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
		return m_itemType; // アイコンの種類を返す
	}
}