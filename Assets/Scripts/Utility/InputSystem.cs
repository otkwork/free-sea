using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour
{
	// InputSystemのメソッドは静的にして、どこからでも呼び出せるようにする

	// 移動
	static public bool MoveUp()
	{
		var keyCurrent = Keyboard.current;
		var padCurrent = Gamepad.current;
		
		if (keyCurrent != null && keyCurrent.wKey.isPressed ||
			padCurrent != null && padCurrent.leftStick.up.isPressed)
		{
			return true;
		}
		return false;
	}

	static public bool MoveLeft()
	{
		var keyCurrent = Keyboard.current;
		var padCurrent = Gamepad.current;

		if (keyCurrent != null && keyCurrent.aKey.isPressed ||
			padCurrent != null && padCurrent.leftStick.left.isPressed)
		{
			return true;
		}
		return false;
	}

	static public bool MoveDown()
	{
		var keyCurrent = Keyboard.current;
		var padCurrent = Gamepad.current;

		if (keyCurrent != null && keyCurrent.sKey.isPressed ||
			padCurrent != null && padCurrent.leftStick.down.isPressed)
		{
			return true;
		}
		return false;
	}

	static public bool MoveRight()
	{
		var keyCurrent = Keyboard.current;
		var padCurrent = Gamepad.current;

		if (keyCurrent != null && keyCurrent.dKey.isPressed ||
			padCurrent != null && padCurrent.leftStick.right.isPressed)
		{
			return true;
		}
		return false;
	}

	// ジャンプ
	static public bool Jump()
	{
		var keyCurrent = Keyboard.current;
		var padCurrent = Gamepad.current;

		if (keyCurrent != null && keyCurrent.spaceKey.wasPressedThisFrame ||
			padCurrent != null && padCurrent.aButton.wasPressedThisFrame)
		{
			return true;
		}
		return false;
	}

	// カメラの回転
	static public Vector2 CameraGetAxis(float sensX, float sensY, float padSens)
	{
		var keyCurrent = Keyboard.current;
		var padCurrent = Gamepad.current;

		if (padCurrent != null && padCurrent.rightStick.ReadValue() != Vector2.zero)
		{
			return padCurrent.rightStick.ReadValue() * padSens;
		}
		else
		{
			return new Vector2(Input.GetAxis("Mouse X") * sensX, Input.GetAxis("Mouse Y") * sensY);
		}
	}

	// メインアイテムの使用
	static public bool Fishing()
	{
		//var mouseCurrent = Mouse.current;
		var keyCurrent = Keyboard.current;
		var padCurrent = Gamepad.current;

		if (keyCurrent != null && keyCurrent.fKey.wasPressedThisFrame ||
			padCurrent != null && padCurrent.rightTrigger.wasPressedThisFrame)
		{
			return true;
		}
		return false;
	}
}
