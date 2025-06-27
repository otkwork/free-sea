using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputSystem : MonoBehaviour
{
	// InputSystem�̃��\�b�h�͐ÓI�ɂ��āA�ǂ�����ł��Ăяo����悤�ɂ���

	// �ړ�
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

	// �J�����̉�]
	static public Vector2 CameraGetAxis(float sensX = 2, float sensY = 2, float padSens = 2)
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

	// ���C���A�C�e���̎g�p
	static public bool UseItem()
	{
		var mouseCurrent = Mouse.current;
		var padCurrent = Gamepad.current;

		if (mouseCurrent != null && mouseCurrent.leftButton.wasPressedThisFrame ||
			padCurrent != null && padCurrent.rightTrigger.wasPressedThisFrame)
		{
			return true;
		}
		return false;
	}

	// ���[���̊������
	static public float ReelGetAxis(ref float lastAngle, ref bool wasActive)
	{
		float wh = Input.GetAxis("Mouse ScrollWheel");
		(float scroll, float newAngle, bool newActive) = GetStickScrollDelta(lastAngle, wasActive);

		// ��Ԃ��X�V
		lastAngle = newAngle;
		wasActive = newActive;

		if (wh != 0)
		{
			return wh;
		}
		else if (scroll != 0f)
		{
			return scroll;
		}
		return 0f;
	}

	static private (float scrollDelta, float nextAngle, bool nextActive) GetStickScrollDelta(
	float lastAngle,
	bool wasActive,
	float sensitivity = 0.002f)
	{
		Vector2 stick = new Vector2(0, 0);
		if (Gamepad.current != null)
		{
			stick = Gamepad.current.leftStick.ReadValue();
		}

		Vector2 absStick = new Vector2(Mathf.Abs(stick.x), Mathf.Abs(stick.y));
		if (absStick.magnitude > 0.2f)
		{
			float currentAngle = Mathf.Atan2(absStick.y, absStick.x) * Mathf.Rad2Deg;
			float delta = Mathf.DeltaAngle(lastAngle, currentAngle);
			return (delta * sensitivity, currentAngle, true);
		}
		else if (wasActive)
		{
			// �X�e�B�b�N�����ꂽ�̂ŏ�ԃ��Z�b�g
			return (0f, 0f, false);
		}

		return (0f, lastAngle, false);
	}

	// PAUSE
	static public bool Pause()
	{
		// �|�[�Y���,�L�[�{�[�h���ǂ���
		var keyCurrent = Keyboard.current;
		var padCurrent = Gamepad.current;

		if (keyCurrent != null && keyCurrent.tabKey.wasPressedThisFrame ||
			padCurrent != null && padCurrent.startButton.wasPressedThisFrame)
		{
			return true;
		}
		return false;
	}

	static public bool GetInputMenuButtonDown(string isButton)
	{
        var padCurrent = Gamepad.current;
        var key = Keyboard.current;

        // �p�b�h
        if (padCurrent != null)
        {
            var dpad = padCurrent.dpad;

            if (dpad.left.wasPressedThisFrame && isButton == "Left") return true;
			else if (dpad.right.wasPressedThisFrame && isButton == "Right") return true;
			else if (dpad.up.wasPressedThisFrame && isButton == "Up") return true;
			else if (dpad.down.wasPressedThisFrame && isButton == "Down") return true;
			else if (padCurrent.buttonSouth.wasPressedThisFrame && isButton == "Decide") return true;
			else if (padCurrent.rightShoulder.wasPressedThisFrame && isButton == "Next") return true;
			else if (padCurrent.leftShoulder.wasPressedThisFrame && isButton == "Prev") return true;
			else if (padCurrent.leftTrigger.wasPressedThisFrame && isButton == "LB") return true;
			else if (padCurrent.rightTrigger.wasPressedThisFrame && isButton == "RB") return true;
			else if (padCurrent.buttonNorth.wasPressedThisFrame && isButton == "Sell") return true;
			else if (padCurrent.buttonEast.wasPressedThisFrame && isButton == "ChangeItem") return true;
			else if (padCurrent.allControls.Any(c => c is ButtonControl button && button.wasPressedThisFrame) && isButton == "Any") return true;
		}
        // �L�[�{�[�h
        if (key != null)
        {
            if (key.leftArrowKey.wasPressedThisFrame && isButton == "Left") return true;
			else if (key.rightArrowKey.wasPressedThisFrame && isButton == "Right") return true;
			else if (key.upArrowKey.wasPressedThisFrame && isButton == "Up") return true;
			else if (key.downArrowKey.wasPressedThisFrame && isButton == "Down") return true;
			else if (key.enterKey.wasPressedThisFrame && isButton == "Decide") return true;
			else if (key.dKey.wasPressedThisFrame && isButton == "Next") return true;
			else if (key.aKey.wasPressedThisFrame && isButton == "Prev") return true;
			else if (key.qKey.wasPressedThisFrame && isButton == "LB") return true;
			else if (key.eKey.wasPressedThisFrame && isButton == "RB") return true;
			else if (key.xKey.wasPressedThisFrame && isButton == "Sell") return true;
			else if (key.cKey.wasPressedThisFrame && isButton == "ChangeItem") return true;
			else if (key.anyKey.wasPressedThisFrame && isButton == "Any") return true; // �C�ӂ̃L�[�������ꂽ�ꍇ
		}

        return false;
    }
}
