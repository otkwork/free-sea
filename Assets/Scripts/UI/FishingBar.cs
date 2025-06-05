using UnityEngine;
using UnityEngine.UI;

public class FishingBar : MonoBehaviour
{
	[SerializeField] FishingRod m_rodFloat; // 釣り竿のスクリプトを参照
	[SerializeField] Fishing m_fishing;     // 釣りのスクリプトを参照
	[SerializeField] HitFishMove m_hitFish;  // かかった魚のオブジェクト
	[SerializeField] GameObject m_fishingRod; // 釣り竿のオブジェクト
	[SerializeField] Slider m_fishingSlider; // UIのスライダーコンポーネント

	const float MaxValue = 100f; // スライダーの最大値
	const float MinValue = 0f;   // スライダーの最小値
	const float InitialValue = 50f; // スライダーの初期値

	const float ReelValue = 0.1f; // リール中にスライダーの値を増加させる量
	const float DirValue = 0.1f; // 釣り竿と魚の方向に応じてスライダーの値を調整する量

	const float RodDirThreshold = 10f; // 釣り竿の方向を調整するための閾値
	const float RodDirMax = 30f; // 釣り竿の方向の最大値

	void Start()
	{
		m_fishingSlider.maxValue = MaxValue; // スライダーの最大値を100に設定
		m_fishingSlider.minValue = MinValue;   // スライダーの最小値を0に設定
		m_fishingSlider.value = InitialValue; // 初期値を50に設定
	}

	void Update()
	{
		if (Cursor.visible) return; // カーソルが表示されている場合は何もしない(Pause)

		if (m_rodFloat.IsHit())
		{
			m_fishingSlider.gameObject.SetActive(true); // 釣り中はスライダーを表示

			// リール中の値更新
			Reel();

			// 釣り竿の方向に関する値更新
			RodDir();

			// スライダーの値が範囲外なら釣りを強制終了
			if (m_fishingSlider.value >= MaxValue || m_fishingSlider.value <= MinValue)
			{
				m_rodFloat.FishingEnd(false); // 釣りを終了
			}
		}
		else
		{
			m_fishingSlider.value = InitialValue; // 初期値を50に設定
			m_fishingSlider.gameObject.SetActive(false); // 釣り中でない場合はスライダーを非表示
		}
	}

	// リールに関する値の更新
	private void Reel()
	{
		if (m_fishing.IsReeling())
		{
			m_fishingSlider.value += ReelValue; // リール中はスライダーの値を増加
		}
	}

	// 釣り竿の方向に関する値の更新
	private void RodDir()
	{
		// X 0 :上, 60:下,
		// Z 30:左, 330(-30):右
		Vector3 rodDir = m_fishingRod.transform.localRotation.eulerAngles; // 釣り竿の方向を取得
		if (rodDir.z > 180f) // Z軸の値が180を超える場合、360から引く
		{
			rodDir.z -= 360f;
		}

		// -1:左, 0:真ん中, 1:右
		float fishDir = m_hitFish.GetDir(); // かかった魚の方向を取得

		// 魚の方向と釣り竿の方向に応じてスライダーの値を調整
		if (fishDir == -1) // 魚が左にいる場合
		{
			// 魚の方向と逆に釣り竿がある場合増加
			if (rodDir.z < -RodDirThreshold)
			{
				m_fishingSlider.value += DirValue * (Mathf.Abs(rodDir.z) / RodDirThreshold); // スライダーの値を増加
			}
			// 魚の方向と同じに釣り竿がある場合減少
			else if (rodDir.z > RodDirThreshold) 
			{
				m_fishingSlider.value -= DirValue * (Mathf.Abs(rodDir.z) / RodDirThreshold); // スライダーの値を減少
			}
		}
		else if (fishDir == 1)// 魚が右にいる場合
		{
			// 魚の方向と逆に釣り竿がある場合増加
			if (rodDir.z > RodDirThreshold)
			{
				m_fishingSlider.value += DirValue * (Mathf.Abs(rodDir.z) / RodDirThreshold); // スライダーの値を増加
			}
			// 魚の方向と同じに釣り竿がある場合減少
			else if (rodDir.z < -RodDirThreshold)
			{
				m_fishingSlider.value -= DirValue * (Mathf.Abs(rodDir.z) / RodDirThreshold); // スライダーの値を減少
			}
		}

		// 釣り竿の上下に応じてスライダーの値を調整
		if (rodDir.x - RodDirMax >= 0) // 釣り竿が下向きの場合
		{
			m_fishingSlider.value -= DirValue * (Mathf.Abs(rodDir.x) / RodDirThreshold); // スライダーの値を減少
		}
		else // 釣り竿が上向きの場合
		{
			m_fishingSlider.value += DirValue * (Mathf.Abs(rodDir.x) / RodDirThreshold); // スライダーの値を増加
		}
	}
}
