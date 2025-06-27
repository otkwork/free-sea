using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private Transform m_verRot;  //縦の視点移動の変数(カメラに合わせる)
	[SerializeField] private Transform m_horRot;  //横の視点移動の変数(プレイヤーに合わせる)
	[SerializeField] private float m_moveSpeed = 5.0f;  //移動速度
	[SerializeField] private float m_sensX = 2.0f;
	[SerializeField] private float m_sensY = 2.0f;
	[SerializeField] private float m_padSens = 2.0f;  //パッドの感度
	private CharacterController m_characterController;  // CharacterController型の変数
	
	private Vector3 m_moveVelocity;  // キャラクターコントローラーを動かすためのVector3型の変数
	private float m_rotationY, m_rotationX;

	// メニュー
	[SerializeField] private GameObject m_pauseMenu; // ポーズメニューのUI

	private static bool m_pause;     // 一時停止中かどうか,およびポーズ画面にしたのがキーボードかどうか
	private bool m_canMove;   // 移動可能かどうか
	private bool m_canCamera; // カメラ操作可能かどうか

	void Start()
	{
		// フレームレートを60に固定
		Application.targetFrameRate = 60;
        m_characterController = GetComponent<CharacterController>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
        m_canCamera = true; // カメラ操作可能にする
        m_canMove = true;   // 移動可能にする
        m_pause = false;
	}

	void FixedUpdate()
	{
		// ゲームの終了
		EndGame();

		// Tabキーoptionが押されたらポーズ画面を表示/非表示にする
		if (!FishingRod.IsFishing() && InputSystem.Pause())
		{
            m_pause = !m_pause;
			
			Cursor.lockState = m_pause ? CursorLockMode.None : CursorLockMode.Locked;
			Cursor.visible = m_pause;
		}

        m_pauseMenu.SetActive(m_pause); // ポーズメニューのUIを表示/非表示にする

		// カメラ操作
		if (m_canCamera && !m_pause) Camera();

		// 移動
		if (m_canMove && !m_pause) Move();

		// 移動スピードをアニメーターに反映する
		//animator.SetFloat("MoveSpeed", new Vector3(moveVelocity.x, 0, moveVelocity.z).magnitude);
	}

	private void Camera()
	{
		Vector2 mouseInput = InputSystem.CameraGetAxis(m_sensX, m_sensY, m_padSens);

        m_rotationX -= mouseInput.y;
        m_rotationY += mouseInput.x;
        m_rotationY %= 360; // 絶対値が大きくなりすぎないように

        // 上下の視点移動量をClamp
        m_rotationX = Mathf.Clamp(m_rotationX, -70, 70);

        // 頭、体の向きの適用
        m_verRot.transform.localRotation = Quaternion.Euler(m_rotationX, 0, 0);
        m_horRot.transform.localRotation = Quaternion.Euler(0, m_rotationY, 0);
	}

	private void Move()
	{
		//Wキーがおされたら
		if (InputSystem.MoveUp())
		{
            m_characterController.Move(gameObject.transform.forward * m_moveSpeed * Time.deltaTime);
		}
		//Sキーがおされたら
		if (InputSystem.MoveDown())
		{
            m_characterController.Move(gameObject.transform.forward * -1f * m_moveSpeed * Time.deltaTime);
		}
		//Aキーがおされたら
		if (InputSystem.MoveLeft())
		{
            m_characterController.Move(gameObject.transform.right * -1 * m_moveSpeed * Time.deltaTime);
		}
		//Dキーがおされたら
		if (InputSystem.MoveRight())
		{
            m_characterController.Move(gameObject.transform.right * m_moveSpeed * Time.deltaTime);
		}

        // 重力をかける
        m_moveVelocity.y += Physics.gravity.y * Time.deltaTime;

        // キャラクターを動かす
        m_characterController.Move(m_moveVelocity * Time.deltaTime);
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
        m_canMove = move;  // 移動可能かどうかを設定
	}

	public void SetCamera(bool camera)
	{
        m_canCamera = camera;  // カメラ操作可能かどうかを設定
	}

	public static bool IsPause()
	{
		return m_pause;  // 現在のポーズ状態を返す
	}
}