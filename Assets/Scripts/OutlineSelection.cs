using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OutlineSelection : MonoBehaviour, ISelection
{
    public void PerformDeselection(Transform currentSelection)
    {
        var outline = currentSelection.GetComponent<Outline>();
        if (outline != null)
        {
            outline.OutlineWidth = 0.0f;
        }
    }

    public void PerformSelection(Transform currentSelection)
    {
        var outline = currentSelection.GetComponent<Outline>();
        if (outline != null)
        {
            outline.OutlineWidth = 20.0f;
        }
        else
        {
            currentSelection.gameObject.AddComponent<Outline>();
        }
    }
}