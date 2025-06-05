using UnityEngine;
using UnityEngine.UI;

public class FishingBar : MonoBehaviour
{
	[SerializeField] FishingRod m_rodFloat; // �ނ�Ƃ̃X�N���v�g���Q��
	[SerializeField] Fishing m_fishing;     // �ނ�̃X�N���v�g���Q��
	[SerializeField] HitFishMove m_hitFish;  // �����������̃I�u�W�F�N�g
	[SerializeField] GameObject m_fishingRod; // �ނ�Ƃ̃I�u�W�F�N�g
	[SerializeField] Slider m_fishingSlider; // UI�̃X���C�_�[�R���|�[�l���g

	const float MaxValue = 100f; // �X���C�_�[�̍ő�l
	const float MinValue = 0f;   // �X���C�_�[�̍ŏ��l
	const float InitialValue = 50f; // �X���C�_�[�̏����l

	const float ReelValue = 0.1f; // ���[�����ɃX���C�_�[�̒l�𑝉��������
	const float DirValue = 0.1f; // �ނ�ƂƋ��̕����ɉ����ăX���C�_�[�̒l�𒲐������

	const float RodDirThreshold = 10f; // �ނ�Ƃ̕����𒲐����邽�߂�臒l
	const float RodDirMax = 30f; // �ނ�Ƃ̕����̍ő�l

	void Start()
	{
		m_fishingSlider.maxValue = MaxValue; // �X���C�_�[�̍ő�l��100�ɐݒ�
		m_fishingSlider.minValue = MinValue;   // �X���C�_�[�̍ŏ��l��0�ɐݒ�
		m_fishingSlider.value = InitialValue; // �����l��50�ɐݒ�
	}

	void Update()
	{
		if (Cursor.visible) return; // �J�[�\�����\������Ă���ꍇ�͉������Ȃ�(Pause)

		if (m_rodFloat.IsHit())
		{
			m_fishingSlider.gameObject.SetActive(true); // �ނ蒆�̓X���C�_�[��\��

			// ���[�����̒l�X�V
			Reel();

			// �ނ�Ƃ̕����Ɋւ���l�X�V
			RodDir();

			// �X���C�_�[�̒l���͈͊O�Ȃ�ނ�������I��
			if (m_fishingSlider.value >= MaxValue || m_fishingSlider.value <= MinValue)
			{
				m_rodFloat.FishingEnd(false); // �ނ���I��
			}
		}
		else
		{
			m_fishingSlider.value = InitialValue; // �����l��50�ɐݒ�
			m_fishingSlider.gameObject.SetActive(false); // �ނ蒆�łȂ��ꍇ�̓X���C�_�[���\��
		}
	}

	// ���[���Ɋւ���l�̍X�V
	private void Reel()
	{
		if (m_fishing.IsReeling())
		{
			m_fishingSlider.value += ReelValue; // ���[�����̓X���C�_�[�̒l�𑝉�
		}
	}

	// �ނ�Ƃ̕����Ɋւ���l�̍X�V
	private void RodDir()
	{
		// X 0 :��, 60:��,
		// Z 30:��, 330(-30):�E
		Vector3 rodDir = m_fishingRod.transform.localRotation.eulerAngles; // �ނ�Ƃ̕������擾
		if (rodDir.z > 180f) // Z���̒l��180�𒴂���ꍇ�A360�������
		{
			rodDir.z -= 360f;
		}

		// -1:��, 0:�^��, 1:�E
		float fishDir = m_hitFish.GetDir(); // �����������̕������擾

		// ���̕����ƒނ�Ƃ̕����ɉ����ăX���C�_�[�̒l�𒲐�
		if (fishDir == -1) // �������ɂ���ꍇ
		{
			// ���̕����Ƌt�ɒނ�Ƃ�����ꍇ����
			if (rodDir.z < -RodDirThreshold)
			{
				m_fishingSlider.value += DirValue * (Mathf.Abs(rodDir.z) / RodDirThreshold); // �X���C�_�[�̒l�𑝉�
			}
			// ���̕����Ɠ����ɒނ�Ƃ�����ꍇ����
			else if (rodDir.z > RodDirThreshold) 
			{
				m_fishingSlider.value -= DirValue * (Mathf.Abs(rodDir.z) / RodDirThreshold); // �X���C�_�[�̒l������
			}
		}
		else if (fishDir == 1)// �����E�ɂ���ꍇ
		{
			// ���̕����Ƌt�ɒނ�Ƃ�����ꍇ����
			if (rodDir.z > RodDirThreshold)
			{
				m_fishingSlider.value += DirValue * (Mathf.Abs(rodDir.z) / RodDirThreshold); // �X���C�_�[�̒l�𑝉�
			}
			// ���̕����Ɠ����ɒނ�Ƃ�����ꍇ����
			else if (rodDir.z < -RodDirThreshold)
			{
				m_fishingSlider.value -= DirValue * (Mathf.Abs(rodDir.z) / RodDirThreshold); // �X���C�_�[�̒l������
			}
		}

		// �ނ�Ƃ̏㉺�ɉ����ăX���C�_�[�̒l�𒲐�
		if (rodDir.x - RodDirMax >= 0) // �ނ�Ƃ��������̏ꍇ
		{
			m_fishingSlider.value -= DirValue * (Mathf.Abs(rodDir.x) / RodDirThreshold); // �X���C�_�[�̒l������
		}
		else // �ނ�Ƃ�������̏ꍇ
		{
			m_fishingSlider.value += DirValue * (Mathf.Abs(rodDir.x) / RodDirThreshold); // �X���C�_�[�̒l�𑝉�
		}
	}
}
