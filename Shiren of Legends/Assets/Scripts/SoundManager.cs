using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips = null;
    [SerializeField] private AudioSource AudioSource = null;

    public void PlaySound(int num)
    {
        AudioSource.PlayOneShot(audioClips[num]);
    }
}
