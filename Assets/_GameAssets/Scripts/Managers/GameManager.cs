using System;
using UnityEngine;

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



    [Header("Settings")]
    [SerializeField] private int _maxEggCount=5; 

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

        Debug.Log("Egg Count: " + _currentEggCount);

        if (_currentEggCount >= _maxEggCount)
        {

            Debug.Log("Game Win!");
            _eggCounterUI.SetEggCompleted();
            ChangeGameState(GameState.GameOver);
        }
    }


      public GameState GetCurrentGameState()
    {   
        return _currentGameState;
    }


}
