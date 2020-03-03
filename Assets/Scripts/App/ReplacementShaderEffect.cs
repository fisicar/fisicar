using UnityEngine;

public class ReplacementShaderEffect : MonoBehaviour
{
    public Shader replacementShader;

    private void OnEnable()
    {
        if (replacementShader != null)
        {
            GetComponent<Camera>().SetReplacementShader(replacementShader, "RenderType");
        }
    }

    private void OnDisable()
    {
        GetComponent<Camera>().SetReplacementShader(null, null);
    }
}
