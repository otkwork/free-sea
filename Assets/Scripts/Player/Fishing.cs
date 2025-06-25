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

	static readonly Vector3 FloatOffset = new Vector3(0, 5, 0); // �����𓊂���Ƃ��̃I�t�Z�b�g
	const int HammerId = 100; // �n���}�[��ID
	readonly float[] reelSpeed =
	{
		1.5f,
		1.0f,
		0.5f
	};

	private float rotationY;
	private float rotationX;
	private bool isHit;

	// ���[���̉�]���擾���邽�߂̕ϐ�
	private float lastAngle = 0f;
	private bool wasActive = false;
	// ���[�������������ǂ���
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
			rodAnime.gameObject.SetActive(false); // �ނ�Ƃ�I�����Ă��Ȃ��ꍇ�̓A�j���[�V�����𖳌��ɂ���
			rodFloat.SetActive(false); // �������\���ɂ���
			rod.FishingEnd(false); // �ނ���I������
			return; // �ނ�Ƃ�I�����Ă��Ȃ��ꍇ�͉������Ȃ�
		}
		else
		{
			rodAnime.gameObject.SetActive(true); // �ނ�Ƃ�I�����Ă���ꍇ�̓A�j���[�V������L���ɂ���
		}

		if (PlayerController.IsPause()) return; // �|�[�Y���͉������Ȃ�

		if (InputSystem.UseItem())
		{
			// �ނ蒆����Ȃ��ꍇ�������΂�
			if (!rod.CanThrow())
			{
				// �ނ�J�n
				rodAnime.SetTrigger("Throw");
				rodFloat.transform.position = playerHead.position + FloatOffset + transform.forward * -3;
				rodFloat.SetActive(true);
				rod.FishingStart(transform.forward);
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

            // �������E�ɓ����Ă��Ȃ����������[��������
            if (hitFishMove.GetDir() == 0) Reel();
        }
	}

	private void MouseRod()
	{
		Vector3 bodyTargetPos = new Vector3(rodFloat.transform.position.x, transform.position.y, rodFloat.transform.position.z);
		transform.LookAt(bodyTargetPos);

		// 2. ���ibody�̐��ʂ���㉺�݂̂Ń^�[�Q�b�g������j
		// ���E��ԂŃ^�[�Q�b�g�ւ̕����x�N�g��
		Vector3 dirToTarget = rodFloat.transform.position - playerHead.position;
		// body�̃��[�J����Ԃɕϊ�
		Vector3 localDir = transform.InverseTransformDirection(dirToTarget);
		// X����]�ʂ��v�Z
		float angleX = Mathf.Atan2(-localDir.y, localDir.z) * Mathf.Rad2Deg;
		// ����X��������]
		playerHead.localRotation = Quaternion.Euler(angleX, 0f, 0f);

		Vector2 mouseInput = InputSystem.CameraGetAxis();

        rotationX -= mouseInput.y;
        rotationY -= mouseInput.x;
        rotationX += 0.5f;
        rotationX = Mathf.Clamp(rotationX, -30, 30);
        rotationY = Mathf.Clamp(rotationY, -30, 30);

        // ���A�̂̌����̓K�p
        fishingRod.transform.localRotation = Quaternion.Euler(rotationX + 30, 0, rotationY);
    }

	private void Reel()
	{
		isReeling = false;
		float wh = InputSystem.ReelGetAxis(ref lastAngle, ref  wasActive);
        // �������x�𒲐�
        wh *= reelSpeed[rod.GetFishSize()];

        if (wh < 0)
        {
            rodFloat.transform.position += (rodFloat.transform.position - transform.position).normalized * wh;
			isReeling = true;
		}
    }

	public void FishingEnd(bool isSuccess, FishDataEntity fish)
	{
		// �ނ萬�����ăn���}�[��ID����Ȃ��ꍇ
		if (isSuccess)
		{
			if (fish.id != HammerId)
			{
				Inventory.AddItem(fish);
				VisualDictionary.AddItem(fish);
			}
			// �ނꂽ�̂��n���}�[�̏ꍇ
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
