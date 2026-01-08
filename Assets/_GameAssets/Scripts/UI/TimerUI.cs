
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TimerUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform _timerRotatableTransform;
    [SerializeField] private TMP_Text _timerText;

    [Header("Settings")]
    [SerializeField] private float _rotationDuration;
    [SerializeField] private Ease _rotationEase;

   private float _elapsedTime;
   private bool _isTimerRunning = false;
   private Tween _rotationTween;
   private string _finalTime;


    private void Start() 
    {
        PlayerRotationAnimations();
        StartTimer();
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(GameState gameState)
    {
          
          switch (gameState)
          {
            case GameState.Pause:
                PauseTimer();
                break;
            case GameState.Resume:
                ResumeTimer();
                break;
            case GameState.GameOver:
                FinishTimer();
                break;
          }
    }

     
     private void PlayerRotationAnimations()
     {
       _rotationTween = _timerRotatableTransform.DORotate(new Vector3(0, 0, -360f), _rotationDuration, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(_rotationEase); 
     }

    public void StartTimer()
    {
        _isTimerRunning = true;
        _elapsedTime = 0f;
        InvokeRepeating(nameof(UpdateTimerUI), 0f, 1f);
        
    }

        private void PauseTimer()
    {
        _isTimerRunning = false;
        CancelInvoke(nameof(UpdateTimerUI));
        _rotationTween.Pause();
    }

    private void ResumeTimer()
    {
        if (!_isTimerRunning)
        {
            _isTimerRunning = true;
            InvokeRepeating(nameof(UpdateTimerUI), 0f, 1f);
            _rotationTween.Play();
        }
    }

    private void FinishTimer()
    {
          StopTimer();
        _finalTime = GetFormattedElapsedTime();
    }

    private void StopTimer()
    {
        _isTimerRunning = false;
        CancelInvoke(nameof(UpdateTimerUI));
        if (_rotationTween != null)
        {
            _rotationTween.Kill();
            _rotationTween = null;
        }
    }

    private string GetFormattedElapsedTime()
    {
        int minutes = Mathf.FloorToInt(_elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(_elapsedTime % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }



    private void UpdateTimerUI() 
    {
        if(!_isTimerRunning){return;}
        
        _elapsedTime +=1;
        int minutes = Mathf.FloorToInt(_elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(_elapsedTime % 60f);
        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


    public string GetFinalTime()
    {
        return _finalTime;
    }






}
