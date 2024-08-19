using UnityEngine;

public class sortlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
 public int sortingOrder = 100;
 public Renderer vfxRenderer;
 public string layer;

    private void OnValidate() {
        vfxRenderer = GetComponent<Renderer>();
        if (vfxRenderer) {
            vfxRenderer.sortingOrder = sortingOrder;
            vfxRenderer.sortingLayerName = layer;

        }
    }
}
