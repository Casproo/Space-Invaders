using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    
    [SerializeField] private float speed = 0.35f;
    [SerializeField] private Transform laserPosition;
    [SerializeField] private SpriteRenderer currentSprite;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite downSprite;
    [SerializeField] private Sprite upSprite;
    
        
    private Vector3 _lastPosition;
    private float _verticalInput;
    private bool _isLaserActive;
    
    public bool isAbleToMove = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        MovePlayer();
        ShootLaser();
    }

    private void OnEnable()
    {
        Laser.LaserDestroyed += OnUpdateLaserActive;
        GameManager.PlayerKilled += OnStopPlayer;
        GameManager.GameOverScreen += OnStopPlayer;
    }

    private void OnDisable()
    {
        Laser.LaserDestroyed -= OnUpdateLaserActive;
        GameManager.PlayerKilled -= OnStopPlayer;
        GameManager.GameOverScreen += OnStopPlayer;
    }

    private void OnDestroy()
    {
        GameManager.GameOverScreen -= OnStopPlayer;
    }

    private void MovePlayer()
    {
        if (!isAbleToMove) return;
        
        _lastPosition = transform.position;
        _verticalInput = Input.GetAxis("Vertical");

        if (_verticalInput == 0)
        {
            currentSprite.sprite = idleSprite;
        }

        switch (_verticalInput)
        {
            case < 0f:
                currentSprite.sprite = downSprite;
                _lastPosition.y -= speed * Time.deltaTime;
                break;
            case > 0f:
                currentSprite.sprite = upSprite;
                _lastPosition.y += speed * Time.deltaTime;
                break;
        }

        _lastPosition.y = Mathf.Clamp(_lastPosition.y, -2.60f, 2.60f);
        transform.position = _lastPosition;
    }

    private void ShootLaser()
    {
        if (!Input.GetKeyDown(KeyCode.Space) && (!Input.GetMouseButtonDown(0) || !isAbleToMove)) return;
        
        GameObject laser = LaserPool.Instance.GetPooledGameObject();

        if (_isLaserActive || laser == null) return;
        
        _isLaserActive = true;
        laser.transform.position = laserPosition.position;
        laser.SetActive(true);
        AudioManager.instance.Play("Shoot");
    }

    private void OnUpdateLaserActive()
    {
        _isLaserActive = false;
    }

    private void OnStopPlayer()
    {
        isAbleToMove = false;
        _isLaserActive = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Missile"))
        {
            GameManager.PlayerKilled.Invoke();
        }
    }
}