using Unity.VisualScripting;
using UnityEngine;

public class FishingRod : MonoBehaviour
{
	[SerializeField] GameObject player;
	[SerializeField] ExcelData excelData;
	
	Fishing m_fishing;
	FishDataEntity m_fishData;
	Rigidbody m_rigidbody;

	const float NotHitHeight = -1.0f;
	const float HitHeight = -1.2f;

	bool m_throw;			// 浮きを投げたているかどうか
	bool m_isFishing;       // 釣り中かどうか
	float m_hitTime;		// 魚ヒットするまでの時間
	float m_elapsedTime;

	void Start()
	{
		m_fishing = player.GetComponent<Fishing>();
		m_rigidbody = GetComponent<Rigidbody>();
		m_throw = false;
		m_isFishing = false;
		m_elapsedTime = 0f;
		gameObject.SetActive(false); // 初期状態では浮きを非表示にする
	}

	void Update()
	{
		// 水面に浮きが当たっているとき
		if (m_fishData != null)
		{
			// ヒットしたら高さを下げる
			float RodHeight = m_elapsedTime >= (float)m_hitTime - 5 ? HitHeight : NotHitHeight;

            transform.position = new Vector3(transform.position.x, RodHeight, transform.position.z);
            // 釣り中の処理
            m_elapsedTime += Time.deltaTime;
			if (m_elapsedTime >= (float)m_hitTime)
			{
				//--------------------//
				//釣りの内部を制作予定//
				//--------------------//

				// 釣り終了
				FishingEnd(true);
			}
        }

		// 釣り中じゃないなら通らない
		if (!m_isFishing) return;
		// プレイヤーの半径5に近づいたらリセットして消える

        if (Mathf.Pow(transform.position.x - player.transform.position.x, 2) + 
			Mathf.Pow(transform.position.z - player.transform.position.z, 2) < 5)
		{
			FishingEnd(false);
		}
	}

	public void FishingStart(Vector3 forward)
	{
		if (m_throw) return;
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
			m_isFishing = true;
			m_rigidbody.isKinematic = true;
			m_fishData = excelData.fish[(int)Random.Range(0, excelData.fish.Count)];
			SetHitTime();
		}
	}

	private void SetHitTime()
	{
		int seed = (int)Random.Range(5, 25);
		// いづれseedに合わせてテーブルを作る
		m_hitTime = seed;
	}

	private void FishingEnd(bool isSuccess)
    {
		if (isSuccess) m_fishing.FishingEnd(m_fishData);
        m_isFishing = false; // 釣り中フラグをリセット
        m_fishData = null; // 魚のデータをリセット
        m_elapsedTime = 0f; // 経過時間をリセット
        m_rigidbody.isKinematic = false; // 浮きを動かせるように戻す
        gameObject.SetActive(false); // 浮きを消す
    }

	public bool IsFishing()
	{
		return m_isFishing;
	}
}
