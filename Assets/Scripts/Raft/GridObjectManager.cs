using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class GridObjectManager
{
	// 座標(Vector2Int)をキーにオブジェクトの有無とオブジェクト管理
	static private Dictionary<Vector2Int, (bool, GameObject)> objectMap = new Dictionary<Vector2Int, (bool, GameObject)>();

	static public void Initialize(GameObject[] startGournd)
	{
		for (int i = 0; i < startGournd.Length; ++i)
		{
			GameObject ground = startGournd[i];
			Vector2Int pos = new Vector2Int(OddRound(ground.transform.position.x), OddRound(ground.transform.position.z));
			AddObject(pos, ground); // 初期化時にサンプルオブジェクトを追加
		}
		for (int i = 0; i < startGournd.Length; ++i)
		{
			if (startGournd[i].TryGetComponent<AroundWall>(out var aroundWall))
			{
				aroundWall.IsAroundGround(); // 周囲の床がある場合はコライダーを無効にする
			}
		}
	}

	// オブジェクトを追加
	static public void AddObject(Vector2Int position, GameObject ground)
	{
		objectMap[position] = (true, ground);
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

	static public (bool, GameObject) IsOnObject(Vector2Int position)
	{
		// 指定のポジションにオブジェクトがある場合はtrueを返す
		if (objectMap.TryGetValue(position, out var value))
		{
			return value;
		}
		return (false, null); // オブジェクトがない場合はfalseとnullを返す
	}


	static public int OddRound(float value)
	{
		int rounded = Mathf.RoundToInt(value);
		// 偶数なら1足して奇数化（または-1でもOK）
		if (rounded % 2 == 0)
		{
			// valueがroundedより大きければ+1、小さければ-1（近い方の奇数に寄せる）
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