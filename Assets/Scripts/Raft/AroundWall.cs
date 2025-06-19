using UnityEngine;

public class AroundWall : MonoBehaviour
{
	public enum WallType // 修正: 'WallType' を 'public' に変更
	{
		Up,    // 上の壁
		Down,  // 下の壁
		Left,  // 左の壁
		Right, // 右の壁

		Length // 壁の数
	}

	private static readonly Vector2Int[] WallDirections =
	{
		new(0, 2),   // 上
		new(0, -2),  // 下
		new(-2, 0),  // 左
		new(2, 0)    // 右
	};

	[SerializeField] BoxCollider[] m_walls = new BoxCollider[(int)WallType.Length]; // 周囲の壁のコライダー

	void Start()
	{
		for (int i = 0; i < (int)WallType.Length; ++i)
		{
			Vector2Int pos = new Vector2Int((int)transform.position.x, (int)transform.position.z);
			// 自分の位置から上下左右に床がある場合は、コライダーを無効にする
			var isOnObjectResult = GridObjectManager.IsOnObject(pos + WallDirections[i]);
			if (isOnObjectResult.Item1)
			{
				m_walls[i].enabled = false;
				isOnObjectResult.Item2.GetComponent<AroundWall>().IsAroundGround(); // 床がある場合はコライダーを無効にする
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

	// 自分の周囲に床がある場合は、コライダーを無効にする
	public void IsAroundGround()
	{
		for (int i = 0; i < (int)WallType.Length; ++i)
		{
			Vector2Int pos = new Vector2Int((int)transform.position.x, (int)transform.position.z);
			// 自分の位置から上下左右に床がある場合は、コライダーを無効にする
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
