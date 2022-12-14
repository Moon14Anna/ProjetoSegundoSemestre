using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public PatrolRouteManager myPatrolRoute;
    public EnemyDataSO enemyData;
    
    private float _moveSpeed;
        
    private int _maxHealthPoints;
        
    private GameObject _enemyMesh;

    public float FollowDistance => _followDistance;
    public float _followDistance; // x = distancia minima pro inimigo seguir o jogador
        
    public float ReturnDistance => _returnDistance;
    public float _returnDistance; // z = distancia maxima pro inimigo seguir o jogador
        
    public float AttackDistance => _attackDistance;
    public float _attackDistance; // y = distancia minima pro inimigo atacar o jogador
    
    public float GiveUpDistance => _giveUpDistance;
    public float _giveUpDistance; // w = distancia maxima pro inimigo atacar o jogador

    private int _currentHealthPoints;

    private float _currentMoveSpeed;

    private Animator _enemyFSM;

    private NavMeshAgent _navMeshAgent;

    private SphereCollider _sphereCollider;

    private Transform _playerTransform;

    private Transform _currentPatrolPoint;

    private Transform _nextPatrolPoint;

    private int _currentePatrolIndex;

    // Start is called before the first frame update
    private void Awake()
    {
        _moveSpeed = enemyData.moveSpeed;
        _maxHealthPoints = enemyData.maxHealthPoints;

        //_enemyMesh = Instantiate(enemyData.enemyMesh, this.transform);
        
        _followDistance = enemyData.followDistance;
        _returnDistance = enemyData.returnDistance;
        _attackDistance = enemyData.attackDistance;
        _giveUpDistance = enemyData.giveUpDistance;

        _currentHealthPoints = _maxHealthPoints;
        _currentMoveSpeed = _moveSpeed;

        _enemyFSM = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _sphereCollider = GetComponent<SphereCollider>();

    }
    
    void Start()
    {
        _currentePatrolIndex = 0; //define a primeira posi????o da patrulha
        _currentPatrolPoint = myPatrolRoute.patrolRoutePoints[_currentePatrolIndex]; //define onde encontrar os pontos (na lista)
    }

    public void Patrulhar()
    {
        _navMeshAgent.destination = myPatrolRoute.patrolRoutePoints[_currentePatrolIndex].position; //diz onde o inimigo vai encontrar o destino dele
        _currentePatrolIndex++; //adiciona mais um, para passar para o pr??ximo ponto da lista

        if (_currentePatrolIndex == 4) //faz com que ao chegar no ??ltimo ponto, volte para o primeiro
        { 
            _currentePatrolIndex = 0;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (_enemyFSM.GetCurrentAnimatorStateInfo(0).IsName("Return"))
        {
            _enemyFSM.SetFloat("ReturnDistance", 
                Vector3.Distance(transform.position, _currentPatrolPoint.position));
        }

        if (_enemyFSM.GetCurrentAnimatorStateInfo(0).IsName("Patrol")) //checa se ele est?? patrulhando ou n??o
        {
            if (_navMeshAgent.remainingDistance < 0.1f) //verifica dist??ncia entre o inimigo e o ponto de destino 
            {
                Patrulhar();
            }
        }
    }
    
    public void SetSphereRadius(float value)
    {
        _sphereCollider.radius = value;
    }

    public void SetDestinationToPlayer()
    {
        //transform.position += (_playerTransform.position - transform.position).normalized * _moveSpeed * Time.deltaTime;
        _navMeshAgent.SetDestination(_playerTransform.position);
        _currentePatrolIndex = 0;
    }

    public void SetDestinationToPatrol()
    {
        _navMeshAgent.SetDestination(_currentPatrolPoint.position);
    }

    public void ResetPlayerTransform()
    {
        _playerTransform = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerTransform = other.transform;
            _enemyFSM.SetTrigger("OnPlayerEntered");
            Debug.Log("Jogador entrou na ??rea");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        _enemyFSM.SetTrigger("OnPlayerExited");
        Debug.Log("Jogador saiu da ??rea");
    }
    
}
