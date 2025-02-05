using UnityEngine;
using UnityEngine.VFX;

public class VFXAutoDestroy : MonoBehaviour
{
    VisualEffect effect;
    private void Awake()
    {
        effect = GetComponent<VisualEffect>();
    }

    private void Update()
    {
        if (effect.aliveParticleCount == 0)
        {
            effect.Stop();
            Destroy(this.gameObject);
        }
    }
}
