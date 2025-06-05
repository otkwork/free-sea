using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;

public class PlayerController : MonoBehaviour
{
	private CharacterController characterController;  // CharacterController型の変数
	[SerializeField] private Animator animator;
	[SerializeField] private Transform verRot;  //縦の視点移動の変数(カメラに合わせる)
	[SerializeField] private Transform horRot;  //横の視点移動の変数(プレイヤーに合わせる)
	[SerializeField] private float moveSpeed;  //移動速度
	[SerializeField] private float sensX = 2.0f;
	[SerializeField] private float sensY = 2.0f;
	[SerializeField] private float padSens = 2.0f;  //パッドの感度
	[SerializeField] private float jumpPower;  //ジャンプ力
	
	private Vector3 moveVelocity;  // キャラクターコントローラーを動かすためのVector3型の変数
	private float rotationY, rotationX;

	[SerializeField] private float _rayLength = 1f;	// Rayの長さ
	[SerializeField] private float _rayOffset;		// Rayをどれくらい身体にめり込ませるか
	[SerializeField] private LayerMask _layerMask;  // Rayの判定に用いるLayer

	bool canMove;   // 移動可能かどうか
	bool canCamera; // カメラ操作可能かどうか
	bool pause;     // 一時停止中かどうか

	void Start()
	{
		// フレームレートを60に固定
		Application.targetFrameRate = 60;
		characterController = GetComponent<CharacterController>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		canCamera = true; // カメラ操作可能にする
		canMove = true;   // 移動可能にする
		pause = false;
	}

	void FixedUpdate()
	{
		// ゲームの終了
		EndGame();

		// Tabキーoptionが押されたらポーズ画面を表示/非表示にする
		if (InputSystem.Pause()) pause = !pause;
		
		Cursor.lockState = pause ? CursorLockMode.None : CursorLockMode.Locked;
		Cursor.visible = pause;

		// カメラ操作
		if (canCamera && !pause) Camera();

		// 移動
		if (canMove && !pause) Move();

		// 移動スピードをアニメーターに反映する
		//animator.SetFloat("MoveSpeed", new Vector3(moveVelocity.x, 0, moveVelocity.z).magnitude);
	}

	private void Camera()
	{
		Vector2 mouseInput = InputSystem.CameraGetAxis(sensX, sensY, padSens);

		rotationX -= mouseInput.y;
		rotationY += mouseInput.x;
		rotationY %= 360; // 絶対値が大きくなりすぎないように

		// 上下の視点移動量をClamp
		rotationX = Mathf.Clamp(rotationX, -70, 70);

		// 頭、体の向きの適用
		verRot.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
		horRot.transform.localRotation = Quaternion.Euler(0, rotationY, 0);
	}

	private void Move()
	{
		//Wキーがおされたら
		if (InputSystem.MoveUp())
		{
			characterController.Move(this.gameObject.transform.forward * moveSpeed * Time.deltaTime);
		}
		//Sキーがおされたら
		if (InputSystem.MoveDown())
		{
			characterController.Move(this.gameObject.transform.forward * -1f * moveSpeed * Time.deltaTime);
		}
		//Aキーがおされたら
		if (InputSystem.MoveLeft())
		{
			characterController.Move(this.gameObject.transform.right * -1 * moveSpeed * Time.deltaTime);
		}
		//Dキーがおされたら
		if (InputSystem.MoveRight())
		{
			characterController.Move(this.gameObject.transform.right * moveSpeed * Time.deltaTime);
		}

		// 接地しているとき
		if (CheckGrounded())
		{
			// ジャンプ
			if (InputSystem.Jump())
			{
				moveVelocity.y = jumpPower;
			}
		}
		// 空中にいる時
		else
		{
			// 重力をかける
			moveVelocity.y += Physics.gravity.y * Time.deltaTime;
		}

		// キャラクターを動かす
		characterController.Move(moveVelocity * Time.deltaTime);
	}

	private bool CheckGrounded()
	{
		// 放つ光線の初期位置と姿勢
		// 若干身体にめり込ませた位置から発射しないと正しく判定できない時がある
		var ray = new Ray(origin: transform.position + Vector3.up * _rayOffset, direction: Vector3.down);

		// Raycastがhitするかどうかで判定
		// レイヤの指定を忘れずに
		return Physics.Raycast(ray, _rayLength, _layerMask);
	}

	//ゲーム終了
	private void EndGame()
	{
		//Escが押された時
		if (Input.GetKey(KeyCode.Escape))
		{

#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
		}

	}

	public void SetMove(bool move)
	{
		canMove = move;  // 移動可能かどうかを設定
	}

	public void SetCamera(bool camera)
	{
		canCamera = camera;  // カメラ操作可能かどうかを設定
	}
}