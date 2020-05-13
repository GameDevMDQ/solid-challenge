using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameController : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private PlayerController _playerController = null;

	private IUpdateBasedSpawner _itemSpawner = null;
	private IUpdateBasedSpawner _enemySpawner = null;
	
	// Propiedades
	public PlayerController GetPlayerController() { return _playerController; }
	
	protected void Awake()
	{
		// Esta es una forma crota de guardar las referencias a los spawners
		IUpdateBasedSpawner[] updateBasedSpawners = GetComponentsInChildren<IUpdateBasedSpawner>();
		_itemSpawner = updateBasedSpawners[0];
		_enemySpawner = updateBasedSpawners[1];
		
		Assert.IsNotNull(_itemSpawner, "Item Spawner reference not initialized.");
		Assert.IsNotNull(_itemSpawner, "Enemy Spawner reference not initialized.");
		Assert.IsNotNull(_playerController, "Player Controller reference not initialized.");
	}

	protected void Start()
	{
		_itemSpawner.Initialize(this);
		_enemySpawner.Initialize(this);
	}

	protected virtual void Update()
	{
		_itemSpawner.UpdateSpawning();
		_enemySpawner.UpdateSpawning();
	}
}