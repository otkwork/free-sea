using UnityEngine;

public class Fishing : MonoBehaviour
{
	[SerializeField] private Transform m_playerHead;
	[SerializeField] private GameObject m_rodFloat;
	[SerializeField] private Animator m_rodAnime;
	[SerializeField] private GameObject m_rodGrip;
	[SerializeField] private HitFishMove m_hitFishMove;
	[SerializeField] private FishGet m_fishGet;  // 釣り成功時に画面に表示する魚の情報を管理するスクリプト
	[SerializeField] private AudioClip m_fishingSe;
	[SerializeField] private AudioClip m_getFishSe;
	[SerializeField] private AudioClip m_failureSe;

    private FishingRod m_rod;
	private PlayerController m_playerController;

	private static readonly Vector3 FloatOffset = new Vector3(0, 5, 0); // 浮きを投げるときのオフセット
	private const int HammerId = 100; // ハンマーのID
	private readonly float[] ReelSpeed =
	{
		1.5f,
		1.0f,
		0.5f
	};

	private float m_rotationY;
	private float m_rotationX;
	private bool m_isHit;

	// リールの回転を取得するための変数
	private float m_lastAngle = 0f;
	private bool m_wasActive = false;

	// リールを巻いたかどうか
	private bool m_isReeling = false;

	private void Awake()
	{
        m_rod = m_rodFloat.GetComponent<FishingRod>();
        m_playerController = GetComponent<PlayerController>();
        m_isHit = false;
	}

	void Update()
	{
		if (SelectItem.GetItemType() != SelectItem.ItemType.FishingRod)
		{
            m_rodAnime.gameObject.SetActive(false); // 釣り竿を選択していない場合はアニメーションを無効にする
            if(m_rodFloat.activeSelf) m_rod.FishingEnd(false); // 釣りを終了する
            m_rodFloat.SetActive(false); // 浮きを非表示にする
			return; // 釣り竿を選択していない場合は何もしない
		}
		else
		{
            m_rodAnime.gameObject.SetActive(true); // 釣り竿を選択している場合はアニメーションを有効にする
		}

		if (PlayerController.IsPause()) return; // ポーズ中は何もしない

		if (InputSystem.UseItem())
		{
			// 釣り中じゃない場合浮きを飛ばす
			if (!m_rod.CanThrow())
			{
                // 釣り開始
                m_rodAnime.SetTrigger("Throw");
				SoundEffect.Play2D(m_fishingSe);
                m_rodFloat.transform.position = m_playerHead.position + FloatOffset + transform.forward * -3;
                m_rodFloat.SetActive(true);
                m_rod.FishingStart(transform.forward);
			}
			// 投げている最中じゃなく魚がかかっていないなら浮きを回収する
			else if(FishingRod.IsFishing() && !m_isHit)
			{
                m_rod.FishingEnd(false);
			}
		}

		
		// 魚がかかった時用の機構
        if (m_isHit)
        {
			// 画面を固定して竿だけ動かせるようにする
			MouseRod();

            // 魚が左右に動いていない時だけリールを巻く
            if (m_hitFishMove.GetDir() == 0) Reel();
        }
	}

	private void MouseRod()
	{
		Vector3 bodyTargetPos = new Vector3(m_rodFloat.transform.position.x, transform.position.y, m_rodFloat.transform.position.z);
		transform.LookAt(bodyTargetPos);

		// 2. 頭（bodyの正面から上下のみでターゲットを見る）
		// 世界空間でターゲットへの方向ベクトル
		Vector3 dirToTarget = m_rodFloat.transform.position - m_playerHead.position;
		// bodyのローカル空間に変換
		Vector3 localDir = transform.InverseTransformDirection(dirToTarget);
		// X軸回転量を計算
		float angleX = Mathf.Atan2(-localDir.y, localDir.z) * Mathf.Rad2Deg;
        // 頭をX軸だけ回転
        m_playerHead.localRotation = Quaternion.Euler(angleX, 0f, 0f);

		Vector2 mouseInput = InputSystem.CameraGetAxis();

        m_rotationX -= mouseInput.y;
        m_rotationY -= mouseInput.x;
        m_rotationX += 0.5f;
        m_rotationX = Mathf.Clamp(m_rotationX, -30, 30);
        m_rotationY = Mathf.Clamp(m_rotationY, -30, 30);

        // 頭、体の向きの適用
        m_rodGrip.transform.localRotation = Quaternion.Euler(m_rotationX + 30, 0, m_rotationY);
    }

	private void Reel()
	{
        m_isReeling = false;
		float wh = InputSystem.ReelGetAxis(ref m_lastAngle, ref m_wasActive);
        // 巻く速度を調整
        wh *= ReelSpeed[m_rod.GetFishSize()];

        if (wh < 0)
        {
            m_rodFloat.transform.position += (m_rodFloat.transform.position - transform.position).normalized * wh;
            m_isReeling = true;
		}
    }

	public void FishingEnd(bool isSuccess, FishDataEntity fish)
	{
		// 釣り成功してハンマーのIDじゃない場合
		if (isSuccess)
		{
			SoundEffect.Play2D(m_getFishSe);
			// 画面に表示する
			m_fishGet.FishingEnd(fish);

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
		else
		{
			SoundEffect.Play2D(m_failureSe);
		}
        m_rodAnime.enabled = true;
        m_isHit = false;
        m_playerController.SetCamera(true);
        m_playerController.SetMove(true);
	}

	public bool IsReeling()
	{
		return m_isReeling;
	}

	public void IsHit()
	{
        m_rodAnime.enabled = false;
        m_isHit = true;
        m_playerController.SetCamera(false);
        m_playerController.SetMove(false);
    }
}
