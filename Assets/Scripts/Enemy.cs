using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public static Enemy Instance;
    
    private int _instanceID;
    private Vector3 _startPosition;
    private Vector3 _lastPosition;
    [SerializeField] private Transform explosion;
    
    public int shieldID;

    public static UnityAction ShieldDestroyed;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        _startPosition = transform.position;
        _instanceID = GetInstanceID();
    }


    private void Update()
    {
        _lastPosition = transform.position;
        _lastPosition.y += -EnemyManager.Instance.EnemySpeed * Time.deltaTime;
        transform.position = _lastPosition;
    }

    private void OnEnable()
    {
        Laser.LaserDestroyed += OnEnemyDamaged;
        GameManager.NextWave -= OnEnemyActivated;
        GameManager.GameRestarted -= OnEnemyActivated;
        GameManager.GameRestarted += OnEnemyLocationResetted;
        GameManager.NextWave += OnEnemyLocationResetted;
    }

    private void OnDisable()
    {
        Laser.LaserDestroyed -= OnEnemyDamaged;
        GameManager.NextWave += OnEnemyActivated;
        GameManager.GameRestarted += OnEnemyActivated;
        GameManager.GameRestarted += OnEnemyLocationResetted;
        GameManager.NextWave += OnEnemyLocationResetted;
    }

    private void OnDestroy()
    {
        GameManager.GameRestarted -= OnEnemyLocationResetted;
        GameManager.NextWave -= OnEnemyLocationResetted;
    }

    private void OnEnemyDamaged()
    {
        if (_instanceID == Laser.Instance.enemyID && explosion)
        {
            GameObject exploder = ((Transform)Instantiate(explosion, this.transform.position, this.transform.rotation))
                .gameObject;
            Destroy(exploder, 1f);
            gameObject.SetActive(false);
            enabled = false;
        }
    }
    private void OnEnemyActivated()
    {
        gameObject.SetActive(true);
        enabled = true;
    }

    public void OnEnemyLocationResetted()
    {
        transform.position = _startPosition;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            GameManager.GameOverScreen?.Invoke();
        }

        if (col.gameObject.CompareTag("Edge"))
        {
            GameManager.GameRestarted?.Invoke();
        }

        if (col.gameObject.CompareTag("Shields"))
        {
            shieldID = col.gameObject.GetComponent<Shield>().GetInstanceID();
            ShieldDestroyed?.Invoke();
            shieldID = 0;
        }
    }
}