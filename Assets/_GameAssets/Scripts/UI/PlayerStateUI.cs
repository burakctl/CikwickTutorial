using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerStateUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private RectTransform _playerWalkingTransform;
    [SerializeField] private RectTransform _playerSlidingTransform;   // ✔ eklendi
    [SerializeField] private RectTransform _boosterSpeedTransform;
    [SerializeField] private RectTransform _boosterJumpTransform;    
    [SerializeField] private RectTransform _boosterSlowTransform;

    [Header("Images")]
    [SerializeField] private Image _goldBossterWheatImage;
    [SerializeField] private Image _holyBossterWheatImage;
    [SerializeField] private Image _rottenBossterWheatImage;

    [Header("Sprites")]
    [SerializeField] private Sprite _playerWalkingActiveSprite;
    [SerializeField] private Sprite _playerWalkingPassiveSprite;
    [SerializeField] private Sprite _playerSlidingActiveSprite;
    [SerializeField] private Sprite _playerSlidingPassiveSprite;

    [Header("Settings")]
    [SerializeField] private float _moveDuration;
    [SerializeField] private Ease MoveEase;

    public RectTransform GetBoosterSpeedTransform() => _boosterSpeedTransform;
    public RectTransform GetBoosterJumpTransform() => _boosterJumpTransform;
    public RectTransform GetBoosterSlowTransform() => _boosterSlowTransform;
    public Image GetGoldBoosterWheatImage() => _goldBossterWheatImage;
    public Image GetHolyBoosterWheatImage() => _holyBossterWheatImage;
    public Image GetRottenBoosterWheatImage() => _rottenBossterWheatImage;

    private Image _playerWalkingImage;
    private Image _playerSlidingImage;

    private void Awake()
    {
        _playerWalkingImage = _playerWalkingTransform.GetComponent<Image>();
        _playerSlidingImage = _playerSlidingTransform.GetComponent<Image>();
    }

    private void Start()
    {
        _playerController.OnPlayerStateChanged += PlayerController_OnPlayerStateChanged;

        SetUserInterfaces(
            _playerWalkingActiveSprite, _playerWalkingPassiveSprite,
            _playerWalkingTransform, _playerSlidingTransform
        );
    }

    private void PlayerController_OnPlayerStateChanged(PlayerState playerState)
    {
        switch (playerState)
        {
            case PlayerState.Idle:
            case PlayerState.Move:
                SetUserInterfaces(
                    _playerWalkingActiveSprite, _playerWalkingPassiveSprite,
                    _playerWalkingTransform, _playerSlidingTransform
                );
                break;

            case PlayerState.SlideIdle:
            case PlayerState.Slide:
                SetUserInterfaces(
                    _playerWalkingPassiveSprite, _playerSlidingActiveSprite,
                    _playerSlidingTransform, _playerWalkingTransform
                );
                break;
        }
    }

    private void SetUserInterfaces(
        Sprite playerWalkingSprite, Sprite playerSlidingSprite,
        RectTransform activeTransform, RectTransform passiveTransform)
    {
        _playerWalkingImage.sprite = playerWalkingSprite;
        _playerSlidingImage.sprite = playerSlidingSprite;

        activeTransform.DOAnchorPosX(-25f, _moveDuration).SetEase(MoveEase);
        passiveTransform.DOAnchorPosX(-90f, _moveDuration).SetEase(MoveEase);
    }

    private IEnumerator SetBoosterInterface(
        RectTransform activeTransform, Image boosterImage, Image wheatImage,
        Sprite activeSprite, Sprite passiveSprite,
        Sprite activeWheatSprite, Sprite passiveWheatSprite,
        float duration)
    {
        boosterImage.sprite = activeSprite;
        wheatImage.sprite = activeWheatSprite;

        activeTransform.DOAnchorPosX(25f, _moveDuration).SetEase(MoveEase);
        yield return new WaitForSeconds(duration);

        boosterImage.sprite = passiveSprite;
        wheatImage.sprite = passiveWheatSprite;

        activeTransform.DOAnchorPosX(90f, _moveDuration).SetEase(MoveEase);
    }

    public void PlayerBoosterUIAnimations(
        RectTransform activeTransform, Image boosterImage, Image wheatImage,
        Sprite activeSprite, Sprite passiveSprite,
        Sprite activeWheatSprite, Sprite passiveWheatSprite,
        float duration)
    {
        StartCoroutine(SetBoosterInterface(
            activeTransform, boosterImage, wheatImage,
            activeSprite, passiveSprite,
            activeWheatSprite, passiveWheatSprite,
            duration));
    }
}
