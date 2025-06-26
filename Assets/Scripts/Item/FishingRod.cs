using UnityEngine;

public class FishingRod : MonoBehaviour
{
	[SerializeField] private GameObject m_player;
	[SerializeField] private ExcelData m_excelData;
	[SerializeField] private HitFishMove m_hitFish; // かかった魚の動きのスクリプト
	[SerializeField] private float m_fishEndDistance = 3; // プレイヤーとの距離がこれ以上近づいたら釣りを終了する距離
	[SerializeField] private float m_fishStartDistance = 15; // 釣り開始時は必ずこれ以上の距離から始まるようにする
	[SerializeField] private float m_fishStartMoveSpeed = 7; // 釣り開始時にプレイヤーから離れる速度
	private Fishing m_fishing;
	private FishDataEntity m_fishData;
	private Rigidbody m_rigidbody;
	
	private const float NotHitHeight = -1.0f;	// 魚にかかる前の浮きの高さ 
	private const float HitHeight = -1.2f;      // 魚にかかった後の浮きの高さ
	private const float FishMaxDistance = 75;   // いい魚が釣れる比較距離の最大値
    private const int MaxFishData = 100; // 魚の最大数

    // ポーズ中に保存するための変数
    private Vector3 m_savedVelocity;
    private Vector3 m_savedAngularVelocity;
	private float m_hitTime;		// 魚ヒットするまでの時間
	private float m_elapsedTime;
	private bool m_isPaused;        // ゲームがポーズ中かどうか
	private bool m_throw;			// 浮きを投げたているかどうか
	private bool m_isFishing;       // 釣り中かどうか
	private bool m_isHit;          // 魚にヒットしたかどうか
	private bool m_setDistance;		// プレイヤーとの距離を調整するためのフラグ

	void Start()
	{
		m_fishing = m_player.GetComponent<Fishing>();
		m_rigidbody = GetComponent<Rigidbody>();
		m_isPaused = false; // 初期状態ではポーズ中ではない
		m_throw = false;
		m_isFishing = false;
		m_isHit = false; // 初期状態ではヒットしていない
		m_setDistance = false;
		m_elapsedTime = 0f;
		gameObject.SetActive(false); // 初期状態では浮きを非表示にする
	}

	void Update()
	{
		if (SelectItem.GetItemType() != SelectItem.ItemType.FishingRod) return; // 釣り竿を選択していない場合は何もしない
		if (PlayerController.IsPause())
		{
			// ポーズ画面に入る時に角度と速度を保存
			if (!m_isPaused)
			{
                m_savedVelocity = m_rigidbody.velocity;
                m_savedAngularVelocity = m_rigidbody.angularVelocity;
			}
			m_rigidbody.isKinematic = true;
			m_isPaused = true; // ポーズ中にカーソルが表示されたらポーズ状態にする
			return;
		}
		else if (m_isPaused)
		{
			m_rigidbody.isKinematic = false;
			m_rigidbody.velocity = m_savedVelocity;
			m_rigidbody.angularVelocity = m_savedAngularVelocity;
			m_isPaused = false;
		}
		else
		{
			m_rigidbody.isKinematic = false; // カーソルが非表示のときは物理演算を有効化
		}

		// 水面に浮きが当たっているとき
		if (m_fishData != null)
		{
			Vector2 floatPos = new(transform.position.x, transform.position.z);
			Vector2 playerPos = new(m_player.transform.position.x, m_player.transform.position.z);
			
			// ヒットしたら高さを下げる
			if (!m_isHit && m_elapsedTime >= (float)m_hitTime)
			{
				// かかった時にプレイヤーとの距離が近すぎる場合にプレイヤーから離すフラグを立てる
				if (Vector2.Distance(floatPos, playerPos) < m_fishStartDistance)
				{
					m_setDistance = true;
				}

				m_isHit = true;
				m_fishing.IsHit();
                m_hitFish.IsHit();
			}
			float RodHeight = m_isHit ? HitHeight : NotHitHeight;

			transform.position = new Vector3(transform.position.x, RodHeight, transform.position.z);
			// 釣り中の処理
			m_elapsedTime += Time.deltaTime;

			// 釣り開始時にプレイヤーとの距離を調整する
			if (m_setDistance)
			{
				// 一定距離話すまで繰り返す
				if (Vector2.Distance(floatPos, playerPos) < m_fishStartDistance)
				{
					transform.Translate(m_hitFish.transform.forward * Time.deltaTime * m_fishStartMoveSpeed); // プレイヤーから離れるように浮きを動かす
                    m_hitFish.SetStartPos();
					return; // プレイヤーとの距離を調整している間は他の処理を行わない
				}
				else
				{
					m_setDistance = false; // 一定距離話したらフラグをリセット
				}
			}

			// プレイヤーが一定距離にいる時は釣りを終了する
			if (m_isHit && Vector2.Distance(floatPos, playerPos) < m_fishEndDistance)
			{
				FishingEnd(m_isHit);
			}
		}

		// 海を貫通してしまったときの例外処理
		if (transform.position.y < -10f) FishingEnd(false);
	}

	public void FishingStart(Vector3 forward)
	{
		if (m_throw) return;
		if (forward.y < 0.5f) forward = new Vector3(forward.x, 0.5f, forward.z);
        m_rigidbody.AddForce(forward * 10f, ForceMode.Impulse);
		m_throw = true;
	}

	public void OnCollisionEnter(Collision other)
	{
		// 浮きが水面に当たったら
		if (m_throw && other.transform.CompareTag("Sea"))
		{
			// 動きを止めて、魚のデータを取得
			m_throw = false;
			m_rigidbody.isKinematic = true;
			m_rigidbody.useGravity = false; // 重力を無効化
			m_isFishing = true;
			m_fishData = GetFishData();
            m_hitFish.SetFishData(m_fishData); // 魚のデータをセット
			SetHitTime();
		}

		if (other.transform.CompareTag("Player"))
		{
			FishingEnd(m_isHit); // プレイヤーに当たったら釣りを終了
		}
	}

	private FishDataEntity GetFishData()
	{
		// 釣り竿の距離が遠いほど良い魚がかかる可能性が上がるようにする

		// 一番離れている方向を取得
		float maxDir = Mathf.Max(Mathf.Abs(transform.position.x), Mathf.Abs(transform.position.z));
		if (maxDir >= FishMaxDistance) maxDir = FishMaxDistance;

		int fishMaxNum = MaxFishData / (int)(FishMaxDistance / maxDir);
		if (fishMaxNum > MaxFishData) fishMaxNum = MaxFishData;

		// ハンマーを足した分の最大数
		int randomIndex = Random.Range(0, fishMaxNum + 1);
		// 今出せる最大数の値ならハンマーに変換する
		if (randomIndex == fishMaxNum) randomIndex = MaxFishData;
		return m_excelData.fish[randomIndex]; ;
	}

	private void SetHitTime()
	{
		int seed = (int)Random.Range(5, 25);
		seed = 3;
		// いづれseedに合わせてテーブルを作る
		m_hitTime = seed;
	}

	public void FishingEnd(bool isSuccess)
    {
		m_fishing.FishingEnd(isSuccess, m_fishData);
        m_hitFish.FishingEnd();
		m_isPaused = false; // ポーズ状態をリセット
		m_throw = false;	// 投げたフラグをリセット
        m_isFishing = false; // 釣り中フラグをリセット
        m_fishData = null; // 魚のデータをリセット
		m_isHit = false; // ヒットフラグをリセット
		m_elapsedTime = 0f; // 経過時間をリセット
        m_rigidbody.isKinematic = false; // 浮きを動かせるように戻す
		m_rigidbody.useGravity = true; // 重力を有効化
		gameObject.SetActive(false); // 浮きを消す
		m_rigidbody.velocity = Vector3.zero;	// AddForceを0に戻す
    }

	public bool IsFishing()
	{
		return m_isFishing;
	}

	public bool CanThrow()
	{
		return m_isFishing || m_throw;
	}

	public bool IsHit()
	{
		return m_isHit;
	}

	public int GetFishSize()
	{
		if (m_fishData != null)
		{
			return m_fishData.fishSize - 1;
		}
		return 0; // 魚がかかっていない場合は0を返す
	}
}
