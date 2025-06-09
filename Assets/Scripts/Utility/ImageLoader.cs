using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class ImageLoader
{
	// 指定したアドレスのSpriteをロードして表示する
	public static AsyncOperationHandle<Sprite> LoadSpriteAsync(string address)
	{
		return Addressables.LoadAssetAsync<Sprite>(address);
	}
}