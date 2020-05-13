using Enemy;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour, IUpdateBasedSpawner
{
    [Header("References")]
    [SerializeField] private EnemyController _enemyPrefab = null;
    [Header("Settings")]
    [SerializeField] private float _enemySpawningInterval = 10.0f;
    [SerializeField] private float _minSpawnDistance = 10.0f;
    [SerializeField] private float _maxSpawnDistance = 30.0f;
    
    // Referencias
    private PlayerController _playerController;
    
    // Logica
    private float _lastTimeSpawnedEnemy;
    private int _walkableLayerMaskIndex;

    public void Initialize(GameController gameController)
    {
        _playerController = gameController.GetPlayerController();
        
        _walkableLayerMaskIndex = (1 << NavMesh.GetAreaFromName("Walkable"));
    }

    public void UpdateSpawning()
    {
        if (Time.realtimeSinceStartup - _lastTimeSpawnedEnemy > _enemySpawningInterval)
        {
            SpawnItem();
        }
    }

    private void SpawnItem()
    {
        // La posicion actual del player
        Vector3 playerPosition = _playerController.GetPlayerTransform().position;

        // Generamos al azar un 1 o un -1
        int directionChanger = Random.Range(0, 2) * 2 - 1;
        
        // Generamos al un numero al azar para X entre la distancia minima y maxima y lo multiplicamos por el coeficiente de variacion.
        float offsetX = Random.Range(_minSpawnDistance, _maxSpawnDistance) * directionChanger;
        
        // Volvemos a generar el coeficiente de variacion
        directionChanger = Random.Range(0, 2) * 2 - 1;
        
        // Generamos al un numero al azar para Y entre la distancia minima y maxima y lo multiplicamos por el coeficiente de variacion.
        float offsetZ = Random.Range(_minSpawnDistance, _maxSpawnDistance) * directionChanger;
        
        // Ahora a la posicion del player, le sumamos estos valores generados 
        float spawnPositionX = playerPosition.x + offsetX;
        float spawnPositionZ = playerPosition.z + offsetZ;
        Vector3 spawnPosition = new Vector3(spawnPositionX, 0, spawnPositionZ);

        // Pedimos un punto en el NavMesh en base a la posicion anteriormente creada
        NavMeshHit hit;
        NavMesh.SamplePosition(spawnPosition, out hit, 50f, _walkableLayerMaskIndex);

        // Creamos el nuevo enemigo en la posicion calculada
        EnemyController spawnedEnemy = Instantiate(_enemyPrefab, hit.position, Quaternion.identity);
        
        // Lo hacemos hijo de este spawner
        spawnedEnemy.transform.SetParent(this.transform, true);
        
        // Inicializamos el enemigo creado, pasandole las referencias que necesita
        spawnedEnemy.Initialize(_playerController);

        // Actualizamos el tiempo de la ultima vez que se creó un enemigo
        _lastTimeSpawnedEnemy = Time.realtimeSinceStartup;
    }
}