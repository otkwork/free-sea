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

	const float NotHitHeight = -1.0f;
	const float HitHeight = -1.2f;

	bool m_throw;			// �����𓊂����Ă��邩�ǂ���
	bool m_isFishing;       // �ނ蒆���ǂ���
	bool m_isHit;          // ���Ƀq�b�g�������ǂ���
	float m_hitTime;		// ���q�b�g����܂ł̎���
	float m_elapsedTime;

	void Start()
	{
		m_fishing = player.GetComponent<Fishing>();
		m_rigidbody = GetComponent<Rigidbody>();
		m_throw = false;
		m_isFishing = false;
		m_isHit = false; // ������Ԃł̓q�b�g���Ă��Ȃ�
		m_elapsedTime = 0f;
		gameObject.SetActive(false); // ������Ԃł͕������\���ɂ���
	}

	void Update()
	{
		// ���ʂɕ������������Ă���Ƃ�
		if (m_fishData != null)
		{
			// �q�b�g�����獂����������
			m_isHit = m_elapsedTime >= (float)m_hitTime;
			float RodHeight = m_isHit ? HitHeight : NotHitHeight;

			transform.position = new Vector3(transform.position.x, RodHeight, transform.position.z);
            // �ނ蒆�̏���
            m_elapsedTime += Time.deltaTime;

			// �����̏������܂��Ȃ��̂ŁA���Ԍo�߂Œނ���I������
			if (m_elapsedTime >= (float)m_hitTime + 5)
			{
				//--------------------//
				//�ނ�̓����𐧍�\��//
				//--------------------//

				// �ނ�I��
				FishingEnd(true);
			}
        }

		// �ނ蒆����Ȃ��Ȃ�ʂ�Ȃ�
		if (!m_isFishing) return;
		// �v���C���[�̔��a5�ɋ߂Â����烊�Z�b�g���ď�����

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
		// ���������ʂɓ���������
		if (m_throw && other.transform.CompareTag("Sea"))
		{
			// �������~�߂āA���̃f�[�^���擾
			m_throw = false;
			m_isFishing = true;
			m_rigidbody.isKinematic = true;
			m_fishData = excelData.fish[(int)Random.Range(0, excelData.fish.Count)];
			SetHitTime();
		}

		if (other.transform.CompareTag("Player"))
		{
			m_throw = false;
			FishingEnd(false); // �v���C���[�ɓ���������ނ���I��
		}
	}

	private void SetHitTime()
	{
		int seed = (int)Random.Range(5, 25);
		// ���Â�seed�ɍ��킹�ăe�[�u�������
		m_hitTime = seed;
	}

	private void FishingEnd(bool isSuccess)
    {
		m_fishing.FishingEnd(isSuccess, m_fishData);
        m_isFishing = false; // �ނ蒆�t���O�����Z�b�g
        m_fishData = null; // ���̃f�[�^�����Z�b�g
		m_isHit = false; // �q�b�g�t���O�����Z�b�g
		m_elapsedTime = 0f; // �o�ߎ��Ԃ����Z�b�g
        m_rigidbody.isKinematic = false; // �����𓮂�����悤�ɖ߂�
        gameObject.SetActive(false); // ����������
    }

	public bool IsFishing()
	{
		return m_isFishing;
	}

	public bool IsHit()
	{
		return m_isHit;
	}
}
