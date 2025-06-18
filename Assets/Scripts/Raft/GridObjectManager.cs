using System.Collections.Generic;
using UnityEngine;

public class GridObjectManager
{
	// 座標(Vector2Int)をキーにオブジェクトの有無を管理（オブジェクトの種類は不要なのでbool型）
	static private Dictionary<Vector2Int, bool> objectMap = new Dictionary<Vector2Int, bool>();

	static public void Initialize()
	{
		for(int i = -1; i <= 1; i += 2)
		{
			for(int j = -1; j <= 1; j += 2)
			{
				Vector2Int pos = new Vector2Int(i, j);
				AddObject(pos); // 初期化時にサンプルオブジェクトを追加
			}
		}
	}

	// オブジェクトを追加
	static public void AddObject(Vector2Int position)
	{
		objectMap[position] = true;
	}

	// オブジェクトを削除
	static public void RemoveObject(Vector2Int position)
	{
		objectMap.Remove(position);
	}

	// 指定座標の上下左右どこかにオブジェクトがあればtrueを返す
	static public bool HasNeighborObject(Vector2Int position)
	{
		// 既に指定のポジションにオブジェクトがある場合はfalseを返す
		if (objectMap.ContainsKey(position)) return false;

		Vector2Int[] directions = {
			new Vector2Int(0, 2),   // 上
            new Vector2Int(0, -2),  // 下
            new Vector2Int(-2, 0),  // 左
            new Vector2Int(2, 0)    // 右
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