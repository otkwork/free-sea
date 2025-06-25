using UnityEngine;

public class HitFishMove : MonoBehaviour
{

	[SerializeField] private Transform m_lure;					// ���A�[�i�ނ�j�j��Transform
	[SerializeField] private Transform m_player;				// �v���C���[��Transform
	[SerializeField] private float m_lateralMoveDistance = 2.0f;// ���E�ɓ����ő勗��
	[SerializeField] private float m_lateralMoveSpeed = 4.0f;   // ���E�ɓ����X�s�[�h
	private FishDataEntity m_fishData; // ���̃f�[�^
	
	private readonly Vector3 Size = new Vector3(0.5f, 0.5f, 0.5f); // ���̃T�C�Y
	private readonly Vector3 HitOffset = new Vector3(0, -3f, 0); // �������������Ƃ��̈ʒu�I�t�Z�b�g

	private Quaternion m_fishingStartRot; // �ނ�J�n���̌���
	private Vector3 m_targetLateralPos;
	private float m_elapsedTime = 0f;
	private int m_lateralDir = 1; // -1: ��, 1: �E
	private bool m_isMovingLaterally = false;
	private bool m_isfishing; // �����������������ǂ���
	private bool m_isHit; // ���������������ǂ���

	void Start()
	{
		m_isfishing = false; // ������Ԃł̓q�b�g���Ă��Ȃ�
		m_isHit = false;
	}


	void Update()
	{
		if (SelectItem.GetItemType() != SelectItem.ItemType.FishingRod) return; // �ނ�Ƃ�I�����Ă��Ȃ��ꍇ�͉������Ȃ�
		if (PlayerController.IsPause()) return; // �J�[�\�����\������Ă���ꍇ�͉������Ȃ�(Pause)

		if (!m_isfishing)
		{
            m_isMovingLaterally = false; // �ނ��ԂłȂ��ꍇ�͉��ړ��𖳌��ɂ���
            m_elapsedTime = 0f; // ���Ԃ����Z�b�g
			return; // �����������Ă��Ȃ��ꍇ�͉������Ȃ�
		}
		// ���A�[�̉�����ʒu�Ƃ���
		Vector3 basePos = m_lure.position + HitOffset + m_player.forward * 5;

		Vector3 escapeDir = (m_lure.position - m_player.position);
		escapeDir.y = 0; // �����𖳎����Đ����ړ��ɂ���
		m_fishingStartRot = Quaternion.LookRotation(escapeDir);

		if (!m_isMovingLaterally)
		{
			// �v���C���[���瓦�������������
			transform.rotation = m_fishingStartRot;

			// ���A�[�̉��Ɉʒu����
			transform.position = Vector3.Lerp(transform.position, basePos, 0.01f);
			transform.position = new Vector3(transform.position.x, basePos.y, transform.position.z); // �����͊�ʒu�Ɠ����ɂ���

			if (!m_isHit) return; // �����������Ă��Ȃ��ꍇ�͉������Ȃ�
            m_elapsedTime += Time.deltaTime;
			// ���Ԋu�ō��E�ړ��J�n
			if (m_elapsedTime > m_fishData.moveInterval)
			{
                m_isMovingLaterally = true;
                m_elapsedTime = 0f; // ���Ԃ����Z�b�g

                // ���E�ǂ��炩�I��
                m_lateralDir = Random.value > 0.5f ? 1 : -1;

				// ���ړ��̕����Ɍ����i0�x:��A45�x:�E�A-45�x:���j
				float lateralAngle = m_lateralDir == 1 ? 45f : -45f;
				transform.rotation = Quaternion.Euler(0, m_fishingStartRot.eulerAngles.y + lateralAngle, 0);
			}
		}
		else
		{
            // ���ړ����̏���
            m_elapsedTime += Time.deltaTime;

            m_targetLateralPos = basePos + Vector3.right * m_lateralDir * m_lateralMoveDistance;
            m_targetLateralPos.y = basePos.y; // �����͊�ʒu�Ɠ����ɂ���
			
			// �������Ɉړ�
			transform.position = Vector3.MoveTowards(transform.position, m_targetLateralPos, m_lateralMoveSpeed * Time.deltaTime);

			// ���ړ����͏�ɍ��E����������
			float lateralAngle = m_lateralDir == 1 ? 45f : -45f;
			transform.rotation = Quaternion.Euler(0, m_fishingStartRot.eulerAngles.y + lateralAngle, 0);

			// ���ړ���������
			if (m_elapsedTime > m_fishData.nextMoveTime)
			{
                m_elapsedTime = 0f; // ���Ԃ����Z�b�g
                m_isMovingLaterally = false;
			}
		}
	}

	public void IsHit()
	{
        m_isHit = true; // ��������������Ԃɂ���
	}

	// ���������������̏����ݒ�
	public void SetFishData(FishDataEntity fishData)
	{
		//==========================================//
		// ���A�[�̈ʒu�ɍ��킹�ċ��̎�ނ�ݒ�\�� //
		//==========================================//

		m_fishData = fishData;
		transform.localScale = Size * m_fishData.fishSize; // ���̃T�C�Y��ݒ�
        m_isfishing = true; // �ނ��Ԃɂ���
	}

	public void FishingEnd()
	{
		transform.position = HitOffset;
        m_isfishing = false; // �ނ��Ԃ�����
        m_isHit = false; // ��������������Ԃ�����
	}

	public float GetDir()
	{
		// ���E�̂ǂ��炩
		if (m_isMovingLaterally)
		{
			// 1: �E, -1: ��
			return m_lateralDir;
		}

		return 0;
	}

	public void SetStartPos()
	{
		// �ނ�J�n�����̃R�[�h��ʂ鎞�͋��̈ʒu�����A�[�̈ʒu���班���O���ɐݒ肷��
		transform.position = m_lure.position + HitOffset + m_player.forward * 5;
		m_fishingStartRot = Quaternion.LookRotation(m_lure.position - m_player.position);
		transform.rotation = m_fishingStartRot; // �v���C���[���瓦�������������
        m_isMovingLaterally = false; // ���ړ��𖳌��ɂ���
        m_elapsedTime = 0f; // ���Ԃ����Z�b�g
	}
}
