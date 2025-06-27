using UnityEngine;

public class SoundEffect
{
	static public void Play3D(AudioClip clip, Vector3 position, bool loop = false, float volume = 1, float pitch = 1)
	{
		PlaySe(clip, position, 1, volume, pitch, loop);
	}

	static public void Play2D(AudioClip clip, bool loop = false, float volume = 1, float pitch = 1)
	{
		PlaySe(clip, Vector3.zero, 0, volume, pitch, loop);
	}

	static void PlaySe(AudioClip clip, Vector3 position, float spatialBlend, float volume, float pitch, bool loop = false)
	{
		GameObject obj = new GameObject(clip.name);

		AudioSource audio = obj.AddComponent<AudioSource>();
		audio.clip = clip;
		audio.transform.position = position;
		audio.spatialBlend = spatialBlend;
		audio.loop = false;
		audio.volume = volume;
		audio.pitch = pitch;

		audio.Play();

		if (!loop) MonoBehaviour.Destroy(obj, clip.length * (1.0f / pitch));
	}
}
