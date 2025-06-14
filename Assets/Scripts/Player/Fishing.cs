using UnityEngine;

public class Fishing : MonoBehaviour
{
	[SerializeField] Transform playerHead;
	[SerializeField] GameObject rodFloat;
	[SerializeField] Animator rodAnime;
	[SerializeField] GameObject fishingRod;
	[SerializeField] HitFishMove hitFishMove;
	FishingRod rod;
	PlayerController playerController;

	static readonly Vector3 FloatOffset = new Vector3(0, 5, 0); // 浮きを投げるときのオフセット
	readonly float[] reelSpeed =
	{
		1.5f,
		1.0f,
		0.5f
	};

	float rotationY;
	float rotationX;
	bool isHit;

	// リールの回転を取得するための変数
	float lastAngle = 0f;
	bool wasActive = false;
	// リールを巻いたかどうか
	bool isReeling = false;

	private void Awake()
	{
		rod = rodFloat.GetComponent<FishingRod>();
		playerController = GetComponent<PlayerController>();
		isHit = false;
	}

	void Update()
	{
		if (Cursor.visible) return; // カーソルが表示されている場合は何もしない(Pause)

		if (InputSystem.Fishing())
		{
			// 釣り中じゃない場合浮きを飛ばす
			if (!rod.CanThrow())
			{
				// 釣り開始
				rodAnime.SetTrigger("Throw");
				rodFloat.transform.position = playerHead.position + FloatOffset + transform.forward * -3;
				rodFloat.SetActive(true);
				rod.FishingStart(transform.forward);
			}
			// 投げている最中じゃなく魚がかかっていないなら浮きを回収する
			else if(rod.IsFishing() && !isHit)
			{
				rod.FishingEnd(false);
			}
		}

		
		// 魚がかかった時用の機構
        if (isHit)
        {
			// 画面を固定して竿だけ動かせるようにする
			MouseRod();

            // 魚が左右に動いていない時だけリールを巻く
            if (hitFishMove.GetDir() == 0) Reel();
        }
	}

	private void MouseRod()
	{
		playerHead.transform.rotation = Quaternion.LookRotation(rodFloat.transform.position);
		transform.rotation = Quaternion.LookRotation(rodFloat.transform.position);

		Vector2 mouseInput = InputSystem.CameraGetAxis();

        rotationX -= mouseInput.y;
        rotationY -= mouseInput.x;
        rotationX += 0.5f;
        rotationX = Mathf.Clamp(rotationX, -30, 30);
        rotationY = Mathf.Clamp(rotationY, -30, 30);

        // 頭、体の向きの適用
        fishingRod.transform.localRotation = Quaternion.Euler(rotationX + 30, 0, rotationY);
    }

	private void Reel()
	{
		isReeling = false;
		float wh = InputSystem.ReelGetAxis(ref lastAngle, ref  wasActive);
        // 巻く速度を調整
        wh *= reelSpeed[rod.GetFishSize()];

        if (wh < 0)
        {
            rodFloat.transform.position += (rodFloat.transform.position - transform.position).normalized * wh;
			isReeling = true;
		}
    }

	public void FishingEnd(bool isSuccess, FishDataEntity fish)
	{
		// 釣り成功
		if (isSuccess)
		{
			Inventory.AddItem(fish);
			VisualDictionary.AddItem(fish);
		}
		rodAnime.enabled = true;
		isHit = false;
		playerController.SetCamera(true);
		playerController.SetMove(true);
	}

	public bool IsReeling()
	{
		return isReeling;
	}

	public void IsHit()
	{
		rodAnime.enabled = false;
		isHit = true;
		playerController.SetCamera(false);
		playerController.SetMove(false);
    }
}
