using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip slime;

    [SerializeField] private PlayerControl playerControl;

    private void Start()
    {
        playerControl.OnJump.Subscribe(_ => source.PlayOneShot(jump));
        playerControl.OnSlimeToWall.Subscribe(_ => source.PlayOneShot(slime));
    }
}