using UnityEngine;

[System.Serializable]
public class FishDataEntity
{
	public int id;					// ID
	public string fishName;			// �摜�ƍ��킹�邽�߂̖��O
	public int exp;					// �o���l�i�������j
	public int price;				// ���i
	public int fishSize;            // ���̑傫���i1: ��, 2: ��, 3: ��j
	public float moveInterval;      // ���E�ɓ����Ԋu�i�b�j
	public float nextMoveTime;      // ���̍��E�ɓ����ÂÂ��鎞�ԁi�b�j
	public string displayName;		// �\����
	public string fishDescription;	// ���̐�����
}
