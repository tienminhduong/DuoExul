using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleEffect : MonoBehaviour, IVfxEffect
{
    [SerializeField] private VfxType type;
    private ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        if(particle == null)
        {
            Debug.LogError("ParticleEffect requires a ParticleSystem component.");
            enabled = false;
        }
    }
    public bool IsFinished => !particle.IsAlive(true);

    public void Play(Vector3 position, Quaternion rotation)
    {
        this.transform.SetPositionAndRotation(position, rotation);
        gameObject.SetActive(true);
        particle.Play();
    }

    private void OnParticleSystemStopped()
    {
        VFXSystem.Instance?.ReturnPool(this.type, this);
    }
    public void Stop()
    {
        particle.Stop();
    }
}
