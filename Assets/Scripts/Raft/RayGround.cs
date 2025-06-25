using UnityEngine;

public class RayGround : MonoBehaviour
{
	[SerializeField] Transform groundParent;
	[SerializeField] GameObject m_ground;   // �ݒu����n�ʃI�u�W�F�N�g
	[SerializeField] Animator hammerAnime; // �n���}�[�̃A�j���[�V����
	GameObject[] m_startGround = new GameObject[(int)AroundWall.WallType.Length]; // �����̒n�ʃI�u�W�F�N�g
							  
	// ���C�̍ő勗��
	const float rayDistance = 100f;
	const float SetGroundHeight = -0.8f; // �ݒu����n�ʂ̍����iY���W�j

	private void Start()
	{
		hammerAnime.gameObject.SetActive(false); // �n���}�[���\���ɂ���

		for (int i = 0; i < m_startGround.Length; ++i)
		{
			m_startGround[i] = groundParent.GetChild(i).gameObject;
		}
		GridObjectManager.Initialize(m_startGround);
	}

	void Update()
	{
		if (SelectItem.GetItemType() != SelectItem.ItemType.Hammer)
		{
			hammerAnime.gameObject.SetActive(false); // �n���}�[��I�����Ă��Ȃ��ꍇ�͖����ɂ���
			return; // �n���}�[��I�����Ă��Ȃ��ꍇ�͉������Ȃ�	
		}
		else
		{
			hammerAnime.gameObject.SetActive(true); // �n���}�[��I�����Ă���ꍇ�͗L���ɂ���
		}

		if (PlayerController.IsPause()) return; // �J�[�\�����\������Ă���ꍇ�͉������Ȃ�(Pause)
		
		// �}�E�X���N���b�N��Ray����
		if (InputSystem.UseItem())
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
			// �q�b�g�������[���h���W���O���b�h���W�i�������j�ɕϊ��iXZ���ʁj
			Vector3 hitPos = hit.point;
			Vector2Int gridPos = new Vector2Int(GridObjectManager.OddRound(hitPos.x), GridObjectManager.OddRound(hitPos.z));

			// �㉺���E�ɃI�u�W�F�N�g�����邩����
			bool hasNeighbor = GridObjectManager.HasNeighborObject(gridPos);

			if (hasNeighbor)
			{
				hammerAnime.SetTrigger("Create"); // �n���}�[�̃A�j���[�V�������Đ�
				GameObject ground = Instantiate(m_ground, new Vector3(gridPos.x, SetGroundHeight, gridPos.y), Quaternion.identity, groundParent);
				GridObjectManager.AddObject(gridPos, ground); // �O���b�h���W�ɒn�ʃI�u�W�F�N�g��o�^
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
}
