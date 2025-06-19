using UnityEngine;

public class AroundWall : MonoBehaviour
{
	public enum WallType // �C��: 'WallType' �� 'public' �ɕύX
	{
		Up,    // ��̕�
		Down,  // ���̕�
		Left,  // ���̕�
		Right, // �E�̕�

		Length // �ǂ̐�
	}

	private static readonly Vector2Int[] WallDirections =
	{
		new(0, 2),   // ��
		new(0, -2),  // ��
		new(-2, 0),  // ��
		new(2, 0)    // �E
	};

	[SerializeField] BoxCollider[] m_walls = new BoxCollider[(int)WallType.Length]; // ���͂̕ǂ̃R���C�_�[

	void Start()
	{
		for (int i = 0; i < (int)WallType.Length; ++i)
		{
			Vector2Int pos = new Vector2Int((int)transform.position.x, (int)transform.position.z);
			// �����̈ʒu����㉺���E�ɏ�������ꍇ�́A�R���C�_�[�𖳌��ɂ���
			var isOnObjectResult = GridObjectManager.IsOnObject(pos + WallDirections[i]);
			if (isOnObjectResult.Item1)
			{
				m_walls[i].enabled = false;
				isOnObjectResult.Item2.GetComponent<AroundWall>().IsAroundGround(); // ��������ꍇ�̓R���C�_�[�𖳌��ɂ���
			}
			else
			{
				m_walls[i].enabled = true;
			}
		}
	}

	// Update is called once per frame
	void Update()
	{

	}

	// �����̎��͂ɏ�������ꍇ�́A�R���C�_�[�𖳌��ɂ���
	public void IsAroundGround()
	{
		for (int i = 0; i < (int)WallType.Length; ++i)
		{
			Vector2Int pos = new Vector2Int((int)transform.position.x, (int)transform.position.z);
			// �����̈ʒu����㉺���E�ɏ�������ꍇ�́A�R���C�_�[�𖳌��ɂ���
			var isOnObjectResult = GridObjectManager.IsOnObject(pos + WallDirections[i]);
			if (isOnObjectResult.Item1)
			{
				m_walls[i].enabled = false;
			}
			else
			{
				m_walls[i].enabled = true;
			}
		}
	}
}
