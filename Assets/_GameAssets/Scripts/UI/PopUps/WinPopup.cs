using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WinPopup : MonoBehaviour
{
    
[Header("References")]
    [SerializeField] private TimerUI _timerUI;
    [SerializeField] private Button _oneMoreButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private TMP_Text _timeText;



void OnEnable() 
{
    _timeText.text = _timerUI.GetFinalTime();
    _oneMoreButton.onClick.AddListener(OnOneMoreButtonClicked);
}
private void OnOneMoreButtonClicked()
{
    SceneManager.LoadScene(Consts.SceneNames.GAME_SCENE);
}




}



