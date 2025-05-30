using System.Linq.Expressions;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class Fishing : MonoBehaviour
{
	[SerializeField] Transform playerHead;
	[SerializeField] GameObject fishingRod;
	FishingRod rod;
	PlayerController playerController;

	static readonly Vector3 FloatOffset = new Vector3(0, 2, 0); // 浮きのオフセット
	const float BeforeHitReelSpeed = 2.0f;
	const float AfterHitReelSpeed = 0.1f;

	private void Awake()
	{
		rod = fishingRod.GetComponent<FishingRod>();
		playerController = GetComponent<PlayerController>();
	}

	void Start()
	{

	}


	void Update()
	{
		// 釣り中じゃない場合浮きを飛ばす
		if (!rod.IsFishing() && InputSystem.Fishing())
		{
			// 釣り開始
			fishingRod.SetActive(true);
			fishingRod.transform.position = playerHead.position + FloatOffset;
			rod.FishingStart(playerHead.forward);
		}

		// リールを巻く
		float wh = Input.GetAxis("Mouse ScrollWheel");
		// 巻く速度を調整
		wh *= rod.IsHit() ? AfterHitReelSpeed : BeforeHitReelSpeed;

		if (wh < 0)
		{
			fishingRod.transform.position += (fishingRod.transform.position - transform.position).normalized * wh * 2;
		}
	}

	public void FishingEnd(bool isSuccess, FishDataEntity fish)
	{
		// 釣り成功
		if (isSuccess)
		{
			Debug.Log(fish.fishName);
		}
		playerController.SetMove(true);
	}
}
