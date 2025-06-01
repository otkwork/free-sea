using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FishMove : MonoBehaviour
{
    [SerializeField] FishingRod rodFloat;
    Transform m_startTransform;
    const float Speed = 2f;
    Rigidbody m_rigidbody;

    Vector3 m_rotation;
    float m_changeRotation;
    float m_elapsedTime;
    bool m_flipFish;
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_startTransform = transform;
        m_changeRotation = Random.Range(1f, 3f);
        m_elapsedTime = 0;
        m_flipFish = false;
    }

    // Update is called once per frame
    void Update()
    {
        m_elapsedTime += Time.deltaTime;
        // �����Ă�������ɐi��
        m_rigidbody.velocity = -transform.up.normalized * Speed;
        
        // �������������瓦����
        if (rodFloat.IsHit())
        {
            // �^�[�Q�b�g�̈ʒu�Ǝ����̈ʒu����ɔ��Ε������v�Z
            Vector3 directionToTarget = transform.position - rodFloat.transform.position;
            Vector3 oppositeDirection = -directionToTarget; // ���Ε���

            // ���Ε����Ɍ�����
            transform.rotation = Quaternion.LookRotation(oppositeDirection);
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
                m_rotation - new Vector3(0, 0, 180),
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
