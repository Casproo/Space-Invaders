using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    
    
    private bool _isMissileActive;
    private Vector3 _lastPosition;
    private Vector3 _deathEnemyVector;
    private int _enemyCount = 33;
    
    public List<GameObject> pooledEnemies = new List<GameObject>();
    public float EnemySpeed = 0.1f;
    private float _enemySpeed = 0.1f;
    
    [SerializeField] private int missileSpawnRate = 2;
    [SerializeField] private Transform enemyParent;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        AddingEnemiesToList();
        InvokeRepeating(nameof(CheckEnemies),0.1f,0.1f);
    }
    

    private void AddingEnemiesToList()
    {
        pooledEnemies.Clear();
        Transform[] enemies = GetComponentsInChildren<Transform>();

        foreach (Transform enemy in enemies)
        {
            if (enemy.gameObject != gameObject)
            {
                pooledEnemies.Add(enemy.gameObject);
            }
        }
    }

    private void ShootMissile()
    {
        if (_enemyCount == 0)
        {
            return;
        }
        
        if (_enemyCount > 0)
        {
            GameObject missile = MissilePool.Instance.GetPooledGameObject();

            if (_isMissileActive || missile == null) return;
            
            int enemyToShoot = Random.Range(0, _enemyCount);
            _isMissileActive = true;
            missile.transform.position = pooledEnemies[enemyToShoot].transform.position;
            missile.SetActive(true);
        }
    }

    private void CheckEnemies()
    {
        _lastPosition = enemyParent.position;
        
        foreach (var enemy in pooledEnemies)
        {
            if (enemy.gameObject.transform.position.y <= -2.65f)
            {
                _lastPosition.x -= 0.3f;
                enemyParent.position = _lastPosition;
                EnemySpeed = -EnemySpeed;
                return;
            }
            
            if (enemy.gameObject.transform.position.y >= 2.65f)
            {
                _lastPosition.x -= 0.3f;
                enemyParent.position = _lastPosition;
                EnemySpeed = -EnemySpeed;
                return;
            }
            
        }

    }
    private void OnEnable()
    {
        Missile.MissileDestroyed += OnUpdateMissileActive;
        GameManager.EnemyKilled += OnEnemyKilled;
        GameManager.GameRestarted += OnRestartWave;
        GameManager.NextWave += OnNextWave;
        InvokeRepeating(nameof(ShootMissile),missileSpawnRate, missileSpawnRate);
    }
    private void OnDisable()
    {
        Missile.MissileDestroyed -= OnUpdateMissileActive;
        GameManager.EnemyKilled -= OnEnemyKilled;
        GameManager.GameRestarted -= OnRestartWave;
        GameManager.NextWave -= OnNextWave;
        CancelInvoke(nameof(ShootMissile));
    }

    private void OnUpdateMissileActive()
    {
        _isMissileActive = false;
    }

    private void OnNextWave()
    {
        _enemySpeed += 0.1f;
        _enemyCount = 33;
        AddingEnemiesToList();
        EnemySpeed = _enemySpeed;
        transform.position = new Vector3(2.6f, 0, 0);
        InvokeRepeating(nameof(ShootMissile),missileSpawnRate, missileSpawnRate);
    }

    private void OnRestartWave()
    {
        _enemyCount = 33;
        AddingEnemiesToList();
        EnemySpeed = _enemySpeed;
        transform.position = new Vector3(2.6f, 0, 0);
        InvokeRepeating(nameof(ShootMissile),missileSpawnRate, missileSpawnRate);
    }

    private void OnEnemyKilled()
    {
        _enemyCount--;
        
        if (EnemySpeed > 0)
        {
            EnemySpeed += 0.01f;
        }

        if (EnemySpeed < 0)
        {
            EnemySpeed -= 0.01f;
        }

        for (int i = 0; i < pooledEnemies.Count; i++)
        {
            if (pooledEnemies[i].GetComponent<Enemy>().GetInstanceID() == Laser.Instance.enemyID)
            {
                AudioManager.instance.Play("InvaderKilled");
                pooledEnemies.RemoveAt(i);
                if (missileSpawnRate >= 2)
                {
                    missileSpawnRate--;
                }
            }
        }
        if (pooledEnemies.Count == 1)
        {
            CancelInvoke(nameof(ShootMissile));
            if (EnemySpeed > 0)
            {
                EnemySpeed += 3.8f;
            }

            if (EnemySpeed < 0)
            {
                EnemySpeed -= 3.8f;
            }
        }

        if (pooledEnemies.Count == 0)
        {
            CancelInvoke(nameof(ShootMissile));
            GameManager.NextWave?.Invoke();
            GameManager.NextWave?.Invoke();
        }
    }
}