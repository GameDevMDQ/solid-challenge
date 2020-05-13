namespace Enemy
{
    using UnityEngine;
    using UnityEngine.AI;
    using UnityEngine.Assertions;

    public class EnemyController : MonoBehaviour
    {
        // Variables del Editor
        [SerializeField] private float _stoppingDistance = 2.0f;
        [SerializeField] private float _rotationSpeed = 10.0f;
        
        // Referencias
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;

        // Logica
        private EnemyStateMachine _enemyStateMachine;
        
        // Propiedadess
        public Transform Target { get; set; }

        /// <summary>
        /// Inicializa el estado de este enemigo.
        /// </summary>
        /// <param name="playerController">La referencia a un objeto Player Controller.</param>
        public void Initialize(PlayerController playerController)
        {   
            _enemyStateMachine = new EnemyStateMachine();
            _enemyStateMachine.Initialize(this, playerController);
        }

        // Comportamiento del enemigo
        
        /// <summary>
        /// Evento para cuando el enemigo empieza a permanecer quieto.
        /// </summary>
        public virtual void OnStartIdle()
        {
            _animator.SetBool("Walk", false);
            _animator.SetBool("Attack", false);
        }

        /// <summary>
        /// Evento para cuando el enemigo empieza a seguir a un objetivo.
        /// </summary>
        public virtual void OnStartFollowing()
        {
            _animator.SetBool("Walk", true);
            _animator.SetBool("Attack", false);
        }

        /// <summary>
        /// Evento para cuando el enemigo comienza a atacar.
        /// </summary>
        public virtual void OnStartAttacking()
        {
            _animator.SetBool("Attack", true);
        }

        /// <summary>
        /// Evento para cuando el enemigo golpea con la espada.
        /// </summary>
        public virtual void OnSwordHit()
        {
            // Este metodo se llama desde un evento asignado en la animación Attack del Animator (UD_infantry_07_attack_A)
            
            Debug.Log("El enemigo pegó con éxito.");
        }

        /// <summary>
        /// Actualiza la posicion actual del jugador en el sistema de Path Finding (Nav Mesh Agent)
        /// </summary>
        public void UpdateAITargetPosition()
        {
            _navMeshAgent.SetDestination(Target.position);
        }

        /// <summary>
        /// Devuelve un valor que indica si el enemigo está cerca del enemigo basandose en el coeficiente de cercania (_stoppingDistance)
        /// </summary>
        public bool IsCloseToTheTarget()
        {
            // Calculamos la distancia actual hasta objetivo 
            float currentDistance = Vector3.Distance(transform.position, Target.position);

            // Devuelve true si está dentro del rango de proximidad
            return currentDistance < _stoppingDistance;
        }
        
        /// <summary>
        /// Rotates this enemy to look at target
        /// </summary>
        public void RotateTowardsTarget()
        {
            Vector3 direction = (Target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);
        }

        protected void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        
            Assert.IsNotNull(_navMeshAgent, "NavMeshAgent Component cannot be null.");
            Assert.IsNotNull(_animator, "Animator Component cannot be null.");

            // Le asigna al Nav Mesh Agent el coeficiente de cercanía 
            _navMeshAgent.stoppingDistance = _stoppingDistance;
        }

        protected void Update()
        {
            _enemyStateMachine.UpdateStateMachine();
        }
    }
}
