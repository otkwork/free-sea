using System.Collections.Generic;
using UnityEngine;

public class GridObjectManager
{
	// ���W(Vector2Int)���L�[�ɃI�u�W�F�N�g�̗L�����Ǘ��i�I�u�W�F�N�g�̎�ނ͕s�v�Ȃ̂�bool�^�j
	static private Dictionary<Vector2Int, bool> objectMap = new Dictionary<Vector2Int, bool>();

	static public void Initialize()
	{
		for(int i = -1; i <= 1; i += 2)
		{
			for(int j = -1; j <= 1; j += 2)
			{
				Vector2Int pos = new Vector2Int(i, j);
				AddObject(pos); // ���������ɃT���v���I�u�W�F�N�g��ǉ�
			}
		}
	}

	// �I�u�W�F�N�g��ǉ�
	static public void AddObject(Vector2Int position)
	{
		objectMap[position] = true;
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
}