using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip rotate;
    public AudioClip score;
    public AudioClip lose;
    public AudioClip pieceDown;
    public AudioClip pieceMove;
    public AudioClip hold;
    public AudioClip levelUp;

    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRotateSound()
    {
        PlaySound(rotate,0.25f);
    }
    public void PlayScoreSound()
    {
        PlaySound(score);
    }
    public void PlayLoseSound()
    {
        PlaySound(lose);
    }
    public void PlayPieceDownSound()
    {
        PlaySound(pieceDown);
    }
    public void PlayPieceMoveSound()
    {
        PlaySound(pieceMove, 0.15f);
    }
    public void PlayHoldSound()
    {
        PlaySound(hold);
    }
    public void PlayLevelUpSound()
    {
        PlaySound(levelUp, 0.22f);
    }

    private void PlaySound(AudioClip sound, float volume = 0.4f)
    {
        audioSource.PlayOneShot(sound, volume);
    }
}
