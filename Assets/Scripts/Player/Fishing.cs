using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class Fishing : MonoBehaviour
{
	[SerializeField] Transform playerHead;
	[SerializeField] GameObject fishingRod;
	FishingRod rod;

	private void Awake()
	{
		rod = fishingRod.GetComponent<FishingRod>();
	}

	void Start()
    {

	}


    void Update()
    {
        if (!rod.IsFishing() && InputSystem.Fishing())
		{
			// �ނ�J�n
			fishingRod.SetActive(true);
			fishingRod.transform.position = playerHead.position + new Vector3(0, 2, 0);
			rod.FishingStart(playerHead.forward);
		}

		// ���[��������
        float wh = Input.GetAxis("Mouse ScrollWheel");
        if (wh < 0)
        {
            fishingRod.transform.position += transform.forward.normalized * wh * 2;
        }
    }

	public void FishingEnd(FishDataEntity fish)
	{
		Debug.Log(fish.fishName);
	}
}
