using System.Drawing;
using System.Security.Cryptography;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class HitFishMove : MonoBehaviour
{

	[SerializeField] Transform lure;					// ルアー（釣り針）のTransform
	[SerializeField] Transform player;					// プレイヤーのTransform
	[SerializeField] float moveInterval = 1.5f;			// 左右に動く間隔（秒）
	[SerializeField] float nextMoveTime = 5f;           // 一回の左右に動きづづける時間（秒）
	[SerializeField] float lateralMoveDistance = 2.0f;	// 左右に動く最大距離
	[SerializeField] float lateralMoveSpeed = 4.0f;     // 左右に動くスピード
	FishDataEntity m_fishData; // 魚のデータ

	readonly Vector3 Size = new Vector3(0.5f, 0.5f, 0.5f); // 魚のサイズ
	readonly Vector3 HitOffset = new Vector3(0, -3f, 0); // 魚がかかったときの位置オフセット

	Quaternion m_fishingStartRot; // 釣り開始時の向き
	private float elapsedTime = 0f;
	private bool isMovingLaterally = false;
	private Vector3 targetLateralPos;
	private int lateralDir = 1; // -1: 左, 1: 右
	private bool isfishing; // 浮きが着水したかどうか
	private bool isHit; // 魚がかかったかどうか

	void Start()
	{
		isfishing = false; // 初期状態ではヒットしていない
		isHit = false;
	}


	void Update()
	{
		if (PlayerController.IsPause()) return; // カーソルが表示されている場合は何もしない(Pause)

		if (!isfishing)
		{
			isMovingLaterally = false; // 釣り状態でない場合は横移動を無効にする
			elapsedTime = 0f; // 時間をリセット
			return; // 魚がかかっていない場合は何もしない
		}
		// ルアーの下を基準位置とする
		Vector3 basePos = lure.position + HitOffset + player.forward * 5;

		if (!isMovingLaterally)
		{
			// プレイヤーから逃げる方向を向く
			transform.rotation = m_fishingStartRot;

			// ルアーの下に位置する
			transform.position = Vector3.Lerp(transform.position, basePos, 0.01f);
			transform.position = new Vector3(transform.position.x, basePos.y, transform.position.z); // 高さは基準位置と同じにする

			if (!isHit) return; // 魚がかかっていない場合は何もしない
			elapsedTime += Time.deltaTime;
			// 一定間隔で左右移動開始
			if (elapsedTime > moveInterval)
			{
				isMovingLaterally = true;
				elapsedTime = 0f; // 時間をリセット

				// 左右どちらか選ぶ
				lateralDir = Random.value > 0.5f ? 1 : -1;

				// 横移動の方向に向く（0度:上、45度:右、-45度:左）
				float lateralAngle = lateralDir == 1 ? 45f : -45f;
				transform.rotation = Quaternion.Euler(0, m_fishingStartRot.eulerAngles.y + lateralAngle, 0);
			}
		}
		else
		{
			// 横移動中の処理
			elapsedTime += Time.deltaTime;

			targetLateralPos = basePos + Vector3.right * lateralDir * lateralMoveDistance;
			targetLateralPos.y = basePos.y; // 高さは基準位置と同じにする
			
			// 横方向に移動
			transform.position = Vector3.MoveTowards(transform.position, targetLateralPos, lateralMoveSpeed * Time.deltaTime);

			// 横移動中は常に左右方向を向く
			float lateralAngle = lateralDir == 1 ? 45f : -45f;
			transform.rotation = Quaternion.Euler(0, m_fishingStartRot.eulerAngles.y + lateralAngle, 0);

			// 横移動完了判定
			if (elapsedTime > nextMoveTime)
			{
				elapsedTime = 0f; // 時間をリセット
				isMovingLaterally = false;
			}
		}
	}

	public void IsHit()
	{
		isHit = true; // 魚がかかった状態にする
	}

	// 魚がかかった時の初期設定
	public void SetFishData(FishDataEntity fishData)
	{
		//==========================================//
		// ルアーの位置に合わせて魚の種類を設定予定 //
		//==========================================//

		m_fishData = fishData;
		transform.localScale = Size * m_fishData.fishSize; // 魚のサイズを設定
		isfishing = true; // 釣り状態にする

		Vector3 escapeDir = (lure.position - player.position);
		escapeDir.y = 0; // 高さを無視して水平移動にする
		m_fishingStartRot = Quaternion.LookRotation(escapeDir);
	}

	public void FishingEnd()
	{
		transform.position = HitOffset;
		isfishing = false; // 釣り状態を解除
		isHit = false; // 魚がかかった状態を解除
	}

	public float GetDir()
	{
		// 左右のどちらか
		if (isMovingLaterally)
		{
			// 1: 右, -1: 左
			return lateralDir;
		}

		return 0;
	}
}
