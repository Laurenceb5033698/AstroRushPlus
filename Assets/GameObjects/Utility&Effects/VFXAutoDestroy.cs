using UnityEngine;
using UnityEngine.VFX;

public class VFXAutoDestroy : MonoBehaviour
{
    VisualEffect effect;
    [SerializeField] private float vfxLifetime = 5f;
    private void Awake()
    {
        effect = GetComponent<VisualEffect>();
    }

    private void Update()
    {
        vfxLifetime -= Time.deltaTime;
        if (effect.aliveParticleCount == 0 || vfxLifetime <= 0)
        {
            effect.Stop();
            Destroy(this.gameObject);
        }
    }
}
