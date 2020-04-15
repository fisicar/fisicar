using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class PostProcessController : MonoBehaviour
{
    public Toggle contrastToggle;
    public PostProcessVolume postProcessVolume;

    private ColorGrading _colorGrading;

    private Vector4 _lift;
    private Vector4 _gamma;
    private Vector4 _gain;

    private void Start()
    {
        postProcessVolume.profile.TryGetSettings(out _colorGrading);
        

        
        UpdateColorGrading(false);
    }

    private void Update()
    {
        contrastToggle.onValueChanged.AddListener(UpdateColorGrading);
    }

    private void UpdateColorGrading(bool boolValue)
    {
        var value = boolValue ? 1 : 0;

        _lift = new Vector4(0, 0, 0, value);  //Lift
        _gamma = new Vector4(0, 0, 0, -value); //Gamma
        _gain = new Vector4(0, 0, 0, value); //Gain

        _colorGrading.lift.value = _lift;
        _colorGrading.gamma.value = _gamma;
        _colorGrading.gain.value = _gain;
    }
}
