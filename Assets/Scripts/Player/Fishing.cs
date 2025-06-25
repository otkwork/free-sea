using TreeEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
	const int HammerId = 100; // ハンマーのID
	readonly float[] reelSpeed =
	{
		1.5f,
		1.0f,
		0.5f
	};

	private float rotationY;
	private float rotationX;
	private bool isHit;

	// リールの回転を取得するための変数
	private float lastAngle = 0f;
	private bool wasActive = false;
	// リールを巻いたかどうか
	private bool isReeling = false;

	private void Awake()
	{
		rod = rodFloat.GetComponent<FishingRod>();
		playerController = GetComponent<PlayerController>();
		isHit = false;
	}

	void Update()
	{
		if (SelectItem.GetItemType() != SelectItem.ItemType.FishingRod)
		{
			rodAnime.gameObject.SetActive(false); // 釣り竿を選択していない場合はアニメーションを無効にする
			rodFloat.SetActive(false); // 浮きを非表示にする
			rod.FishingEnd(false); // 釣りを終了する
			return; // 釣り竿を選択していない場合は何もしない
		}
		else
		{
			rodAnime.gameObject.SetActive(true); // 釣り竿を選択している場合はアニメーションを有効にする
		}

		if (PlayerController.IsPause()) return; // ポーズ中は何もしない

		if (InputSystem.UseItem())
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
		Vector3 bodyTargetPos = new Vector3(rodFloat.transform.position.x, transform.position.y, rodFloat.transform.position.z);
		transform.LookAt(bodyTargetPos);

		// 2. 頭（bodyの正面から上下のみでターゲットを見る）
		// 世界空間でターゲットへの方向ベクトル
		Vector3 dirToTarget = rodFloat.transform.position - playerHead.position;
		// bodyのローカル空間に変換
		Vector3 localDir = transform.InverseTransformDirection(dirToTarget);
		// X軸回転量を計算
		float angleX = Mathf.Atan2(-localDir.y, localDir.z) * Mathf.Rad2Deg;
		// 頭をX軸だけ回転
		playerHead.localRotation = Quaternion.Euler(angleX, 0f, 0f);

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
		// 釣り成功してハンマーのIDじゃない場合
		if (isSuccess)
		{
			if (fish.id != HammerId)
			{
				Inventory.AddItem(fish);
				VisualDictionary.AddItem(fish);
			}
			// 釣れたのがハンマーの場合
			else
			{
				SelectItem.SetHammer();
			}
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
