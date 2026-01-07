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



    private void Start() 
    {
        PlayerRotationAnimations();
        StartTimer();
    }
     
     private void PlayerRotationAnimations()
     {
        _timerRotatableTransform.DORotate(new Vector3(0, 0, -360f), _rotationDuration, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(_rotationEase); 
     }

    public void StartTimer()
    {
        _elapsedTime = 0f;
        InvokeRepeating(nameof(UpdateTimerUI), 0f, 1f);
        
    }

    private void UpdateTimerUI() 
    {
        _elapsedTime +=1;
        int minutes = Mathf.FloorToInt(_elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(_elapsedTime % 60f);
        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }






}
