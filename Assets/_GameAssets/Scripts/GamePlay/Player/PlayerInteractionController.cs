using UnityEngine;

public class PlayerInteractionController : MonoBehaviour

{
    [SerializeField] private Transform playerVisualTransform;

    private PlayerController _playerController;
    private Rigidbody _playerRigidbody;


    private void Awake() 
    {
        _playerController = GetComponent<PlayerController>();
        _playerRigidbody = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.TryGetComponent<ICollectible>(out var collectible))
        {
            collectible.Collect(); //collectin içinde ne çalışırsa onu tek seferde istediğim yerde tutabiliyorum
        }
    }

    private void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.TryGetComponent<IBoostable>(out var boostable))
        {
            boostable.Boost(GetComponent<PlayerController>());
        }
    }
    private void OnParticleCollision(GameObject other) 
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.GiveDamage(_playerRigidbody, playerVisualTransform);
        }
    }
}