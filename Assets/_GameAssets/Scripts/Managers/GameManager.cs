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


    [Header("References")]
    [SerializeField] private EggCounterUI _eggCounterUI;



    [Header("Settings")]
    [SerializeField] private int _maxEggCount=5; 


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
        }
    }
}
