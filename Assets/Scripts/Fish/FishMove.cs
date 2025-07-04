using UnityEngine;

public class FishMove : MonoBehaviour
{
    [SerializeField] private FishingRod m_rodFloat;
    private const float Speed = 2f;

    private Vector3 m_startPos;
	private Quaternion m_startRot;
	private Vector3 m_rotation;
    private float m_changeRotation;
    private float m_elapsedTime;
    private bool m_flipFish;
    private bool m_isReturnFish; // 魚が逃げているかどうか

	void Start()
    {
		m_startPos = transform.position;
		m_startRot = transform.rotation;
		m_changeRotation = Random.Range(2f, 5f);
        m_elapsedTime = 0;
        m_flipFish = false;
		m_isReturnFish = false; // 初期状態では魚は逃げていない
	}

    // Update is called once per frame
    void Update()
    {
		if (PlayerController.IsPause()) return; // カーソルが表示されている場合は何もしない(Pause)

		m_elapsedTime += Time.deltaTime;
        // 向いている方向に進む
        transform.position += transform.forward * Speed * Time.deltaTime;
        // 魚がかかったらそれ以外の魚を逃がす
        if (m_rodFloat.IsHit() && !m_isReturnFish)
        {
			m_isReturnFish = true; // 魚が逃げている状態にする
			// ターゲットの位置と自分の位置を基に反対方向を計算
			Vector3 dir = m_rodFloat.transform.position - transform.position;
			Vector3 oppositionDir = -dir;
			oppositionDir = new Vector3(oppositionDir.x, 0, oppositionDir.z).normalized; // 水平方向に限定

			// 反対方向に向ける
			transform.rotation = Quaternion.LookRotation(oppositionDir);
            return;
        }

        // 釣り中
        if (m_isReturnFish)
        {
            if (!FishingRod.IsFishing())
            {
                m_isReturnFish = false;
                transform.position = m_startPos;
                transform.rotation = m_startRot;
            }
            return;
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
