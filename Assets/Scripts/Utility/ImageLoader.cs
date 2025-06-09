using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class ImageLoader
{
	// �w�肵���A�h���X��Sprite�����[�h���ĕ\������
	public static AsyncOperationHandle<Sprite> LoadSpriteAsync(string address)
	{
		return Addressables.LoadAssetAsync<Sprite>(address);
	}
}