using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class Fishing : MonoBehaviour
{
	[SerializeField] GameObject fishingRod;
	FishingRod rod;
	Transform playerHead;

	private void Awake()
	{
		rod = fishingRod.GetComponent<FishingRod>();
	}

	void Start()
    {
		playerHead = transform.GetChild(0);
	}


    void Update()
    {
        if (!rod.IsFishing() && InputSystem.Fishing())
		{
			// ’Þ‚èŠJŽn
			fishingRod.SetActive(true);
			fishingRod.transform.position = playerHead.position + playerHead.forward * 0.5f;
			rod.FishingStart(playerHead.forward);
		}
    }

	public void FishingEnd(FishDataEntity fish)
	{
		Debug.Log(fish.fishName);
	}
}
