using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Laser : MonoBehaviour
{
    public static Laser Instance;
    
    [SerializeField] private float laserSpeed = 1f;
    
    private Vector3 _lastPosition;
    
    public static UnityAction LaserDestroyed;
    public int shieldID;
    public int enemyID;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        MoveLaser();
    }

    private void MoveLaser()
    {
        _lastPosition = transform.position;
        _lastPosition.x += laserSpeed * Time.deltaTime;
        transform.position = _lastPosition;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Edge"))
        {
            gameObject.SetActive(false);
            LaserDestroyed?.Invoke();
        }
        if (col.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
            enemyID = col.gameObject.GetComponent<Enemy>().GetInstanceID();
            LaserDestroyed?.Invoke();
            GameManager.EnemyKilled?.Invoke();
            enemyID = 0;
        }
        if (col.CompareTag("Mystery"))
        {
            gameObject.SetActive(false);
            LaserDestroyed?.Invoke();
            GameManager.MysteryKilled?.Invoke();
        }
        if (col.CompareTag("Missile"))
        {
            gameObject.SetActive(false);
            LaserDestroyed?.Invoke();
            Missile.MissileDestroyed?.Invoke();
        }
        if (col.CompareTag("Shields"))
        {
            gameObject.SetActive(false);
            shieldID = col.gameObject.GetComponent<Shield>().GetInstanceID();
            LaserDestroyed?.Invoke();
            shieldID = 0;

        }
        // MainMenu Colliders

        if (col.CompareTag("Play"))
        {
            SceneManager.LoadScene(1);
        }

        if (col.CompareTag("Quit"))
        {
            Application.Quit();
        }
    }
    
    
}