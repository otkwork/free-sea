using UnityEngine;

[System.Serializable]
public class FishDataEntity
{
	public int id;					// ID
	public string fishName;			// 画像と合わせるための名前
	public int exp;					// 経験値（未実装）
	public int price;				// 価格
	public int fishSize;            // 魚の大きさ（1: 小, 2: 中, 3: 大）
	public float moveInterval;      // 左右に動く間隔（秒）
	public float nextMoveTime;      // 一回の左右に動きづづける時間（秒）
	public string displayName;		// 表示名
	public string fishDescription;	// 魚の説明文
}
