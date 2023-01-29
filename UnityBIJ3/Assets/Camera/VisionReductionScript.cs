
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VisionReductionScript : MonoBehaviour
{
    [SerializeField] Volume globalVolume;

    private SideEffectManager sideEffectManager;
    private Vignette vignette;

    private void Start()
    {
        sideEffectManager = SideEffectManager.Instance;
        globalVolume.profile.TryGet<Vignette>(out vignette);
    }

    private void Update()
    {
        if (vignette != null)
        {
            var intensity = 1 - sideEffectManager.GetVision();
            vignette.intensity.value = intensity;
        }
    }
}
