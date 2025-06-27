using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private Transform m_verRot;  //�c�̎��_�ړ��̕ϐ�(�J�����ɍ��킹��)
	[SerializeField] private Transform m_horRot;  //���̎��_�ړ��̕ϐ�(�v���C���[�ɍ��킹��)
	[SerializeField] private float m_moveSpeed = 5.0f;  //�ړ����x
	[SerializeField] private float m_sensX = 2.0f;
	[SerializeField] private float m_sensY = 2.0f;
	[SerializeField] private float m_padSens = 2.0f;  //�p�b�h�̊��x
	private CharacterController m_characterController;  // CharacterController�^�̕ϐ�
	
	private Vector3 m_moveVelocity;  // �L�����N�^�[�R���g���[���[�𓮂������߂�Vector3�^�̕ϐ�
	private float m_rotationY, m_rotationX;

	// ���j���[
	[SerializeField] private GameObject m_pauseMenu; // �|�[�Y���j���[��UI

	private static bool m_pause;     // �ꎞ��~�����ǂ���,����у|�[�Y��ʂɂ����̂��L�[�{�[�h���ǂ���
	private bool m_canMove;   // �ړ��\���ǂ���
	private bool m_canCamera; // �J��������\���ǂ���

	void Start()
	{
		// �t���[�����[�g��60�ɌŒ�
		Application.targetFrameRate = 60;
        m_characterController = GetComponent<CharacterController>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
        m_canCamera = true; // �J��������\�ɂ���
        m_canMove = true;   // �ړ��\�ɂ���
        m_pause = false;
	}

	void FixedUpdate()
	{
		// �Q�[���̏I��
		EndGame();

		// Tab�L�[option�������ꂽ��|�[�Y��ʂ�\��/��\���ɂ���
		if (!FishingRod.IsFishing() && InputSystem.Pause())
		{
            m_pause = !m_pause;
			
			Cursor.lockState = m_pause ? CursorLockMode.None : CursorLockMode.Locked;
			Cursor.visible = m_pause;
		}

        m_pauseMenu.SetActive(m_pause); // �|�[�Y���j���[��UI��\��/��\���ɂ���

		// �J��������
		if (m_canCamera && !m_pause) Camera();

		// �ړ�
		if (m_canMove && !m_pause) Move();

		// �ړ��X�s�[�h���A�j���[�^�[�ɔ��f����
		//animator.SetFloat("MoveSpeed", new Vector3(moveVelocity.x, 0, moveVelocity.z).magnitude);
	}

	private void Camera()
	{
		Vector2 mouseInput = InputSystem.CameraGetAxis(m_sensX, m_sensY, m_padSens);

        m_rotationX -= mouseInput.y;
        m_rotationY += mouseInput.x;
        m_rotationY %= 360; // ��Βl���傫���Ȃ肷���Ȃ��悤��

        // �㉺�̎��_�ړ��ʂ�Clamp
        m_rotationX = Mathf.Clamp(m_rotationX, -70, 70);

        // ���A�̂̌����̓K�p
        m_verRot.transform.localRotation = Quaternion.Euler(m_rotationX, 0, 0);
        m_horRot.transform.localRotation = Quaternion.Euler(0, m_rotationY, 0);
	}

	private void Move()
	{
		//W�L�[�������ꂽ��
		if (InputSystem.MoveUp())
		{
            m_characterController.Move(gameObject.transform.forward * m_moveSpeed * Time.deltaTime);
		}
		//S�L�[�������ꂽ��
		if (InputSystem.MoveDown())
		{
            m_characterController.Move(gameObject.transform.forward * -1f * m_moveSpeed * Time.deltaTime);
		}
		//A�L�[�������ꂽ��
		if (InputSystem.MoveLeft())
		{
            m_characterController.Move(gameObject.transform.right * -1 * m_moveSpeed * Time.deltaTime);
		}
		//D�L�[�������ꂽ��
		if (InputSystem.MoveRight())
		{
            m_characterController.Move(gameObject.transform.right * m_moveSpeed * Time.deltaTime);
		}

        // �d�͂�������
        m_moveVelocity.y += Physics.gravity.y * Time.deltaTime;

        // �L�����N�^�[�𓮂���
        m_characterController.Move(m_moveVelocity * Time.deltaTime);
	}

	//�Q�[���I��
	private void EndGame()
	{
		//Esc�������ꂽ��
		if (Input.GetKey(KeyCode.Escape))
		{

#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
    Application.Quit();//�Q�[���v���C�I��
#endif
		}

	}

	public void SetMove(bool move)
	{
        m_canMove = move;  // �ړ��\���ǂ�����ݒ�
	}

	public void SetCamera(bool camera)
	{
        m_canCamera = camera;  // �J��������\���ǂ�����ݒ�
	}

	public static bool IsPause()
	{
		return m_pause;  // ���݂̃|�[�Y��Ԃ�Ԃ�
	}
}