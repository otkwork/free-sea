using System.Linq.Expressions;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class Fishing : MonoBehaviour
{
	[SerializeField] Transform playerHead;
	[SerializeField] GameObject fishingRod;
	FishingRod rod;
	PlayerController playerController;

	static readonly Vector3 FloatOffset = new Vector3(0, 2, 0); // �����̃I�t�Z�b�g
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
		// �ނ蒆����Ȃ��ꍇ�������΂�
		if (!rod.IsFishing() && InputSystem.Fishing())
		{
			// �ނ�J�n
			fishingRod.SetActive(true);
			fishingRod.transform.position = playerHead.position + FloatOffset;
			rod.FishingStart(playerHead.forward);
		}

		// ���[��������
		float wh = Input.GetAxis("Mouse ScrollWheel");
		// �������x�𒲐�
		wh *= rod.IsHit() ? AfterHitReelSpeed : BeforeHitReelSpeed;

		if (wh < 0)
		{
			fishingRod.transform.position += (fishingRod.transform.position - transform.position).normalized * wh * 2;
		}
	}

	public void FishingEnd(bool isSuccess, FishDataEntity fish)
	{
		// �ނ萬��
		if (isSuccess)
		{
			Debug.Log(fish.fishName);
		}
		playerController.SetMove(true);
	}
}
