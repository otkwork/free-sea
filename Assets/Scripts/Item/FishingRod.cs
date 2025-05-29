using UnityEngine;

public class FishingRod : MonoBehaviour
{
	[SerializeField] Fishing m_fishing;
	[SerializeField] ExcelData excelData;
	FishDataEntity m_fishData;
	Rigidbody m_rigidbody;

	bool m_throw;			// 浮きを投げたているかどうか
	bool m_isFishing;		// 釣り中かどうか
	float m_elapsedTime;

	void Start()
	{
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
			// 釣り中の処理
			m_elapsedTime += Time.deltaTime;
			if (m_elapsedTime >= (float)m_fishData.hp)
			{
				// 釣り終了
				m_fishing.FishingEnd(m_fishData);
				m_fishData = null; // 魚のデータをリセット
				m_isFishing = false; // 釣り中フラグをリセット
				m_rigidbody.isKinematic = false; // 浮きを動かせるように戻す
				m_elapsedTime = 0f; // 経過時間をリセット
				gameObject.SetActive(false); // 浮きを消す
			}
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
		}
	}

	public bool IsFishing()
	{
		return m_isFishing;
	}
}
