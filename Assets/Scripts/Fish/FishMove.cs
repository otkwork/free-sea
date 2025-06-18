using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FishMove : MonoBehaviour
{
    [SerializeField] FishingRod rodFloat;
    const float Speed = 2f;

    Vector3 m_startPos;
	Quaternion m_startRot;
	Vector3 m_rotation;
    float m_changeRotation;
    float m_elapsedTime;
    bool m_flipFish;
	bool m_isReturnFish; // ‹›‚ª“¦‚°‚Ä‚¢‚é‚©‚Ç‚¤‚©

	void Start()
    {
		m_startPos = transform.position;
		m_startRot = transform.rotation;
		m_changeRotation = Random.Range(2f, 5f);
        m_elapsedTime = 0;
        m_flipFish = false;
		m_isReturnFish = false; // ‰Šúó‘Ô‚Å‚Í‹›‚Í“¦‚°‚Ä‚¢‚È‚¢
	}

    // Update is called once per frame
    void Update()
    {
		if (PlayerController.IsPause()) return; // ƒJ[ƒ\ƒ‹‚ª•\Ž¦‚³‚ê‚Ä‚¢‚éê‡‚Í‰½‚à‚µ‚È‚¢(Pause)

		m_elapsedTime += Time.deltaTime;
        // Œü‚¢‚Ä‚¢‚é•ûŒü‚Éi‚Þ
        transform.position += transform.forward * Speed * Time.deltaTime;
        // ‹›‚ª‚©‚©‚Á‚½‚ç‚»‚êˆÈŠO‚Ì‹›‚ð“¦‚ª‚·
        if (rodFloat.IsHit() && !m_isReturnFish)
        {
			m_isReturnFish = true; // ‹›‚ª“¦‚°‚Ä‚¢‚éó‘Ô‚É‚·‚é
			// ƒ^[ƒQƒbƒg‚ÌˆÊ’u‚ÆŽ©•ª‚ÌˆÊ’u‚ðŠî‚É”½‘Î•ûŒü‚ðŒvŽZ
			Vector3 dir = rodFloat.transform.position - transform.position;
			Vector3 oppositionDir = -dir;
			oppositionDir = new Vector3(oppositionDir.x, 0, oppositionDir.z).normalized; // …•½•ûŒü‚ÉŒÀ’è

			// ”½‘Î•ûŒü‚ÉŒü‚¯‚é
			transform.rotation = Quaternion.LookRotation(oppositionDir);
            return;
        }

        // ’Þ‚è’†
        if (m_isReturnFish)
        {
            if (!rodFloat.IsFishing())
            {
                m_isReturnFish = false;
                transform.position = m_startPos;
                transform.rotation = m_startRot;
            }
            return;
        }

        // ‰ñ“]
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
            // ”¼‰ñ“]
            transform.rotation = Quaternion.Euler(Vector3.Lerp
                (
                m_rotation,
                m_rotation - new Vector3(0, 180, 0),
                m_elapsedTime
                ));

            // ‰ñ“]‚µ‚«‚Á‚½‚ç‰ñ“]‚ð‚â‚ß‚é
            if (m_elapsedTime > 1)
            {
                m_flipFish = false;
                m_elapsedTime = 0;
            }
        }
    }
}
