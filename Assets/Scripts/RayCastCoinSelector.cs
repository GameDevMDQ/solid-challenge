using System;
using UnityEngine;

public class RayCastCoinSelector : MonoBehaviour, ISelector
{
    [SerializeField] private float _collectionRadius = 4.0f;

    private int _layerMaskItems;
    
    protected void Awake()
    {
        _layerMaskItems = LayerMask.GetMask("Item");
    }

    public CoinBehavior Check(Ray ray)
    {
        // Proyectar un rayo que busca solo en la capa Items
        if (Physics.Raycast(ray, out RaycastHit itemHoverHit, _layerMaskItems))
        {
            // Pedir el componente CoinBehavior a cualquier cosa contra lo que haya pegado el rayo
            CoinBehavior selectedCoin = itemHoverHit.collider.GetComponent<CoinBehavior>();
			
            // Si el componente CoinBehavior fue encontrado
            if (selectedCoin != null)
            {
                // Revisar si se encontraba en la distancia minima para seleccionar
                if (Vector3.Distance(transform.position, selectedCoin.transform.position) <  _collectionRadius)
                {
                    // Retornar la moneda encontrada
                    return selectedCoin;
                }
            }
        }

        return null;
    }
}