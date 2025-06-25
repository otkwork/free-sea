using UnityEngine;
using UnityEngine.UIElements;

public class FishingRod : MonoBehaviour
{
	[SerializeField] GameObject player;
	[SerializeField] ExcelData excelData;
	[SerializeField] HitFishMove hitFish; // �����������̓����̃X�N���v�g
	[SerializeField] float fishEndDistance = 3; // �v���C���[�Ƃ̋���������ȏ�߂Â�����ނ���I�����鋗��
	[SerializeField] float fishStartDistance = 15; // �ނ�J�n���͕K������ȏ�̋�������n�܂�悤�ɂ���
	[SerializeField] float fishStartMoveSpeed = 7; // �ނ�J�n���Ƀv���C���[���痣��鑬�x

	Fishing m_fishing;
	FishDataEntity m_fishData;
	Rigidbody m_rigidbody;
	
	const float NotHitHeight = -1.0f;	// ���ɂ�����O�̕����̍��� 
	const float HitHeight = -1.2f;      // ���ɂ���������̕����̍���
	const float FishMaxDistance = 75;	// ���������ނ���r�����̍ő�l
									  
	// �|�[�Y���ɕۑ����邽�߂̕ϐ�
	private Vector3 savedVelocity;
    private Vector3 savedAngularVelocity;
	bool m_isPaused;        // �Q�[�����|�[�Y�����ǂ���
	bool m_throw;			// �����𓊂����Ă��邩�ǂ���
	bool m_isFishing;       // �ނ蒆���ǂ���
	bool m_isHit;          // ���Ƀq�b�g�������ǂ���
	bool m_setDistance;		// �v���C���[�Ƃ̋����𒲐����邽�߂̃t���O
	float m_hitTime;		// ���q�b�g����܂ł̎���
	float m_elapsedTime;

	void Start()
	{
		m_fishing = player.GetComponent<Fishing>();
		m_rigidbody = GetComponent<Rigidbody>();
		m_isPaused = false; // ������Ԃł̓|�[�Y���ł͂Ȃ�
		m_throw = false;
		m_isFishing = false;
		m_isHit = false; // ������Ԃł̓q�b�g���Ă��Ȃ�
		m_setDistance = false;
		m_elapsedTime = 0f;
		gameObject.SetActive(false); // ������Ԃł͕������\���ɂ���
	}

	void Update()
	{
		if (SelectItem.GetItemType() != SelectItem.ItemType.FishingRod) return; // �ނ�Ƃ�I�����Ă��Ȃ��ꍇ�͉������Ȃ�
		if (PlayerController.IsPause())
		{
			// �|�[�Y��ʂɓ��鎞�Ɋp�x�Ƒ��x��ۑ�
			if (!m_isPaused)
			{
				savedVelocity = m_rigidbody.velocity;
				savedAngularVelocity = m_rigidbody.angularVelocity;
			}
			m_rigidbody.isKinematic = true;
			m_isPaused = true; // �|�[�Y���ɃJ�[�\�����\�����ꂽ��|�[�Y��Ԃɂ���
			return;
		}
		else if (m_isPaused)
		{
			m_rigidbody.isKinematic = false;
			m_rigidbody.velocity = savedVelocity;
			m_rigidbody.angularVelocity = savedAngularVelocity;
			m_isPaused = false;
		}
		else
		{
			m_rigidbody.isKinematic = false; // �J�[�\������\���̂Ƃ��͕������Z��L����
		}

		// ���ʂɕ������������Ă���Ƃ�
		if (m_fishData != null)
		{
			Vector2 floatPos = new(transform.position.x, transform.position.z);
			Vector2 playerPos = new(player.transform.position.x, player.transform.position.z);
			
			// �q�b�g�����獂����������
			if (!m_isHit && m_elapsedTime >= (float)m_hitTime)
			{
				// �����������Ƀv���C���[�Ƃ̋������߂�����ꍇ�Ƀv���C���[���痣���t���O�𗧂Ă�
				if (Vector2.Distance(floatPos, playerPos) < fishStartDistance)
				{
					m_setDistance = true;
				}

				m_isHit = true;
				m_fishing.IsHit();
				hitFish.IsHit();
			}
			float RodHeight = m_isHit ? HitHeight : NotHitHeight;

			transform.position = new Vector3(transform.position.x, RodHeight, transform.position.z);
			// �ނ蒆�̏���
			m_elapsedTime += Time.deltaTime;

			// �ނ�J�n���Ƀv���C���[�Ƃ̋����𒲐�����
			if (m_setDistance)
			{
				// ��苗���b���܂ŌJ��Ԃ�
				if (Vector2.Distance(floatPos, playerPos) < fishStartDistance)
				{
					transform.Translate(hitFish.transform.forward * Time.deltaTime * fishStartMoveSpeed); // �v���C���[���痣���悤�ɕ����𓮂���
					hitFish.SetStartPos();
					return; // �v���C���[�Ƃ̋����𒲐����Ă���Ԃ͑��̏������s��Ȃ�
				}
				else
				{
					m_setDistance = false; // ��苗���b������t���O�����Z�b�g
				}
			}

			// �v���C���[����苗���ɂ��鎞�͒ނ���I������
			if (m_isHit && Vector2.Distance(floatPos, playerPos) < fishEndDistance)
			{
				FishingEnd(m_isHit);
			}
		}

		// �C���ђʂ��Ă��܂����Ƃ��̗�O����
		if (transform.position.y < -10f) FishingEnd(false);
	}

	public void FishingStart(Vector3 forward)
	{
		if (m_throw) return;
		if (forward.y < 0.5f) forward = new Vector3(forward.x, 0.5f, forward.z);
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
			m_rigidbody.isKinematic = true;
			m_rigidbody.useGravity = false; // �d�͂𖳌���
			m_isFishing = true;
			m_fishData = GetFishData();
			hitFish.SetFishData(m_fishData); // ���̃f�[�^���Z�b�g
			SetHitTime();
		}

		if (other.transform.CompareTag("Player"))
		{
			FishingEnd(m_isHit); // �v���C���[�ɓ���������ނ���I��
		}
	}

	private FishDataEntity GetFishData()
	{
		// �ނ�̃f�[�^���擾����
		// �����_��(���Â�e�[�u���𐧍�)�ɋ���I��
		// �ނ�Ƃ̋����������قǗǂ�����������\�����オ��悤�ɂ���\��

		FishDataEntity fishData;
		int randomIndex = Random.Range(0, excelData.fish.Count);
		randomIndex = 100;
		fishData = excelData.fish[randomIndex];

		return fishData;
	}

	private void SetHitTime()
	{
		int seed = (int)Random.Range(5, 25);
		seed = 3;
		// ���Â�seed�ɍ��킹�ăe�[�u�������
		m_hitTime = seed;
	}

	public void FishingEnd(bool isSuccess)
    {
		m_fishing.FishingEnd(isSuccess, m_fishData);
		hitFish.FishingEnd();
		m_isPaused = false; // �|�[�Y��Ԃ����Z�b�g
		m_throw = false;	// �������t���O�����Z�b�g
        m_isFishing = false; // �ނ蒆�t���O�����Z�b�g
        m_fishData = null; // ���̃f�[�^�����Z�b�g
		m_isHit = false; // �q�b�g�t���O�����Z�b�g
		m_elapsedTime = 0f; // �o�ߎ��Ԃ����Z�b�g
        m_rigidbody.isKinematic = false; // �����𓮂�����悤�ɖ߂�
		m_rigidbody.useGravity = true; // �d�͂�L����
		gameObject.SetActive(false); // ����������
		m_rigidbody.velocity = Vector3.zero;	// AddForce��0�ɖ߂�
    }

	public bool IsFishing()
	{
		return m_isFishing;
	}

	public bool CanThrow()
	{
		return m_isFishing || m_throw;
	}

	public bool IsHit()
	{
		return m_isHit;
	}

	public int GetFishSize()
	{
		if (m_fishData != null)
		{
			return m_fishData.fishSize - 1;
		}
		return 0; // �����������Ă��Ȃ��ꍇ��0��Ԃ�
	}
}
