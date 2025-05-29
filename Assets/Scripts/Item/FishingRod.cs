using UnityEngine;

public class FishingRod : MonoBehaviour
{
	[SerializeField] Fishing m_fishing;
	[SerializeField] ExcelData excelData;
	FishDataEntity m_fishData;
	Rigidbody m_rigidbody;

	bool m_throw;			// �����𓊂����Ă��邩�ǂ���
	bool m_isFishing;		// �ނ蒆���ǂ���
	float m_elapsedTime;

	void Start()
	{
		m_rigidbody = GetComponent<Rigidbody>();
		m_throw = false;
		m_isFishing = false;
		m_elapsedTime = 0f;
		gameObject.SetActive(false); // ������Ԃł͕������\���ɂ���
	}

	void Update()
	{
		// ���ʂɕ������������Ă���Ƃ�
		if (m_fishData != null)
		{
			// �ނ蒆�̏���
			m_elapsedTime += Time.deltaTime;
			if (m_elapsedTime >= (float)m_fishData.hp)
			{
				// �ނ�I��
				m_fishing.FishingEnd(m_fishData);
				m_fishData = null; // ���̃f�[�^�����Z�b�g
				m_isFishing = false; // �ނ蒆�t���O�����Z�b�g
				m_rigidbody.isKinematic = false; // �����𓮂�����悤�ɖ߂�
				m_elapsedTime = 0f; // �o�ߎ��Ԃ����Z�b�g
				gameObject.SetActive(false); // ����������
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
		// ���������ʂɓ���������
		if (m_throw && other.transform.CompareTag("Sea"))
		{
			// �������~�߂āA���̃f�[�^���擾
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
