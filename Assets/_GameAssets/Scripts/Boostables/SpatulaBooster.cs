using UnityEngine;

public class SpatulaBooster : MonoBehaviour, IBoostable
{

    [Header("Preferences")]
    [SerializeField]  private Animator _spatulaAnimator;
    [Header("Settings")]
    [SerializeField]  private float _jumpForce;

    private bool _isActivated;

    public void Boost(PlayerController playerController)
    {
        if (_isActivated) return;
        PlayboostAnimation();
        Rigidbody playerRigidbody = playerController.GetPlayerRigidbody();
        playerRigidbody.linearVelocity = new Vector3(playerRigidbody.linearVelocity.x, 0f, playerRigidbody.linearVelocity.z);
        playerRigidbody.AddForce(transform.forward*_jumpForce, ForceMode.Impulse);  
        _isActivated = true;
        Invoke(nameof(ResetActivation), 0.2f);
    }

    private void PlayboostAnimation()
    {
        _spatulaAnimator.SetTrigger(Consts.OtherAnimations.IS_SPATULA_JUMPING);
    }

    private void ResetActivation()
    {
        _isActivated = false;
    }
}
