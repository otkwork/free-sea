using UnityEngine;

public class RayGround : MonoBehaviour
{
	[SerializeField] GameObject m_ground;	// �ݒu����n�ʃI�u�W�F�N�g
	// ���C�̍ő勗��
	const float rayDistance = 100f;
	const float SetGroundHeight = -0.8f; // �ݒu����n�ʂ̍����iY���W�j

	private void Start()
	{
		GridObjectManager.Initialize();
	}

	void Update()
	{
		if (PlayerController.IsPause()) return; // �J�[�\�����\������Ă���ꍇ�͉������Ȃ�(Pause)
		
		// �}�E�X���N���b�N��Ray����
		if (Input.GetMouseButtonDown(0))
		{
			CheckRay();
		}
	}

	private void CheckRay()
	{
		// �J�����̒��S���牺�����iVector3.down�j��Ray���΂�
		Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

		// Water�ȊO�̃��C���[�}�X�N���쐬
		int waterLayer = LayerMask.NameToLayer("Water");
		int waterMask = 1 << waterLayer;
		int mask = ~waterMask;

		if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, waterMask))
		{
			Debug.Log(hit.transform.name);
			// �q�b�g�������[���h���W���O���b�h���W�i�������j�ɕϊ��iXZ���ʁj
			Vector3 hitPos = hit.point;
			Vector2Int gridPos = new Vector2Int(OddRound(hitPos.x), OddRound(hitPos.z));

			// �㉺���E�ɃI�u�W�F�N�g�����邩����
			bool hasNeighbor = GridObjectManager.HasNeighborObject(gridPos);

			if (hasNeighbor)
			{
				Debug.Log($"�O���b�h���W {gridPos} �̏㉺���E�ɃI�u�W�F�N�g������܂�");
				Instantiate(m_ground, new Vector3(gridPos.x, SetGroundHeight, gridPos.y), Quaternion.identity);
				GridObjectManager.AddObject(gridPos); // �O���b�h���W�ɒn�ʃI�u�W�F�N�g��o�^
			}
			else
			{
				Debug.Log($"�O���b�h���W {gridPos} �̏㉺���E�ɃI�u�W�F�N�g�͂���܂���");
			}
		}
		else
		{
			Debug.Log("Ray���I�u�W�F�N�g�Ƀq�b�g���܂���ł���");
		}

		Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red, 2f); // Ray������
	}

	private int OddRound(float value)
	{
		int rounded = Mathf.RoundToInt(value);
		// �����Ȃ�1�����Ċ���i�܂���-1�ł�OK�j
		if (rounded % 2 == 0)
		{
			// value��rounded���傫�����+1�A���������-1�i�߂����̊�Ɋ񂹂�j
			if (value >= rounded)
				return rounded + 1;
			else
				return rounded - 1;
		}
		else
		{
			return rounded;
		}
	}
}
