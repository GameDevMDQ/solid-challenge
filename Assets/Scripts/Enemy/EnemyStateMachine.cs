using System;
using System.Collections.Generic;

namespace Enemy
{
    public class EnemyStateMachine
    {
        public IEnemyState CurrentState { get; private set; }

        private Dictionary<Type, IEnemyState> _turretStates;

        public void Initialize(EnemyController enemyController, PlayerController playerController)
        {
            // Initialize the states
            _turretStates = new Dictionary<Type, IEnemyState>()
            {
                { typeof(EnemyIdleState), new EnemyIdleState(enemyController, playerController) },
                { typeof(EnemyFollowingState), new EnemyFollowingState(enemyController) },
                { typeof(EnemyAttackingState), new EnemyAttackingState(enemyController) }
            };

            ChangeState(typeof(EnemyIdleState));
        }

        public void UpdateStateMachine()
        {
            if (CurrentState == null)
            {
                return;
            }

            Type newState = CurrentState.UpdateState();

            if (newState != null)
            {
                ChangeState(newState);
            }
        }

        private void ChangeState(Type newStateType)
        {
            // Cambiar al nuevo estado
            CurrentState = _turretStates[newStateType];
            
            // Iniciar el nuevo estado
            CurrentState.StartState();
        }

    }
}