using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{ 
    [SerializeField] private int shieldHealth = 100;
    [SerializeField] private List<Sprite> shieldSprites = new List<Sprite>();
    private int _spriteCount = 5;
    [SerializeField] private SpriteRenderer currentSprite;
    private int _instanceID;

    private void Awake()
    {
        _instanceID = GetInstanceID();
    }

    private void OnEnable()
    {
        Laser.LaserDestroyed += OnShieldDamaged;
        Missile.MissileDestroyed += OnShieldDamaged;
        GameManager.GameRestarted -= OnResetShield;
        GameManager.NextWave -= OnResetShield;
        Enemy.ShieldDestroyed += OnShieldDestroyed;
    }

    private void OnDisable()
    {
        Laser.LaserDestroyed -= OnShieldDamaged;
        Missile.MissileDestroyed -= OnShieldDamaged;
        GameManager.GameRestarted += OnResetShield;
        GameManager.NextWave += OnResetShield;
        Enemy.ShieldDestroyed -= OnShieldDestroyed;
    }

    private void OnShieldDamaged()
    {
        if (_instanceID == Laser.Instance.shieldID || _instanceID == Missile.Instance.shieldID)
        {
            _spriteCount--;
            currentSprite.sprite = shieldSprites[_spriteCount];
            shieldHealth -= 20;

            if (shieldHealth == 0)
            {
                gameObject.SetActive(false);
                enabled = false;
            }
        }
    }

    private void OnResetShield()
    {
        _spriteCount = 5;
        currentSprite.sprite = shieldSprites[5];
        shieldHealth = 100;
        gameObject.SetActive(true);
        enabled = true;
    }

    private void OnShieldDestroyed()
    {
        if (_instanceID == Enemy.Instance.shieldID)
        {
            shieldHealth = 0;
            gameObject.SetActive(false);
            enabled = false;
        }
    }
}