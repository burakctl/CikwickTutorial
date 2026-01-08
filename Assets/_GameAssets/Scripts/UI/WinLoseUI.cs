using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WinLoseUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _blackBackgroundObject;
    [SerializeField] private GameObject _winPopupObject;
    [SerializeField] private GameObject _losePopupObject;

    [Header("Settings")]    
    [SerializeField] private float _animationDuration=0.3f;



    private Image _blackBackgroundObjectImage;
    private RectTransform _winPopupRectTransform;
    private RectTransform _losePopupRectTransform;

    void Awake() {
        _blackBackgroundObjectImage = _blackBackgroundObject.GetComponent<Image>();
        _winPopupRectTransform = _winPopupObject.GetComponent<RectTransform>();
        _losePopupRectTransform = _losePopupObject.GetComponent<RectTransform>();
         }
         

    public void OnGameWin()
    {
        _blackBackgroundObject.SetActive(true);
        _winPopupObject.SetActive(true);
        _blackBackgroundObjectImage.DOFade(0.8f, _animationDuration).SetEase(Ease.Linear);
        _winPopupRectTransform.DOScale(1.5f, _animationDuration).SetEase(Ease.OutBack);
        
        
    }

    public void OnGameLose()
    {
        _blackBackgroundObject.SetActive(true);
        _losePopupObject.SetActive(true);
        _blackBackgroundObjectImage.DOFade(0.8f, _animationDuration).SetEase(Ease.Linear);
        _losePopupRectTransform.DOScale(1.5f, _animationDuration).SetEase(Ease.OutBack);
        
        
    }
}



