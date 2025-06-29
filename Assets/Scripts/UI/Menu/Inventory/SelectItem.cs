using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectItem : MonoBehaviour
{
	public enum ItemType
	{
		FishingRod, // 釣り竿
		Hammer,     // ハンマー

		Length, // アイテムの数
	}
	
	[SerializeField] private SelectItemIcon[] m_selectItem; // アイテムの画像を格納する配列
	[SerializeField] private TextMeshProUGUI m_haveGourndText; // 所持しているいかだの数を表示するテキスト

    private const int MaxDisplayNum = 99999; // 表示上限の定数

	static private int m_selectItemIndex;
	static private bool m_isHaveHammer; // ハンマーを持っているかどうか

	private void Start()
	{
		m_selectItemIndex = (int)ItemType.FishingRod; // 初期選択アイテムを釣り竿に設定
		m_isHaveHammer = false;

		for (int i = 0; i < (int)ItemType.Length; i++)
		{
			m_selectItem[i].SetClick(m_selectItem[i].GetItemType() == (ItemType)m_selectItemIndex); // 初期状態で釣り竿を選択状態にする
		}
	}

	private void Update()
	{
		int displayNum = RayGround.GetHaveGround(); // 所持しているいかだの数を取得
		
		// 表示上限を超えた場合表示を99999にする
		if (displayNum > MaxDisplayNum) displayNum = MaxDisplayNum;
		m_haveGourndText.text = "×\n" + displayNum.ToString(); // 所持しているいかだの数を更新

		if (InputSystem.GetInputMenuButtonDown("ChangeItem"))
		{
			// 今選択しているアイテムと違うアイテムを選択する
			m_selectItemIndex = m_selectItemIndex == (int)ItemType.FishingRod ? 
				(int)ItemType.Hammer : 
				(int)ItemType.FishingRod;
			
			for (int i = 0; i < (int)ItemType.Length; i++)
			{
				m_selectItem[i].SetClick(m_selectItemIndex == i); // 選択したアイテムをハイライト
			}
		}
	}

	public void Select(GameObject icon)
	{
		for (int i = 0; i < (int)ItemType.Length; i++)
		{
			// 選択したアイコンを設定
			if (icon == m_selectItem[i].gameObject)
			{
				if (m_selectItem[i].GetItemType() == ItemType.Hammer && !m_isHaveHammer) return; // ハンマーを持っていない場合は選択不可
				m_selectItemIndex = (int)m_selectItem[i].GetItemType(); // アイコンがクリックされたらそのアイテムを選択
				break;
			}
		}

		for (int i = 0; i < (int)ItemType.Length; i++)
		{
			m_selectItem[i].SetClick(m_selectItem[i].GetItemType() == (ItemType)m_selectItemIndex); // 選択されているアイコンをハイライト
		}
	}

	static public void SetHammer()
	{
		m_isHaveHammer = true; // ハンマーを持っている状態にする
	}

	static public bool GetIsHaveHammer()
	{
		return m_isHaveHammer; // ハンマーを持っているかどうかを返す
	}

	static public ItemType GetItemType()
	{
		return (ItemType)m_selectItemIndex;
	}
}
