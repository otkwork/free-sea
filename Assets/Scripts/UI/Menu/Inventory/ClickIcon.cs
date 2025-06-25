using UnityEngine;
using UnityEngine.EventSystems;

public class ClickIcon : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private DescriptionText m_descriptionText; // 説明テキストを表示するためのコンポーネント
	private FishDataEntity m_fishData;          // アイコンに表示するデータ
	private Inventory m_inventory;				// アイコンが所属するインベントリ
	private UnityEngine.UI.Image m_image;		// アイコンを押されたときに色を変えるためのImage
	private UnityEngine.UI.Image m_fishImage;	// アイコンに表示する魚の画像
	
	private bool m_isOnClick;

    private void Awake()
    {
        m_inventory = GetComponentInParent<Inventory>(); // 親のインベントリを取得
		m_image = GetComponent<UnityEngine.UI.Image>();
		m_fishImage = transform.GetChild(0).GetComponent<UnityEngine.UI.Image>(); // 子オブジェクトのFishImageを取得
    }

    private void Start()
	{
		m_fishData = null; // 初期状態では魚のデータはなし
        m_isOnClick = false;
	}

	void Update()
	{
        // クリックされているとき
        if (m_isOnClick)
		{
			m_descriptionText.SetDescription(m_fishData); // 説明テキストに魚のデータを設定する
		}

		if (m_fishData != null)
		{
			m_fishImage.enabled = true; // 魚の画像を表示する
			m_fishImage.sprite = ImageLoader.LoadSpriteAsync(m_fishData.fishName).Result; // 魚の画像をロードして表示する
		}
		else
		{
			m_fishImage.enabled = false; // 魚の画像を非表示にする
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

	// マウスが離れたとき
	public void OnPointerExit(PointerEventData eventData)
	{
		SetOnMouse(false);
	}

	// クリックされたときに呼ばれる
	public void OnPointerClick(PointerEventData eventData)
	{
		m_inventory.SetClickIcon(gameObject); // クリックされたアイコンをインベントリに設定する
	}

    public void OnDisable()
    {
        m_isOnClick = false;
        m_image.color = Color.white;
    }

    // クリックされたアイコンだけをクリック状態にしてそれ以外を解除する
    public void SetClick(bool isClick)
	{
		// 自分がクリックされたとき
		if (isClick)
		{
            m_isOnClick = !m_isOnClick; // クリック状態をトグルする
			m_image.color = m_isOnClick ? Color.blue : Color.white; // クリック時の処理（例: 色を変える）

			// 自分自身をクリックしてクリック状態を解除した場合
			if (!m_isOnClick)
			{
				m_descriptionText.ReSetDescription(); // 説明テキストをリセット
			}
		}
		// 他のアイコンがクリックされたとき
		else
		{
            m_isOnClick = false; // クリック状態を解除
			m_image.color = Color.white; // 色を元に戻す
		}
	}


	public void SetOnMouse(bool onMouse)
	{
		if (m_isOnClick) return; // クリック状態のときは何もしない

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