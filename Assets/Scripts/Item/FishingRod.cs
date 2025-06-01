using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FishingRod : MonoBehaviour
{
	[SerializeField] GameObject player;
	[SerializeField] ExcelData excelData;
	
	Fishing m_fishing;
	FishDataEntity m_fishData;
	Rigidbody m_rigidbody;

	const float NotHitHeight = -1.0f;	// 魚にかかる前の浮きの高さ 
	const float HitHeight = -1.2f;		// 魚にかかった後の浮きの高さ
	const float ResetArea = 3f;			// 釣りをリセットするプレイヤーの範囲

	bool m_throw;			// 浮きを投げたているかどうか
	bool m_isFishing;       // 釣り中かどうか
	bool m_isHit;          // 魚にヒットしたかどうか
	bool m_canFishing;		// 釣りができる海域かどうか
	float m_hitTime;		// 魚ヒットするまでの時間
	float m_elapsedTime;

	void Start()
	{
		m_fishing = player.GetComponent<Fishing>();
		m_rigidbody = GetComponent<Rigidbody>();
		m_throw = false;
		m_isFishing = false;
		m_isHit = false; // 初期状態ではヒットしていない
		m_elapsedTime = 0f;
		gameObject.SetActive(false); // 初期状態では浮きを非表示にする
	}

	void Update()
	{
		// 水面に浮きが当たっているとき
		if (m_fishData != null)
		{
			// ヒットしたら高さを下げる
			if (!m_isHit && m_elapsedTime >= (float)m_hitTime)
			{
				m_isHit = true;
				m_fishing.IsHit();
			}
			float RodHeight = m_isHit ? HitHeight : NotHitHeight;

			transform.position = new Vector3(transform.position.x, RodHeight, transform.position.z);
            // 釣り中の処理
            m_elapsedTime += Time.deltaTime;

			// 内部の処理がまだないので、時間経過で釣りを終了する
			if (m_elapsedTime >= (float)m_hitTime + 15)
			{
				//--------------------//
				//釣りの内部を制作予定//
				//--------------------//

				// 釣り終了
				FishingEnd(true);
			}
        }

		// プレイヤーの半径5かつプレイヤーよりも低ければリセットして消える

        if (Mathf.Pow(transform.position.x - player.transform.position.x, 2) + 
			Mathf.Pow(transform.position.z - player.transform.position.z, 2) < ResetArea &&
			transform.position.y < player.transform.position.y)
		{
			FishingEnd(m_isHit);
		}

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

			// XZ両方の距離が10よりも近い場合何も釣れない
			if (transform.position.x > 10 || transform.position.z > 10)
			{
				m_isFishing = true;
				m_fishData = excelData.fish[(int)Random.Range(0, excelData.fish.Count)];
				SetHitTime();
			}
		}

		if (other.transform.CompareTag("Player"))
		{
			FishingEnd(false); // プレイヤーに当たったら釣りを終了
		}
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
		m_throw = false;	// 投げたフラグをリセット
        m_isFishing = false; // 釣り中フラグをリセット
        m_fishData = null; // 魚のデータをリセット
		m_isHit = false; // ヒットフラグをリセット
		m_elapsedTime = 0f; // 経過時間をリセット
        m_rigidbody.isKinematic = false; // 浮きを動かせるように戻す
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
}
