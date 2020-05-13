using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HighlightSelection : MonoBehaviour, ISelection
{
    [SerializeField] public Material _highlightMaterial = null;

    public void PerformDeselection(Transform currentSelection)
    {
        var selectionRenderer = currentSelection.GetComponentInChildren<Renderer>();
        if (selectionRenderer)
        {
            List<Material> materials = selectionRenderer.sharedMaterials.ToList();
            materials.Remove(this._highlightMaterial);
            selectionRenderer.materials = materials.ToArray();
        }
    }

    public void PerformSelection(Transform currentSelection)
    {
        var selectionRenderer = currentSelection.GetComponentInChildren<Renderer>();
        if (selectionRenderer)
        {
            List<Material> materials = selectionRenderer.sharedMaterials.ToList();
            materials.Add(this._highlightMaterial);
            selectionRenderer.materials = materials.ToArray();
        }
    }
}