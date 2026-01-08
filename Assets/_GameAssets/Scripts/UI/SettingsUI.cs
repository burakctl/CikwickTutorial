using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingsUI : MonoBehaviour
{
     [Header("References")]
     [SerializeField] private GameManager _gameManager;

     [SerializeField] private GameObject _settingsPopupObject;
     [SerializeField] private GameObject _blackBackgroundObject;




    [Header("Buttons")]
    
     [SerializeField] private Button _settingsButton;
     [SerializeField] private Button _musicButton;
     [SerializeField] private Button _soundButton;
     [SerializeField] private Button _resumeButton;
     [SerializeField] private Button _mainMenuButton;

     [Header("Settings")]
     [SerializeField] private float _animationDuration;


     private Image _blackBackgroundObjectImage;

        private void Awake()
        {
            
            _blackBackgroundObjectImage = _blackBackgroundObject.GetComponent<Image>();
            _settingsPopupObject.transform.localScale = Vector3.zero;
            _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
            _resumeButton.onClick.AddListener(OnResumeButtonClicked);
        }

     private void OnSettingsButtonClicked()
     {
       _gameManager.ChangeGameState(GameState.Pause);
        _blackBackgroundObject.SetActive(true);
        _settingsPopupObject.SetActive(true);
        _blackBackgroundObjectImage.DOFade(0.8f, _animationDuration).SetEase(Ease.Linear);
        _settingsPopupObject.transform.DOScale(1.5f, _animationDuration).SetEase(Ease.OutBack);
     }

     private void OnResumeButtonClicked()
     {
        _blackBackgroundObjectImage.DOFade(0f, _animationDuration).SetEase(Ease.Linear);
        _settingsPopupObject.transform.DOScale(0f, _animationDuration).SetEase(Ease.OutExpo).OnComplete(() =>
        {
           _gameManager.ChangeGameState(GameState.Resume);
              _blackBackgroundObject.SetActive(false);  
              _settingsPopupObject.SetActive(false);

        });
      
      
     }
 

}
