using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class PostProcessController : MonoBehaviour
{
    public Slider contrastSlider;
    public PostProcessVolume postProcessVolume;

    private ColorGrading _colorGrading;

    private Vector4 _lift;
    private Vector4 _gamma;
    private Vector4 _gain;

    private void Start()
    {
        postProcessVolume.profile.TryGetSettings(out _colorGrading);
        

        
        UpdateColorGrading(0);
    }

    private void Update()
    {
        contrastSlider.onValueChanged.AddListener(UpdateColorGrading);
    }

    private void UpdateColorGrading(float value)
    {
        _lift = new Vector4(0, 0, 0, value);  //Lift
        _gamma = new Vector4(0, 0, 0, -value); //Gamma
        _gain = new Vector4(0, 0, 0, value); //Gain

        _colorGrading.lift.value = _lift;
        _colorGrading.gamma.value = _gamma;
        _colorGrading.gain.value = _gain;
    }
}
