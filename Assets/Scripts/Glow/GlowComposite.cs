using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class GlowComposite : MonoBehaviour
{
    [Range(0, 10)]
    public float Intensity = 2;

    private Material _compositeMat;

    private void OnEnable()
    {
        _compositeMat = new Material(Shader.Find("Hidden/GlowComposite"));
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        _compositeMat.SetFloat("_Intensity", Intensity);
        Graphics.Blit(src, dst, _compositeMat, 0);
    }
}