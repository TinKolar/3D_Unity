using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager instance;

    [Header("Sound Effects")]
    [SerializeField] public AudioClip bulletBounce;
    [SerializeField] public AudioClip bulletShot;
    [SerializeField] public AudioClip bulletExplosion;
    [SerializeField] public AudioClip playerDeath;
    [SerializeField] public AudioClip victory;
    [SerializeField] public AudioClip turretDeath;
    [SerializeField] public AudioClip keyDeath;

    //[SerializeField] public AudioClip objectDestroy;



    [Header("Particle Effects")]
    public ParticleSystem boomPS;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void MakeBoom(Transform target)
    {
        // Instantiate at position and rotation
        ParticleSystem ps = Instantiate(boomPS, target.position, Quaternion.identity);

        PlaySound(bulletExplosion, target.transform);

        // Destroy after the particle finishes
        Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
    }

    public void PlaySound(AudioClip sound, Transform transform)
    {
        if (sound == null || transform == null) return;

        AudioSource.PlayClipAtPoint(sound, transform.position);
    }

}