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
        // Œü‚¢‚Ä‚¢‚é•ûŒü‚Éi‚Þ
        m_rigidbody.velocity = -transform.up.normalized * Speed;
        
        // ‹›‚ª‚©‚©‚Á‚½‚ç“¦‚ª‚·
        if (rodFloat.IsHit())
        {
            // ƒ^[ƒQƒbƒg‚ÌˆÊ’u‚ÆŽ©•ª‚ÌˆÊ’u‚ðŠî‚É”½‘Î•ûŒü‚ðŒvŽZ
            Vector3 directionToTarget = transform.position - rodFloat.transform.position;
            Vector3 oppositeDirection = -directionToTarget; // ”½‘Î•ûŒü

            // ”½‘Î•ûŒü‚ÉŒü‚¯‚é
            transform.rotation = Quaternion.LookRotation(oppositeDirection);
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
                m_rotation - new Vector3(0, 0, 180),
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
