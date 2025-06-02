using System.Security.Cryptography;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class HitFishMove : MonoBehaviour
{
	[SerializeField] GameObject player; // �v���C���[
	[SerializeField] GameObject rodFloat;   // ����

	readonly Vector3 Size = new Vector3(0.35f, 0.35f, 1); // ���̃T�C�Y
	readonly Vector3 HitOffset = new Vector3(0, -3f, 0); // �������������Ƃ��̈ʒu�I�t�Z�b�g
	const float MoveSpeed = 2f;           // �O�i���x
	const float MoveInterval = 1.0f;      // �ړ��̊Ԋu
	const float MoveAngle = 45f;          // ���E�ǂ��炩�̈ړ��p�x�i�x�j

	FishDataEntity m_fishData; // ���̃f�[�^
	Rigidbody m_rigidbody;

	bool isHit;

	float moveTimer = 0f; // �O�i�̃^�C�}�[
	void Start()
    {
		m_rigidbody = GetComponent<Rigidbody>();
		isHit = false; // ������Ԃł̓q�b�g���Ă��Ȃ�
	}

	// Update is called once per frame
	void Update()
    {
		m_rigidbody.isKinematic = Cursor.visible; ; // �J�[�\�����\������Ă���ꍇ�͕������Z�𖳌���
		if (Cursor.visible) return; // �J�[�\�����\������Ă���ꍇ�͉������Ȃ�(Pause)

		if (!isHit) return; // �����������Ă��Ȃ��ꍇ�͉������Ȃ�

		// �������v���C���[���瓦��������ɂ���
		Vector3 toPlayer = player.transform.position - transform.position;
		Vector3 escapeDir = -toPlayer.normalized;
		transform.rotation = Quaternion.LookRotation(escapeDir);

		// ���Ԋu�ō��E�ǂ��炩�ɑO�i
		moveTimer += Time.deltaTime;
		if (moveTimer >= MoveInterval)
		{
			moveTimer = 0f;

			// �����_���ɍ��E������i-1 or 1�j
			int direction = Random.value < 0.5f ? -1 : 1;

			// ���E�ɏ����p�x�����炵�đO�i
			Quaternion offsetRot = Quaternion.Euler(0f, direction * MoveAngle, 0f);
			Vector3 moveDir = offsetRot * transform.forward;

			transform.position += moveDir * MoveSpeed;
		}

		// Y���W��rod�̈ʒu - 3 �ɌŒ�
		Vector3 pos = rodFloat.transform.position;
		pos.y -= 3f;
		transform.position = pos;
	}

	// ���������������̏����ݒ�
	public void SetFishData(FishDataEntity fishData)
	{
		m_fishData = fishData;
		transform.localScale = Size * m_fishData.fishSize; // ���̃T�C�Y��ݒ�
		isHit = true; // ��������������Ԃɂ���
	}

	public void FishingEnd()
	{
		transform.position = HitOffset;
		isHit = false; // ��������������Ԃ�����
	}
}
