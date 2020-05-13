using UnityEngine;

public interface ISelection
{
    void PerformDeselection(Transform currentSelection);
    void PerformSelection(Transform currentSelection);
}