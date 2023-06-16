using UnityEngine;
using UnityEngine.Events;

public class Missile : MonoBehaviour
{
    public static Missile Instance;
    
    [SerializeField] private float missileSpeed = 0.7f;
    
    private Vector3 _lastPosition;
    
    public static UnityAction MissileDestroyed;
    public int shieldID;

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
        _lastPosition.x -= missileSpeed * Time.deltaTime;
        transform.position = _lastPosition;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Edge"))
        {
            gameObject.SetActive(false);
            MissileDestroyed?.Invoke();
            shieldID = 0;
        }
        if (col.CompareTag("Shields"))
        {
            gameObject.SetActive(false);
            shieldID = col.gameObject.GetComponent<Shield>().GetInstanceID();
            MissileDestroyed?.Invoke();
            shieldID = 0;
        }
        if (col.CompareTag("Laser"))
        {
            gameObject.SetActive(false);
            MissileDestroyed?.Invoke();
            shieldID = 0;
        }
    }
}