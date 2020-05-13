using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class ItemSpawner : MonoBehaviour, IUpdateBasedSpawner
{
    [SerializeField] private GameObject _itemPrefab = null;
    [SerializeField] private float _itemSpawningInterval = 5.0f;

    private float _lastTimeSpawnedItem;
    private Transform _playerTransform;
    private int _walkableLayerMaskIndex;

    public void Initialize(GameController gameController)
    {
        _playerTransform = gameController.GetPlayerController().GetPlayerTransform();
        
        _walkableLayerMaskIndex = (1 << NavMesh.GetAreaFromName("Walkable"));
    }

    public void UpdateSpawning()
    {
        // Revisar si ya es tiempo de spawnear un item
        if (Time.realtimeSinceStartup - _lastTimeSpawnedItem > _itemSpawningInterval)
        {
            SpawnItem();
        }
    }

    private void SpawnItem()
    {
        // La posicion actual del player
        Vector3 playerPosition = _playerTransform.position;

        // Calcular un punto al azar cerca del player
        float spawnPositionX = playerPosition.x + Random.Range(-15f, 15f);
        float spawnPositionZ = playerPosition.z + Random.Range(-15f, 15f);
        Vector3 spawnPosition = new Vector3(spawnPositionX, 0, spawnPositionZ);

        // Obtener una posición en el NavMesh lo más cercana posible a la posición al azar anteriormente generada
        NavMeshHit hit;
        NavMesh.SamplePosition(spawnPosition, out hit, 50f, _walkableLayerMaskIndex);

        // Subir un poquito más la altura sobre el NavMesh
        hit.position = new Vector3(hit.position.x, hit.position.y + .1f, hit.position.z);

        // Crear el nuevo item en esta última posición
        Instantiate(_itemPrefab, hit.position, Quaternion.identity).transform.SetParent(transform, true);
        
        // Actualizar el tiempo que indica cuando fue la última vez que se creó un item
        _lastTimeSpawnedItem = Time.realtimeSinceStartup;
    }
}