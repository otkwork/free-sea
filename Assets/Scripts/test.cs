using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class test : MonoBehaviour
{
	[SerializeField] private ExcelData excelData;   // エクセルのデータを格納する変数
	[SerializeField] TextMeshPro nameText;   // 名前を表示するためのText

	void Start()
	{
		// エクセルデータから最初の魚の名前を取得して表示
		if (excelData != null && excelData.fish.Count > 0)
		{
			nameText.text = excelData.fish[0].fishName;
		}
	}
}
