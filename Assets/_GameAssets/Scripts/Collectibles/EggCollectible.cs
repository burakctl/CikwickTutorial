using UnityEngine;

public class EggCollectible : MonoBehaviour, ICollectible
{
     private bool _collected = false;
     public void Collect()
    {
          if (_collected) return;
          _collected = true;

           GameManager.Instance.OnEggCollected();
           Destroy(gameObject);// yumurtanın yenilmesi
    }
}
