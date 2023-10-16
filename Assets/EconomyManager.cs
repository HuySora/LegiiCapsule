using UnityEngine;

public class EconomyManager : MonoBehaviour {
    public static EconomyManager Instance;
    [field: Header("Runtime")]
    [field: SerializeField] public int Coin { get; private set; }
    private void Awake() {
        Instance = this;
    }
    public void AddCoin(int value) {
        Coin += value;
    }
    public void RemoveCoin(int value) {
        Coin -= value;
    }
}
