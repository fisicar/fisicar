using System.Diagnostics.SymbolStore;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class PostProcessController : MonoBehaviour
{
    public Toggle contrastToggle;
    public Toggle invertColorToggle;
    public PostProcessVolume postProcessVolume;

    private ColorGrading _colorGrading;

    private Vector4 _lift;
    private Vector4 _gamma;
    private Vector4 _gain;

    private void Awake()
    {
        postProcessVolume.profile.TryGetSettings(out _colorGrading);
        
        contrastToggle.onValueChanged.AddListener(UpdateColorContrast);
        invertColorToggle.onValueChanged.AddListener(UpdateColorInvert);

        UpdateColorContrast(false);
    }

    private void UpdateColorInvert(bool arg0)
    {
        _colorGrading.hueShift.value = arg0 ? 180 : 0;
    }

    private void UpdateColorContrast(bool boolValue)
    {
        var value = boolValue ? 1 : 0;

        _colorGrading.lift.value = new Vector4(0, 0, 0, value);
        _colorGrading.gamma.value = new Vector4(0, 0, 0, -value);
        _colorGrading.gain.value = new Vector4(0, 0, 0, value);
    }
}
