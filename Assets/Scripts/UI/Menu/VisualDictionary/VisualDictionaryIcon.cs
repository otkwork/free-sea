using UnityEngine;
using UnityEngine.EventSystems;

public class VisualDictionaryIcon : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private DescriptionText m_descriptionText; // 説明テキストを表示するためのコンポーネント
	FishDataEntity m_fishData;          // アイコンに表示するデータ
	VisualDictionary m_visualDictionary;// アイコンが所属する図鑑
	UnityEngine.UI.Image m_image;       // アイコンを押されたときに色を変えるためのImage
	UnityEngine.UI.Image m_fishImage;   // アイコンに表示する魚の画像

	bool isOnClick;

    private void Awake()
    {
        m_visualDictionary = GetComponentInParent<VisualDictionary>(); // 親のインベントリを取得
		m_image = GetComponent<UnityEngine.UI.Image>();
		m_fishImage = transform.GetChild(0).GetComponent<UnityEngine.UI.Image>(); // 子オブジェクトのFishImageを取得
		isOnClick = false;
    }


	void Update()
	{
		// クリックされているとき
		if (isOnClick)
		{
			m_descriptionText.SetDescription(m_fishData, m_visualDictionary.IsGetFish(m_fishData.id)); // 説明テキストに魚のデータを設定する
		}

		if (m_fishData != null)
		{
			m_fishImage.enabled = true; // 魚の画像を表示する
			m_fishImage.sprite = ImageLoader.LoadSpriteAsync(m_fishData.fishName).Result; // 魚の画像をロードして表示する
		}
	}

	public void SetFishData(FishDataEntity fishData)
	{
		m_fishData = fishData;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		SetOnMouse(true); // マウスがアイコンの上にあるときは色を変える
	}

	// マウスが離れたとき
	public void OnPointerExit(PointerEventData eventData)
	{
		SetOnMouse(false);
	}

	// クリックされたときに呼ばれる
	public void OnPointerClick(PointerEventData eventData)
	{
		m_visualDictionary.SetClickIcon(gameObject); // クリックされたアイコンを図鑑に設定する
	}
    public void OnDisable()
    {
        isOnClick = false;
		m_image.color = Color.white;
    }

    // クリックされたアイコンだけをクリック状態にしてそれ以外を解除する
    public void SetClick(bool isClick)
	{
		// 自分がクリックされたとき
		if (isClick)
		{
			isOnClick = !isOnClick; // クリック状態をトグルする
			m_image.color = isOnClick ? Color.red : Color.white; // クリック時の処理（例: 色を変える）

			// 自分自身をクリックしてクリック状態を解除した場合
			if (!isOnClick)
			{
				m_descriptionText.ReSetDescription(); // 説明テキストをリセット
			}
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
}