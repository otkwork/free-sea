using System.Linq.Expressions;
using System.Security.Cryptography;
using UnityEditor.PackageManager;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fishing : MonoBehaviour
{
	[SerializeField] Transform playerHead;
	[SerializeField] GameObject rodFloat;
	[SerializeField] Animator rodAnime;
	[SerializeField] GameObject fishingRod;
	FishingRod rod;
	PlayerController playerController;

	static readonly Vector3 FloatOffset = new Vector3(0, 5, 0); // �����𓊂���Ƃ��̃I�t�Z�b�g
	readonly float[] reelSpeed =
	{
		1.5f,
		1.0f,
		0.5f
	};

	float rotationY;
	float rotationX;
	bool isHit;

	// ���[���̉�]���擾���邽�߂̕ϐ�
	float lastAngle = 0f;
	bool wasActive = false;

	private void Awake()
	{
		rod = rodFloat.GetComponent<FishingRod>();
		playerController = GetComponent<PlayerController>();
		isHit = false;
	}

	void Update()
	{
		if (Cursor.visible) return; // �J�[�\�����\������Ă���ꍇ�͉������Ȃ�(Pause)

		if (InputSystem.Fishing())
		{
			// �ނ蒆����Ȃ��ꍇ�������΂�
			if (!rod.CanThrow())
			{
				// �ނ�J�n
				rodAnime.SetTrigger("Throw");
				rodFloat.transform.position = playerHead.position + FloatOffset + transform.forward * -3;
				rodFloat.SetActive(true);
				rod.FishingStart(playerHead.forward);
			}
			// �����Ă���Œ�����Ȃ������������Ă��Ȃ��Ȃ畂�����������
			else if(rod.IsFishing() && !isHit)
			{
				rod.FishingEnd(false);
			}
		}

		
		// ���������������p�̋@�\
        if (isHit)
        {
			// ��ʂ��Œ肵�ĊƂ�����������悤�ɂ���
			MouseRod();

            // ���[��������
            Reel();
        }
	}

	private void MouseRod()
	{
        Vector2 mouseInput = InputSystem.CameraGetAxis();

        rotationX += mouseInput.y;
        rotationY -= mouseInput.x;
        rotationX += 0.5f;
        rotationX = Mathf.Clamp(rotationX, -30, 30);
        rotationY = Mathf.Clamp(rotationY, -30, 30);

        // ���A�̂̌����̓K�p
        fishingRod.transform.localRotation = Quaternion.Euler(rotationX + 30, 0, rotationY);
    }

	private void Reel()
	{
        float wh = InputSystem.ReelGetAxis(ref lastAngle, ref  wasActive);
        // �������x�𒲐�
        wh *= reelSpeed[rod.GetFishSize()];

        if (wh < 0)
        {
            rodFloat.transform.position += (rodFloat.transform.position - transform.position).normalized * wh;
        }
    }

	public void FishingEnd(bool isSuccess, FishDataEntity fish)
	{
		// �ނ萬��
		if (isSuccess)
		{
			Debug.Log(fish.fishName);
		}
		rodAnime.enabled = true;
		isHit = false;
		playerController.SetCamera(true);
		playerController.SetMove(true);
	}

	public void IsHit()
	{
		rodAnime.enabled = false;
		isHit = true;
		playerController.SetCamera(false);
		playerController.SetMove(false);
    }
}
