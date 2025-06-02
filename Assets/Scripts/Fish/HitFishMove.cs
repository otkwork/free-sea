using System.Security.Cryptography;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class HitFishMove : MonoBehaviour
{
	[SerializeField] GameObject player; // プレイヤー
	[SerializeField] GameObject rodFloat;   // 浮き

	readonly Vector3 Size = new Vector3(0.35f, 0.35f, 1); // 魚のサイズ
	readonly Vector3 HitOffset = new Vector3(0, -3f, 0); // 魚がかかったときの位置オフセット
	const float MoveSpeed = 2f;           // 前進速度
	const float MoveInterval = 1.0f;      // 移動の間隔
	const float MoveAngle = 45f;          // 左右どちらかの移動角度（度）

	FishDataEntity m_fishData; // 魚のデータ
	Rigidbody m_rigidbody;

	bool isHit;

	float moveTimer = 0f; // 前進のタイマー
	void Start()
    {
		m_rigidbody = GetComponent<Rigidbody>();
		isHit = false; // 初期状態ではヒットしていない
	}

	// Update is called once per frame
	void Update()
    {
		m_rigidbody.isKinematic = Cursor.visible; ; // カーソルが表示されている場合は物理演算を無効化
		if (Cursor.visible) return; // カーソルが表示されている場合は何もしない(Pause)

		if (!isHit) return; // 魚がかかっていない場合は何もしない

		// 向きをプレイヤーから逃げる方向にする
		Vector3 toPlayer = player.transform.position - transform.position;
		Vector3 escapeDir = -toPlayer.normalized;
		transform.rotation = Quaternion.LookRotation(escapeDir);

		// 一定間隔で左右どちらかに前進
		moveTimer += Time.deltaTime;
		if (moveTimer >= MoveInterval)
		{
			moveTimer = 0f;

			// ランダムに左右を決定（-1 or 1）
			int direction = Random.value < 0.5f ? -1 : 1;

			// 左右に少し角度をずらして前進
			Quaternion offsetRot = Quaternion.Euler(0f, direction * MoveAngle, 0f);
			Vector3 moveDir = offsetRot * transform.forward;

			transform.position += moveDir * MoveSpeed;
		}

		// Y座標をrodの位置 - 3 に固定
		Vector3 pos = rodFloat.transform.position;
		pos.y -= 3f;
		transform.position = pos;
	}

	// 魚がかかった時の初期設定
	public void SetFishData(FishDataEntity fishData)
	{
		m_fishData = fishData;
		transform.localScale = Size * m_fishData.fishSize; // 魚のサイズを設定
		isHit = true; // 魚がかかった状態にする
	}

	public void FishingEnd()
	{
		transform.position = HitOffset;
		isHit = false; // 魚がかかった状態を解除
	}
}
