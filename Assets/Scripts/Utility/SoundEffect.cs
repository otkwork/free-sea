using System.Collections.Generic;
using UnityEngine;

public class SoundEffect
{
	static private List<AudioSource> m_audioSource = new List<AudioSource>();
	static public AudioSource Play3D(AudioClip clip, Vector3 position, bool loop = false, float volume = 1, float pitch = 1)
	{
		return PlaySe(clip, position, 1, volume, pitch, loop);
	}

	static public AudioSource Play2D(AudioClip clip, bool loop = false, float volume = 1, float pitch = 1)
	{
		return PlaySe(clip, Vector3.zero, 0, volume, pitch, loop);
	}

	static AudioSource PlaySe(AudioClip clip, Vector3 position, float spatialBlend, float volume, float pitch, bool loop = false)
	{
		GameObject obj = new GameObject(clip.name);

		AudioSource audio = obj.AddComponent<AudioSource>();
		audio.clip = clip;
		audio.transform.position = position;
		audio.spatialBlend = spatialBlend;
		audio.loop = loop;
		audio.volume = volume;
		audio.pitch = pitch;

		audio.Play();

		m_audioSource.Add(audio);
		if (!loop) MonoBehaviour.Destroy(obj, clip.length * (1.0f / pitch));

		return audio;
	}

	static public void StopSe(AudioSource source)
	{
		MonoBehaviour.Destroy(source);
    }

    static public void OnDestory()
	{
        foreach(AudioSource audio in m_audioSource) MonoBehaviour.Destroy(audio);
    }
}
