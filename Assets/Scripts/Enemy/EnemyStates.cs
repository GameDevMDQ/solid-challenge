namespace Enemy
{
	using System;

	public interface IEnemyState
	{
		void StartState();
		
		Type UpdateState();
	}

	public class EnemyIdleState : IEnemyState
	{
		private readonly EnemyController _enemyController;
		private readonly PlayerController _playerController;

		public EnemyIdleState(EnemyController enemyController, PlayerController playerController)
		{
			_enemyController = enemyController;
			_playerController = playerController;
		}

		public void StartState()
		{
			_enemyController.OnStartIdle();
		}

		public Type UpdateState()
		{
			// Si hay un player objetivo, empezar a seguirlo
			if (_playerController != null)
			{
				_enemyController.Target = _playerController.transform;
				
				return typeof(EnemyFollowingState);
			}

			return null;
		}
	}

	public class EnemyFollowingState : IEnemyState
	{
		private EnemyController _enemyController;

		public EnemyFollowingState(EnemyController enemyController)
		{
			_enemyController = enemyController;
		}

		public void StartState()
		{
			_enemyController.OnStartFollowing();
		}

		public Type UpdateState()
		{
			if (_enemyController.Target == null)
			{
				return typeof(EnemyIdleState);
			}
			
			// Actualizar la posicion del player para los calculos de IA
			_enemyController.UpdateAITargetPosition();

			// Si el enemigo se acercó lo suficiente al player, atacarlo
			if (_enemyController.IsCloseToTheTarget())
			{
				return typeof(EnemyAttackingState);
			}
			
			return null;
		}
	}

	public class EnemyAttackingState : IEnemyState
	{
		private EnemyController _enemyController;

		public EnemyAttackingState(EnemyController enemyController)
		{
			_enemyController = enemyController;
		}

		public void StartState()
		{
			_enemyController.OnStartAttacking();
		}

		public Type UpdateState()
		{
			if (_enemyController.Target == null)
			{
				return typeof(EnemyIdleState);
			}
			
			// El enemigo rota siempre mirando hacia el objetivo
			_enemyController.RotateTowardsTarget();
			
			// Actualizar la posicion del player para los calculos de IA
			_enemyController.UpdateAITargetPosition();

			// Si el enemigo se alejo del player, empezar a seguirlo
			if (!_enemyController.IsCloseToTheTarget())
			{
				return typeof(EnemyFollowingState);
			}

			return null;
		}

		
	}
}