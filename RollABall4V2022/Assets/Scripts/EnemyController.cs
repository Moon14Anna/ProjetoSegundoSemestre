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

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SetSphereRadius(float value)
    {
        _sphereCollider.radius = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jogador entrou na área");
        }
    }
}
