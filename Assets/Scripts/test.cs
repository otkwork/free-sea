using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class test : MonoBehaviour
{
	[SerializeField] private ExcelData excelData;   // エクセルのデータを格納する変数
	[SerializeField] TextMeshPro nameText;   // 名前を表示するためのText]

	private int hp;
	private int exp;
	private int price;

	void Start()
	{
		// エクセルデータから最初の魚の名前を取得して表示
		if (excelData != null && excelData.fish.Count > 0)
		{
			nameText.text = excelData.fish[0].fishName;
			hp = excelData.fish[0].hp;
			exp = excelData.fish[0].exp;
			price = excelData.fish[0].price;
		}
	}
}
