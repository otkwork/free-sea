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
	bool m_isReturnFish; // ���������Ă��邩�ǂ���

	void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
		m_meshRenderer = GetComponent<MeshRenderer>();
		m_startPos = transform.position;
		m_startRot = transform.rotation;
		m_changeRotation = Random.Range(1f, 4f);
        m_elapsedTime = 0;
        m_flipFish = false;
		m_isReturnFish = false; // ������Ԃł͋��͓����Ă��Ȃ�
	}

    // Update is called once per frame
    void Update()
    {
		m_rigidbody.isKinematic = Cursor.visible; ; // �J�[�\�����\������Ă���ꍇ�͕������Z�𖳌���
		if (Cursor.visible) return; // �J�[�\�����\������Ă���ꍇ�͉������Ȃ�(Pause)

		m_elapsedTime += Time.deltaTime;
        // �����Ă�������ɐi��
        m_rigidbody.velocity = transform.forward * Speed;
        // �������������炻��ȊO�̋��𓦂���
        if (rodFloat.IsHit() && !m_isReturnFish)
        {
			m_isReturnFish = true; // ���������Ă����Ԃɂ���
			// �^�[�Q�b�g�̈ʒu�Ǝ����̈ʒu����ɔ��Ε������v�Z
			Vector3 dir = rodFloat.transform.position - transform.position;
			Vector3 oppositionDir = -dir;

			// ���Ε����Ɍ�����
			transform.rotation = Quaternion.LookRotation(oppositionDir);
            return;
        }

		// �������������\���ɂ���
		if (m_isReturnFish)
		{
			if (m_elapsedTime > 5f)
			{
				m_meshRenderer.enabled = false; // �����\���ɂ���
				m_isReturnFish = false; // ������������͍ēx�������Ԃ����Z�b�g
			}
			return;
		}
		// ���������Ĕ�\���ɂȂ��Ă���Ƃ��ɒނ肪�I�������ĕ\������
		else if (!m_meshRenderer.enabled)
		{
			m_meshRenderer.enabled = true; // ����\������
			transform.position = m_startPos; // ���̈ʒu�������ʒu�ɖ߂�
			transform.rotation = m_startRot; // ���̉�]��������]�ɖ߂�
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
