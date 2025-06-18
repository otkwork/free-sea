using UnityEngine;

public class RayGround : MonoBehaviour
{
	[SerializeField] GameObject m_ground;	// 設置する地面オブジェクト
	// レイの最大距離
	const float rayDistance = 100f;
	const float SetGroundHeight = -0.8f; // 設置する地面の高さ（Y座標）

	private void Start()
	{
		GridObjectManager.Initialize();
	}

	void Update()
	{
		if (PlayerController.IsPause()) return; // カーソルが表示されている場合は何もしない(Pause)
		
		// マウス左クリックでRay発射
		if (Input.GetMouseButtonDown(0))
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

		if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, waterMask))
		{
			Debug.Log(hit.transform.name);
			// ヒットしたワールド座標をグリッド座標（整数化）に変換（XZ平面）
			Vector3 hitPos = hit.point;
			Vector2Int gridPos = new Vector2Int(OddRound(hitPos.x), OddRound(hitPos.z));

			// 上下左右にオブジェクトがあるか判定
			bool hasNeighbor = GridObjectManager.HasNeighborObject(gridPos);

			if (hasNeighbor)
			{
				Debug.Log($"グリッド座標 {gridPos} の上下左右にオブジェクトがあります");
				Instantiate(m_ground, new Vector3(gridPos.x, SetGroundHeight, gridPos.y), Quaternion.identity);
				GridObjectManager.AddObject(gridPos); // グリッド座標に地面オブジェクトを登録
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

		Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red, 2f); // Rayを可視化
	}

	private int OddRound(float value)
	{
		int rounded = Mathf.RoundToInt(value);
		// 偶数なら1足して奇数化（または-1でもOK）
		if (rounded % 2 == 0)
		{
			// valueがroundedより大きければ+1、小さければ-1（近い方の奇数に寄せる）
			if (value >= rounded)
				return rounded + 1;
			else
				return rounded - 1;
		}
		else
		{
			return rounded;
		}
	}
}
