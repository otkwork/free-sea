using System.Drawing;
using System.Security.Cryptography;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class HitFishMove : MonoBehaviour
{

	[SerializeField] Transform lure;					// ���A�[�i�ނ�j�j��Transform
	[SerializeField] Transform player;					// �v���C���[��Transform
	[SerializeField] float moveInterval = 1.5f;			// ���E�ɓ����Ԋu�i�b�j
	[SerializeField] float nextMoveTime = 5f;           // ���̍��E�ɓ����ÂÂ��鎞�ԁi�b�j
	[SerializeField] float lateralMoveDistance = 2.0f;	// ���E�ɓ����ő勗��
	[SerializeField] float lateralMoveSpeed = 4.0f;     // ���E�ɓ����X�s�[�h
	FishDataEntity m_fishData; // ���̃f�[�^

	readonly Vector3 Size = new Vector3(0.5f, 0.5f, 0.5f); // ���̃T�C�Y
	readonly Vector3 HitOffset = new Vector3(0, -3f, 0); // �������������Ƃ��̈ʒu�I�t�Z�b�g

	Quaternion m_fishingStartRot; // �ނ�J�n���̌���
	private float elapsedTime = 0f;
	private bool isMovingLaterally = false;
	private Vector3 targetLateralPos;
	private int lateralDir = 1; // -1: ��, 1: �E
	private bool isfishing; // �����������������ǂ���
	private bool isHit; // ���������������ǂ���

	void Start()
	{
		isfishing = false; // ������Ԃł̓q�b�g���Ă��Ȃ�
		isHit = false;
	}


	void Update()
	{
		if (PlayerController.IsPause()) return; // �J�[�\�����\������Ă���ꍇ�͉������Ȃ�(Pause)

		if (!isfishing)
		{
			isMovingLaterally = false; // �ނ��ԂłȂ��ꍇ�͉��ړ��𖳌��ɂ���
			elapsedTime = 0f; // ���Ԃ����Z�b�g
			return; // �����������Ă��Ȃ��ꍇ�͉������Ȃ�
		}
		// ���A�[�̉�����ʒu�Ƃ���
		Vector3 basePos = lure.position + HitOffset + player.forward * 5;

		if (!isMovingLaterally)
		{
			// �v���C���[���瓦�������������
			transform.rotation = m_fishingStartRot;

			// ���A�[�̉��Ɉʒu����
			transform.position = Vector3.Lerp(transform.position, basePos, 0.01f);
			transform.position = new Vector3(transform.position.x, basePos.y, transform.position.z); // �����͊�ʒu�Ɠ����ɂ���

			if (!isHit) return; // �����������Ă��Ȃ��ꍇ�͉������Ȃ�
			elapsedTime += Time.deltaTime;
			// ���Ԋu�ō��E�ړ��J�n
			if (elapsedTime > moveInterval)
			{
				isMovingLaterally = true;
				elapsedTime = 0f; // ���Ԃ����Z�b�g

				// ���E�ǂ��炩�I��
				lateralDir = Random.value > 0.5f ? 1 : -1;

				// ���ړ��̕����Ɍ����i0�x:��A45�x:�E�A-45�x:���j
				float lateralAngle = lateralDir == 1 ? 45f : -45f;
				transform.rotation = Quaternion.Euler(0, m_fishingStartRot.eulerAngles.y + lateralAngle, 0);
			}
		}
		else
		{
			// ���ړ����̏���
			elapsedTime += Time.deltaTime;

			targetLateralPos = basePos + Vector3.right * lateralDir * lateralMoveDistance;
			targetLateralPos.y = basePos.y; // �����͊�ʒu�Ɠ����ɂ���
			
			// �������Ɉړ�
			transform.position = Vector3.MoveTowards(transform.position, targetLateralPos, lateralMoveSpeed * Time.deltaTime);

			// ���ړ����͏�ɍ��E����������
			float lateralAngle = lateralDir == 1 ? 45f : -45f;
			transform.rotation = Quaternion.Euler(0, m_fishingStartRot.eulerAngles.y + lateralAngle, 0);

			// ���ړ���������
			if (elapsedTime > nextMoveTime)
			{
				elapsedTime = 0f; // ���Ԃ����Z�b�g
				isMovingLaterally = false;
			}
		}
	}

	public void IsHit()
	{
		isHit = true; // ��������������Ԃɂ���
	}

	// ���������������̏����ݒ�
	public void SetFishData(FishDataEntity fishData)
	{
		//==========================================//
		// ���A�[�̈ʒu�ɍ��킹�ċ��̎�ނ�ݒ�\�� //
		//==========================================//

		m_fishData = fishData;
		transform.localScale = Size * m_fishData.fishSize; // ���̃T�C�Y��ݒ�
		isfishing = true; // �ނ��Ԃɂ���

		Vector3 escapeDir = (lure.position - player.position);
		escapeDir.y = 0; // �����𖳎����Đ����ړ��ɂ���
		m_fishingStartRot = Quaternion.LookRotation(escapeDir);
	}

	public void FishingEnd()
	{
		transform.position = HitOffset;
		isfishing = false; // �ނ��Ԃ�����
		isHit = false; // ��������������Ԃ�����
	}

	public float GetDir()
	{
		// ���E�̂ǂ��炩
		if (isMovingLaterally)
		{
			// 1: �E, -1: ��
			return lateralDir;
		}

		return 0;
	}
}
