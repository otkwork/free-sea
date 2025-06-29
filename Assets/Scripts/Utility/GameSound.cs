using UnityEngine;

public class GameSound : MonoBehaviour
{
    [SerializeField] private AudioClip m_bgm;
    [SerializeField] private AudioClip m_hitBgm;
    [SerializeField] private AudioClip m_seaSe;
    private AudioSource m_bgmSource;
    private AudioSource m_hitBgmSource;

    private void Awake()
    {
        m_bgmSource = SoundEffect.Play2D(m_bgm, true);
        SoundEffect.Play2D(m_seaSe, true);
    }

    public void ChangeBGM(bool isHit)
    {
        // ‚©‚©‚Á‚½Žž
        if (isHit)
        {
            m_hitBgmSource = SoundEffect.Play2D(m_hitBgm, true);
            SoundEffect.StopSe(m_bgmSource);
        }
        else
        {
            m_bgmSource = SoundEffect.Play2D(m_bgm, true);
            SoundEffect.StopSe(m_hitBgmSource);
        }
    }
}
