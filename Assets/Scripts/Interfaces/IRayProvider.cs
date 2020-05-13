using UnityEngine;

public interface IRayProvider
{
    Ray CreateRay(Camera mainCamera);
}