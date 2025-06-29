using UnityEngine;

public class Fishing : MonoBehaviour
{
	[SerializeField] private Transform m_playerHead;
	[SerializeField] private GameObject m_rodFloat;
	[SerializeField] private Animator m_rodAnime;
	[SerializeField] private GameObject m_rodGrip;
	[SerializeField] private HitFishMove m_hitFishMove;
	[SerializeField] private FishGet m_fishGet;  // �ނ萬�����ɉ�ʂɕ\�����鋛�̏����Ǘ�����X�N���v�g
	[SerializeField] private AudioClip m_fishingSe;
	[SerializeField] private AudioClip m_getFishSe;
	[SerializeField] private AudioClip m_failureSe;

    private FishingRod m_rod;
	private PlayerController m_playerController;

	private static readonly Vector3 FloatOffset = new Vector3(0, 5, 0); // �����𓊂���Ƃ��̃I�t�Z�b�g
	private const int HammerId = 100; // �n���}�[��ID
	private readonly float[] ReelSpeed =
	{
		1.5f,
		1.0f,
		0.5f
	};

	private float m_rotationY;
	private float m_rotationX;
	private bool m_isHit;

	// ���[���̉�]���擾���邽�߂̕ϐ�
	private float m_lastAngle = 0f;
	private bool m_wasActive = false;

	// ���[�������������ǂ���
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
            m_rodAnime.gameObject.SetActive(false); // �ނ�Ƃ�I�����Ă��Ȃ��ꍇ�̓A�j���[�V�����𖳌��ɂ���
            if(m_rodFloat.activeSelf) m_rod.FishingEnd(false); // �ނ���I������
            m_rodFloat.SetActive(false); // �������\���ɂ���
			return; // �ނ�Ƃ�I�����Ă��Ȃ��ꍇ�͉������Ȃ�
		}
		else
		{
            m_rodAnime.gameObject.SetActive(true); // �ނ�Ƃ�I�����Ă���ꍇ�̓A�j���[�V������L���ɂ���
		}

		if (PlayerController.IsPause()) return; // �|�[�Y���͉������Ȃ�

		if (InputSystem.UseItem())
		{
			// �ނ蒆����Ȃ��ꍇ�������΂�
			if (!m_rod.CanThrow())
			{
                // �ނ�J�n
                m_rodAnime.SetTrigger("Throw");
				SoundEffect.Play2D(m_fishingSe);
                m_rodFloat.transform.position = m_playerHead.position + FloatOffset + transform.forward * -3;
                m_rodFloat.SetActive(true);
                m_rod.FishingStart(transform.forward);
			}
			// �����Ă���Œ�����Ȃ������������Ă��Ȃ��Ȃ畂�����������
			else if(FishingRod.IsFishing() && !m_isHit)
			{
                m_rod.FishingEnd(false);
			}
		}

		
		// ���������������p�̋@�\
        if (m_isHit)
        {
			// ��ʂ��Œ肵�ĊƂ�����������悤�ɂ���
			MouseRod();

            // �������E�ɓ����Ă��Ȃ����������[��������
            if (m_hitFishMove.GetDir() == 0) Reel();
        }
	}

	private void MouseRod()
	{
		Vector3 bodyTargetPos = new Vector3(m_rodFloat.transform.position.x, transform.position.y, m_rodFloat.transform.position.z);
		transform.LookAt(bodyTargetPos);

		// 2. ���ibody�̐��ʂ���㉺�݂̂Ń^�[�Q�b�g������j
		// ���E��ԂŃ^�[�Q�b�g�ւ̕����x�N�g��
		Vector3 dirToTarget = m_rodFloat.transform.position - m_playerHead.position;
		// body�̃��[�J����Ԃɕϊ�
		Vector3 localDir = transform.InverseTransformDirection(dirToTarget);
		// X����]�ʂ��v�Z
		float angleX = Mathf.Atan2(-localDir.y, localDir.z) * Mathf.Rad2Deg;
        // ����X��������]
        m_playerHead.localRotation = Quaternion.Euler(angleX, 0f, 0f);

		Vector2 mouseInput = InputSystem.CameraGetAxis();

        m_rotationX -= mouseInput.y;
        m_rotationY -= mouseInput.x;
        m_rotationX += 0.5f;
        m_rotationX = Mathf.Clamp(m_rotationX, -30, 30);
        m_rotationY = Mathf.Clamp(m_rotationY, -30, 30);

        // ���A�̂̌����̓K�p
        m_rodGrip.transform.localRotation = Quaternion.Euler(m_rotationX + 30, 0, m_rotationY);
    }

	private void Reel()
	{
        m_isReeling = false;
		float wh = InputSystem.ReelGetAxis(ref m_lastAngle, ref m_wasActive);
        // �������x�𒲐�
        wh *= ReelSpeed[m_rod.GetFishSize()];

        if (wh < 0)
        {
            m_rodFloat.transform.position += (m_rodFloat.transform.position - transform.position).normalized * wh;
            m_isReeling = true;
		}
    }

	public void FishingEnd(bool isSuccess, FishDataEntity fish)
	{
		// �ނ萬�����ăn���}�[��ID����Ȃ��ꍇ
		if (isSuccess)
		{
			SoundEffect.Play2D(m_getFishSe);
			// ��ʂɕ\������
			m_fishGet.FishingEnd(fish);

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
