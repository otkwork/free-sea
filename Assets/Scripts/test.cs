using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class test : MonoBehaviour
{
	[SerializeField] private ExcelData excelData;   // �G�N�Z���̃f�[�^���i�[����ϐ�
	[SerializeField] TextMeshPro nameText;   // ���O��\�����邽�߂�Text

	void Start()
	{
		// �G�N�Z���f�[�^����ŏ��̋��̖��O���擾���ĕ\��
		if (excelData != null && excelData.fish.Count > 0)
		{
			nameText.text = excelData.fish[0].fishName;
		}
	}
}
