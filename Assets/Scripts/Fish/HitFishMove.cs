using UnityEngine;

public class HitFishMove : MonoBehaviour
{

	[SerializeField] private Transform m_lure;					// ルアー（釣り針）のTransform
	[SerializeField] private Transform m_player;				// プレイヤーのTransform
	[SerializeField] private float m_lateralMoveDistance = 2.0f;// 左右に動く最大距離
	[SerializeField] private float m_lateralMoveSpeed = 4.0f;   // 左右に動くスピード
	private FishDataEntity m_fishData; // 魚のデータ
	
	private readonly Vector3 Size = new Vector3(0.5f, 0.5f, 0.5f); // 魚のサイズ
	private readonly Vector3 HitOffset = new Vector3(0, -3f, 0); // 魚がかかったときの位置オフセット

	private Quaternion m_fishingStartRot; // 釣り開始時の向き
	private Vector3 m_targetLateralPos;
	private float m_elapsedTime = 0f;
	private int m_lateralDir = 1; // -1: 左, 1: 右
	private bool m_isMovingLaterally = false;
	private bool m_isfishing; // 浮きが着水したかどうか
	private bool m_isHit; // 魚がかかったかどうか

	void Start()
	{
		m_isfishing = false; // 初期状態ではヒットしていない
		m_isHit = false;
	}


	void Update()
	{
		if (SelectItem.GetItemType() != SelectItem.ItemType.FishingRod) return; // 釣り竿を選択していない場合は何もしない
		if (PlayerController.IsPause()) return; // カーソルが表示されている場合は何もしない(Pause)

		if (!m_isfishing)
		{
            m_isMovingLaterally = false; // 釣り状態でない場合は横移動を無効にする
            m_elapsedTime = 0f; // 時間をリセット
			return; // 魚がかかっていない場合は何もしない
		}
		// ルアーの下を基準位置とする
		Vector3 basePos = m_lure.position + HitOffset + m_player.forward * 5;

		Vector3 escapeDir = (m_lure.position - m_player.position);
		escapeDir.y = 0; // 高さを無視して水平移動にする
		m_fishingStartRot = Quaternion.LookRotation(escapeDir);

		if (!m_isMovingLaterally)
		{
			// プレイヤーから逃げる方向を向く
			transform.rotation = m_fishingStartRot;

			// ルアーの下に位置する
			transform.position = Vector3.Lerp(transform.position, basePos, 0.01f);
			transform.position = new Vector3(transform.position.x, basePos.y, transform.position.z); // 高さは基準位置と同じにする

			if (!m_isHit) return; // 魚がかかっていない場合は何もしない
            m_elapsedTime += Time.deltaTime;
			// 一定間隔で左右移動開始
			if (m_elapsedTime > m_fishData.moveInterval)
			{
                m_isMovingLaterally = true;
                m_elapsedTime = 0f; // 時間をリセット

                // 左右どちらか選ぶ
                m_lateralDir = Random.value > 0.5f ? 1 : -1;

				// 横移動の方向に向く（0度:上、45度:右、-45度:左）
				float lateralAngle = m_lateralDir == 1 ? 45f : -45f;
				transform.rotation = Quaternion.Euler(0, m_fishingStartRot.eulerAngles.y + lateralAngle, 0);
			}
		}
		else
		{
            // 横移動中の処理
            m_elapsedTime += Time.deltaTime;

            m_targetLateralPos = basePos + Vector3.right * m_lateralDir * m_lateralMoveDistance;
            m_targetLateralPos.y = basePos.y; // 高さは基準位置と同じにする
			
			// 横方向に移動
			transform.position = Vector3.MoveTowards(transform.position, m_targetLateralPos, m_lateralMoveSpeed * Time.deltaTime);

			// 横移動中は常に左右方向を向く
			float lateralAngle = m_lateralDir == 1 ? 45f : -45f;
			transform.rotation = Quaternion.Euler(0, m_fishingStartRot.eulerAngles.y + lateralAngle, 0);

			// 横移動完了判定
			if (m_elapsedTime > m_fishData.nextMoveTime)
			{
                m_elapsedTime = 0f; // 時間をリセット
                m_isMovingLaterally = false;
			}
		}
	}

	public void IsHit()
	{
        m_isHit = true; // 魚がかかった状態にする
	}

	// 魚がかかった時の初期設定
	public void SetFishData(FishDataEntity fishData)
	{
		//==========================================//
		// ルアーの位置に合わせて魚の種類を設定予定 //
		//==========================================//

		m_fishData = fishData;
		transform.localScale = Size * m_fishData.fishSize; // 魚のサイズを設定
        m_isfishing = true; // 釣り状態にする
	}

	public void FishingEnd()
	{
		transform.position = HitOffset;
        m_isfishing = false; // 釣り状態を解除
        m_isHit = false; // 魚がかかった状態を解除
	}

	public float GetDir()
	{
		// 左右のどちらか
		if (m_isMovingLaterally)
		{
			// 1: 右, -1: 左
			return m_lateralDir;
		}

		return 0;
	}

	public void SetStartPos()
	{
		// 釣り開始時このコードを通る時は魚の位置をルアーの位置から少し前方に設定する
		transform.position = m_lure.position + HitOffset + m_player.forward * 5;
		m_fishingStartRot = Quaternion.LookRotation(m_lure.position - m_player.position);
		transform.rotation = m_fishingStartRot; // プレイヤーから逃げる方向を向く
        m_isMovingLaterally = false; // 横移動を無効にする
        m_elapsedTime = 0f; // 時間をリセット
	}
}
