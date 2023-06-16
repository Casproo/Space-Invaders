using UnityEngine;

public class Mystery : MonoBehaviour
{
    private Vector3 _lastPosition;
    [SerializeField] private float mysterySpeed = 0.25f;
    [SerializeField] private Transform explosion;
    
    private void Update()
    {
        _lastPosition = transform.position;
        _lastPosition.y += mysterySpeed * Time.deltaTime;
        transform.position = _lastPosition;
        
        if (_lastPosition.y >= 2.8f)
        {
            gameObject.SetActive(false);
            transform.position = new Vector3(4.25f, -2.8f, 0);
        }
    }

    private void OnEnable()
    {
        GameManager.MysteryKilled += OnMysteryKilled;
        GameManager.GameRestarted += OnMysteryResetted;
    }

    private void OnDisable()
    {
        GameManager.MysteryKilled -= OnMysteryKilled;
        GameManager.GameRestarted -= OnMysteryResetted;
    }

    private void OnMysteryKilled()
    {
        if (explosion)
        {
            GameObject exploder = ((Transform)Instantiate(explosion, this.transform.position, this.transform.rotation))
                .gameObject;
            Destroy(exploder, 1f);
        }

        gameObject.SetActive(false);
        transform.position = new Vector3(4.25f, -2.8f, 0);
    }

    private void OnMysteryResetted()
    {
        gameObject.SetActive(false);
        transform.position = new Vector3(4.25f, -2.8f, 0);
    }
}