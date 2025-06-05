using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;

public class PlayerController : MonoBehaviour
{
	private CharacterController characterController;  // CharacterController�^�̕ϐ�
	[SerializeField] private Animator animator;
	[SerializeField] private Transform verRot;  //�c�̎��_�ړ��̕ϐ�(�J�����ɍ��킹��)
	[SerializeField] private Transform horRot;  //���̎��_�ړ��̕ϐ�(�v���C���[�ɍ��킹��)
	[SerializeField] private float moveSpeed;  //�ړ����x
	[SerializeField] private float sensX = 2.0f;
	[SerializeField] private float sensY = 2.0f;
	[SerializeField] private float padSens = 2.0f;  //�p�b�h�̊��x
	[SerializeField] private float jumpPower;  //�W�����v��
	
	private Vector3 moveVelocity;  // �L�����N�^�[�R���g���[���[�𓮂������߂�Vector3�^�̕ϐ�
	private float rotationY, rotationX;

	[SerializeField] private float _rayLength = 1f;	// Ray�̒���
	[SerializeField] private float _rayOffset;		// Ray���ǂꂭ�炢�g�̂ɂ߂荞�܂��邩
	[SerializeField] private LayerMask _layerMask;  // Ray�̔���ɗp����Layer

	bool canMove;   // �ړ��\���ǂ���
	bool canCamera; // �J��������\���ǂ���
	bool pause;     // �ꎞ��~�����ǂ���

	void Start()
	{
		// �t���[�����[�g��60�ɌŒ�
		Application.targetFrameRate = 60;
		characterController = GetComponent<CharacterController>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		canCamera = true; // �J��������\�ɂ���
		canMove = true;   // �ړ��\�ɂ���
		pause = false;
	}

	void FixedUpdate()
	{
		// �Q�[���̏I��
		EndGame();

		// Tab�L�[option�������ꂽ��|�[�Y��ʂ�\��/��\���ɂ���
		if (InputSystem.Pause()) pause = !pause;
		
		Cursor.lockState = pause ? CursorLockMode.None : CursorLockMode.Locked;
		Cursor.visible = pause;

		// �J��������
		if (canCamera && !pause) Camera();

		// �ړ�
		if (canMove && !pause) Move();

		// �ړ��X�s�[�h���A�j���[�^�[�ɔ��f����
		//animator.SetFloat("MoveSpeed", new Vector3(moveVelocity.x, 0, moveVelocity.z).magnitude);
	}

	private void Camera()
	{
		Vector2 mouseInput = InputSystem.CameraGetAxis(sensX, sensY, padSens);

		rotationX -= mouseInput.y;
		rotationY += mouseInput.x;
		rotationY %= 360; // ��Βl���傫���Ȃ肷���Ȃ��悤��

		// �㉺�̎��_�ړ��ʂ�Clamp
		rotationX = Mathf.Clamp(rotationX, -70, 70);

		// ���A�̂̌����̓K�p
		verRot.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
		horRot.transform.localRotation = Quaternion.Euler(0, rotationY, 0);
	}

	private void Move()
	{
		//W�L�[�������ꂽ��
		if (InputSystem.MoveUp())
		{
			characterController.Move(this.gameObject.transform.forward * moveSpeed * Time.deltaTime);
		}
		//S�L�[�������ꂽ��
		if (InputSystem.MoveDown())
		{
			characterController.Move(this.gameObject.transform.forward * -1f * moveSpeed * Time.deltaTime);
		}
		//A�L�[�������ꂽ��
		if (InputSystem.MoveLeft())
		{
			characterController.Move(this.gameObject.transform.right * -1 * moveSpeed * Time.deltaTime);
		}
		//D�L�[�������ꂽ��
		if (InputSystem.MoveRight())
		{
			characterController.Move(this.gameObject.transform.right * moveSpeed * Time.deltaTime);
		}

		// �ڒn���Ă���Ƃ�
		if (CheckGrounded())
		{
			// �W�����v
			if (InputSystem.Jump())
			{
				moveVelocity.y = jumpPower;
			}
		}
		// �󒆂ɂ��鎞
		else
		{
			// �d�͂�������
			moveVelocity.y += Physics.gravity.y * Time.deltaTime;
		}

		// �L�����N�^�[�𓮂���
		characterController.Move(moveVelocity * Time.deltaTime);
	}

	private bool CheckGrounded()
	{
		// �������̏����ʒu�Ǝp��
		// �኱�g�̂ɂ߂荞�܂����ʒu���甭�˂��Ȃ��Ɛ���������ł��Ȃ���������
		var ray = new Ray(origin: transform.position + Vector3.up * _rayOffset, direction: Vector3.down);

		// Raycast��hit���邩�ǂ����Ŕ���
		// ���C���̎w���Y�ꂸ��
		return Physics.Raycast(ray, _rayLength, _layerMask);
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
		canMove = move;  // �ړ��\���ǂ�����ݒ�
	}

	public void SetCamera(bool camera)
	{
		canCamera = camera;  // �J��������\���ǂ�����ݒ�
	}
}