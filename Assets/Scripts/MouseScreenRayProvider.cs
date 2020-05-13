using UnityEngine;

public class MouseScreenRayProvider : MonoBehaviour, IRayProvider
{
    public Ray CreateRay(Camera mainCamera)
    {
        // Proyectar un rayo desde la posicion del mouse hacia el mundo 3D y devolver el resultado
        return mainCamera.ScreenPointToRay(Input.mousePosition);
    }
}