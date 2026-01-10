using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    var go = new GameObject("GameManager");
                    _instance = go.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
        private set => _instance = value;
    }
     
     public event Action<GameState> OnGameStateChanged;

    [Header("References")]
    [SerializeField] private EggCounterUI _eggCounterUI;
    [SerializeField] private WinLoseUI _winLoseUI;



    [Header("Settings")]
    [SerializeField] private int _maxEggCount=5; 
    [SerializeField] private float _delay;

    private GameState _currentGameState;


    private int _currentEggCount;

    void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _currentEggCount = 0;
    }

    void Start() 
    {
         HealthManager.Instance.OnPlayerDeath += HealthManager_OnPlayerDeath;    
    }
    private void HealthManager_OnPlayerDeath()
    {
        StartCoroutine(OnGameOver());
    }

    private void OnEnable() 
    {
        ChangeGameState(_currentGameState);
    }
        
    


    public void ChangeGameState(GameState gameState)
    {
        OnGameStateChanged?.Invoke(gameState);
        _currentGameState = gameState;
        Debug.Log("Game State :" + gameState);
    }


    public void OnEggCollected()
    {
        if (_currentEggCount >= _maxEggCount) return;

        _currentEggCount++;
        _eggCounterUI.SetEggCounterText(_currentEggCount, _maxEggCount);


        if (_currentEggCount >= _maxEggCount)
        {
           //WIN
            _eggCounterUI.SetEggCompleted();
            ChangeGameState(GameState.GameOver);
            _winLoseUI.OnGameWin();
        }
    }
     
     private IEnumerator OnGameOver()
     {
            yield return new WaitForSeconds(_delay);
            ChangeGameState(GameState.GameOver);
            _winLoseUI.OnGameLose();
            }

      



      public GameState GetCurrentGameState()
    {   
        return _currentGameState;
    }


}
