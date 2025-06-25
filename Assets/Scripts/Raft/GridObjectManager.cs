using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class GridObjectManager
{
	// ���W(Vector2Int)���L�[�ɃI�u�W�F�N�g�̗L���ƃI�u�W�F�N�g�Ǘ�
	static private Dictionary<Vector2Int, (bool, GameObject)> objectMap = new Dictionary<Vector2Int, (bool, GameObject)>();

	static public void Initialize(GameObject[] startGournd)
	{
		for (int i = 0; i < startGournd.Length; ++i)
		{
			GameObject ground = startGournd[i];
			Vector2Int pos = new Vector2Int(OddRound(ground.transform.position.x), OddRound(ground.transform.position.z));
			AddObject(pos, ground); // ���������ɃT���v���I�u�W�F�N�g��ǉ�
		}
		for (int i = 0; i < startGournd.Length; ++i)
		{
			if (startGournd[i].TryGetComponent<AroundWall>(out var aroundWall))
			{
				aroundWall.IsAroundGround(); // ���͂̏�������ꍇ�̓R���C�_�[�𖳌��ɂ���
			}
		}
	}

	// �I�u�W�F�N�g��ǉ�
	static public void AddObject(Vector2Int position, GameObject ground)
	{
		objectMap[position] = (true, ground);
	}

	// �I�u�W�F�N�g���폜
	static public void RemoveObject(Vector2Int position)
	{
		objectMap.Remove(position);
	}

	// �w����W�̏㉺���E�ǂ����ɃI�u�W�F�N�g�������true��Ԃ�
	static public bool HasNeighborObject(Vector2Int position)
	{
		// ���Ɏw��̃|�W�V�����ɃI�u�W�F�N�g������ꍇ��false��Ԃ�
		if (objectMap.ContainsKey(position)) return false;

		Vector2Int[] directions = {
			new Vector2Int(0, 2),   // ��
            new Vector2Int(0, -2),  // ��
            new Vector2Int(-2, 0),  // ��
            new Vector2Int(2, 0)    // �E
        };

		foreach (var dir in directions)
		{
			Vector2Int neighborPos = position + dir;
			if (objectMap.ContainsKey(neighborPos))
			{
				return true;
			}
		}
		return false;
	}

	static public (bool, GameObject) IsOnObject(Vector2Int position)
	{
		// �w��̃|�W�V�����ɃI�u�W�F�N�g������ꍇ��true��Ԃ�
		if (objectMap.TryGetValue(position, out var value))
		{
			return value;
		}
		return (false, null); // �I�u�W�F�N�g���Ȃ��ꍇ��false��null��Ԃ�
	}


	static public int OddRound(float value)
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