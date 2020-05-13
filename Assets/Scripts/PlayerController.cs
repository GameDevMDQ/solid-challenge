using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class PlayerController : MonoBehaviour
{
	private NavMeshAgent _navMeshAgent;
	private Animator _animator;

	private int _layerMaskGround;
	
	private Camera _camera;
	private Transform _currentSelection;
	private IRayProvider _mouseScreenRayProvider;
	private ISelection _highlightSelection;
	private ISelector _coinSelector;
	
	public Transform GetPlayerTransform() { return transform; }

	protected virtual void Awake()
	{
		_layerMaskGround = LayerMask.GetMask("Ground");

		_camera = Camera.main;
		_navMeshAgent = GetComponent<NavMeshAgent>();
		_animator = GetComponentInChildren<Animator>();
		_mouseScreenRayProvider = GetComponent<IRayProvider>();
		_highlightSelection = GetComponent<ISelection>();
		_coinSelector = GetComponent<ISelector>();

		Assert.IsNotNull(_camera, "Camera Component not initialized.");
		Assert.IsNotNull(_navMeshAgent, "NavMeshAgent Component not initialized.");
		Assert.IsNotNull(_animator, "Animator Component not initialized.");
		Assert.IsNotNull(_mouseScreenRayProvider, "MouseScreenRayProvider Component not initialized.");
		Assert.IsNotNull(_highlightSelection, "HighlightSelectionManager Component not initialized.");
		Assert.IsNotNull(_coinSelector, "RayCastCoinSelector Component not initialized.");
	}

	protected virtual void Update()
	{
		// Revisar si el jugador ya está cerca del objetivo.
		if (_navMeshAgent.remainingDistance < _navMeshAgent.stoppingDistance)
		{
			// Hacer que deje de correr
			_animator.SetBool("Running", false);
		}

		// Revisar si el jugador está intentando interactuar con algo via mouse
		CheckInteraction();

		// Revisar si el jugador está seleccionado algo al poner el mouse encima
		CheckSelection();
	}

	private void CheckInteraction()
	{
		if (Input.GetMouseButtonDown(0))
		{
			// Crear un rayo hacia el mundo 3D desde la posicion del mouse
			Ray ray = _mouseScreenRayProvider.CreateRay(_camera);
	
			// Revisar si el jugador hizo click sobre una moneda
			var selectedCoin = _coinSelector.Check(ray);

			// Si hizo click, recolectar esa moneda y dejar verificar interacción con otras cosas
			if (selectedCoin != null)
			{
				// Seleccionar la moneda
				selectedCoin.Collect();
				return;
			}
			
			// Revisar si el jugador hizo click sobre el piso
			if (Physics.Raycast(ray, out RaycastHit hit, _layerMaskGround))
			{
				// Indicar cual es el nuevo objetivo del jugador
				_navMeshAgent.SetDestination(hit.point);

				// Iniciar animación de correr
				_animator.SetBool("Running", true);
			}
		}
	}

	private void CheckSelection()
	{
		// Deseleccionar cualquier seleccion ya existente
		if (_currentSelection != null)
		{
			_highlightSelection.PerformDeselection(_currentSelection);
		}

		_currentSelection = null;

		// Crear un rayo hacia el mundo 3D desde la posicion del mouse
		Ray ray = _mouseScreenRayProvider.CreateRay(_camera);
		
		// Revisar si se está apoyando el mouse sobre una moneda
		var selectedCoin = _coinSelector.Check(ray);

		// Si se apoyó, guardar la referencia a esa moneda
		if (selectedCoin != null)
		{
			_currentSelection = selectedCoin.transform;
		}

		// Si se encontró algo, marcarlo como seleccionado
		if (_currentSelection != null)
		{
			_highlightSelection.PerformSelection(_currentSelection);
		}
	}
}