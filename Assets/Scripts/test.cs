using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class test : MonoBehaviour
{
	[SerializeField] private ExcelData excelData;   // �G�N�Z���̃f�[�^���i�[����ϐ�
	[SerializeField] TextMeshPro nameText;   // ���O��\�����邽�߂�Text]

	private int hp;
	private int exp;
	private int price;

	void Start()
	{
		// �G�N�Z���f�[�^����ŏ��̋��̖��O���擾���ĕ\��
		if (excelData != null && excelData.fish.Count > 0)
		{
			nameText.text = excelData.fish[0].fishName;
			hp = excelData.fish[0].hp;
			exp = excelData.fish[0].exp;
			price = excelData.fish[0].price;
		}
	}
}
