using UnityEngine;

public class CoreBehaviour : MonoBehaviour {
    public GachaManager GachaManager => GachaManager.Instance;
    public EconomyManager EconomyManager => EconomyManager.Instance;
    public CollectionManager CollectionManager => CollectionManager.Instance;
}