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
	bool m_isReturnFish; // ���������Ă��邩�ǂ���

	void Start()
    {
		m_startPos = transform.position;
		m_startRot = transform.rotation;
		m_changeRotation = Random.Range(2f, 5f);
        m_elapsedTime = 0;
        m_flipFish = false;
		m_isReturnFish = false; // ������Ԃł͋��͓����Ă��Ȃ�
	}

    // Update is called once per frame
    void Update()
    {
		if (PlayerController.IsPause()) return; // �J�[�\�����\������Ă���ꍇ�͉������Ȃ�(Pause)

		m_elapsedTime += Time.deltaTime;
        // �����Ă�������ɐi��
        transform.position += transform.forward * Speed * Time.deltaTime;
        // �������������炻��ȊO�̋��𓦂���
        if (rodFloat.IsHit() && !m_isReturnFish)
        {
			m_isReturnFish = true; // ���������Ă����Ԃɂ���
			// �^�[�Q�b�g�̈ʒu�Ǝ����̈ʒu����ɔ��Ε������v�Z
			Vector3 dir = rodFloat.transform.position - transform.position;
			Vector3 oppositionDir = -dir;
			oppositionDir = new Vector3(oppositionDir.x, 0, oppositionDir.z).normalized; // ���������Ɍ���

			// ���Ε����Ɍ�����
			transform.rotation = Quaternion.LookRotation(oppositionDir);
            return;
        }

        // �ނ蒆
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

        // ��]
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
            // ����]
            transform.rotation = Quaternion.Euler(Vector3.Lerp
                (
                m_rotation,
                m_rotation - new Vector3(0, 180, 0),
                m_elapsedTime
                ));

            // ��]�����������]����߂�
            if (m_elapsedTime > 1)
            {
                m_flipFish = false;
                m_elapsedTime = 0;
            }
        }
    }
}
