using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FishMove : MonoBehaviour
{
    [SerializeField] FishingRod rodFloat;
    const float Speed = 2f;
    Rigidbody m_rigidbody;
	MeshRenderer m_meshRenderer;

    Vector3 m_startPos;
	Quaternion m_startRot;
	Vector3 m_rotation;
    float m_changeRotation;
    float m_elapsedTime;
    bool m_flipFish;
	bool m_isReturnFish; // 魚が逃げているかどうか

	void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
		m_meshRenderer = GetComponent<MeshRenderer>();
		m_startPos = transform.position;
		m_startRot = transform.rotation;
		m_changeRotation = Random.Range(1f, 4f);
        m_elapsedTime = 0;
        m_flipFish = false;
		m_isReturnFish = false; // 初期状態では魚は逃げていない
	}

    // Update is called once per frame
    void Update()
    {
		m_rigidbody.isKinematic = Cursor.visible; ; // カーソルが表示されている場合は物理演算を無効化
		if (Cursor.visible) return; // カーソルが表示されている場合は何もしない(Pause)

		m_elapsedTime += Time.deltaTime;
        // 向いている方向に進む
        m_rigidbody.velocity = transform.forward * Speed;
        // 魚がかかったらそれ以外の魚を逃がす
        if (rodFloat.IsHit() && !m_isReturnFish)
        {
			m_isReturnFish = true; // 魚が逃げている状態にする
			// ターゲットの位置と自分の位置を基に反対方向を計算
			Vector3 dir = rodFloat.transform.position - transform.position;
			Vector3 oppositionDir = -dir;

			// 反対方向に向ける
			transform.rotation = Quaternion.LookRotation(oppositionDir);
            return;
        }

		// 魚が逃げた後非表示にする
		if (m_isReturnFish)
		{
			if (m_elapsedTime > 5f)
			{
				m_meshRenderer.enabled = false; // 魚を非表示にする
				m_isReturnFish = false; // 魚が逃げた後は再度逃げる状態をリセット
			}
			return;
		}
		// 魚が逃げて非表示になっているときに釣りが終わったら再表示する
		else if (!m_meshRenderer.enabled)
		{
			m_meshRenderer.enabled = true; // 魚を表示する
			transform.position = m_startPos; // 魚の位置を初期位置に戻す
			transform.rotation = m_startRot; // 魚の回転を初期回転に戻す
		}

		// 回転
		if (!m_flipFish && m_elapsedTime > m_changeRotation)
		{
			m_flipFish = true;
			m_rotation = transform.rotation.eulerAngles;
			m_elapsedTime = 0;
		}
        ChangeRotation();
    }

    private void ChangeRotation()
    {
        if (m_flipFish)
        {
            // 半回転
            transform.rotation = Quaternion.Euler(Vector3.Lerp
                (
                m_rotation,
                m_rotation - new Vector3(0, 180, 0),
                m_elapsedTime
                ));

            // 回転しきったら回転をやめる
            if (m_elapsedTime > 1)
            {
                m_flipFish = false;
                m_elapsedTime = 0;
            }
        }
    }
}
