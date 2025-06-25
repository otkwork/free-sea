using UnityEngine;

public class RayGround : MonoBehaviour
{
	[SerializeField] private Transform m_groundParent;
	[SerializeField] private GameObject m_ground;   // 設置する地面オブジェクト
	[SerializeField] private Animator m_hammerAnime; // ハンマーのアニメーション
	private GameObject[] m_startGround = new GameObject[(int)AroundWall.WallType.Length]; // 初期の地面オブジェクト
							  
	// レイの最大距離
	private const float RayDistance = 10f;
	private const float SetGroundHeight = -0.8f; // 設置する地面の高さ（Y座標）

	private void Start()
	{
        m_hammerAnime.gameObject.SetActive(false); // ハンマーを非表示にする

		for (int i = 0; i < m_startGround.Length; ++i)
		{
			m_startGround[i] = m_groundParent.GetChild(i).gameObject;
		}
		GridObjectManager.Initialize(m_startGround);
	}

	void Update()
	{
		if (SelectItem.GetItemType() != SelectItem.ItemType.Hammer)
		{
            m_hammerAnime.gameObject.SetActive(false); // ハンマーを選択していない場合は無効にする
			return; // ハンマーを選択していない場合は何もしない	
		}
		else
		{
            m_hammerAnime.gameObject.SetActive(true); // ハンマーを選択している場合は有効にする
		}

		if (PlayerController.IsPause()) return; // カーソルが表示されている場合は何もしない(Pause)
		
		// マウス左クリックでRay発射
		if (InputSystem.UseItem())
		{
			CheckRay();
		}
	}

	private void CheckRay()
	{
		// カメラの中心から下方向（Vector3.down）にRayを飛ばす
		Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

		// Water以外のレイヤーマスクを作成
		int waterLayer = LayerMask.NameToLayer("Water");
		int waterMask = 1 << waterLayer;
		int mask = ~waterMask;

		if (Physics.Raycast(ray, out RaycastHit hit, RayDistance, waterMask))
		{
			// ヒットしたワールド座標をグリッド座標（整数化）に変換（XZ平面）
			Vector3 hitPos = hit.point;
			Vector2Int gridPos = new Vector2Int(GridObjectManager.OddRound(hitPos.x), GridObjectManager.OddRound(hitPos.z));

			// 上下左右にオブジェクトがあるか判定
			bool hasNeighbor = GridObjectManager.HasNeighborObject(gridPos);

			if (hasNeighbor)
			{
                m_hammerAnime.SetTrigger("Create"); // ハンマーのアニメーションを再生
				GameObject ground = Instantiate(m_ground, new Vector3(gridPos.x, SetGroundHeight, gridPos.y), Quaternion.identity, m_groundParent);
				GridObjectManager.AddObject(gridPos, ground); // グリッド座標に地面オブジェクトを登録
			}
			else
			{
				Debug.Log($"グリッド座標 {gridPos} の上下左右にオブジェクトはありません");
			}
		}
		else
		{
			Debug.Log("Rayがオブジェクトにヒットしませんでした");
		}

		Debug.DrawRay(ray.origin, ray.direction * RayDistance, Color.blue, 2f); // Rayを可視化
	}
}
