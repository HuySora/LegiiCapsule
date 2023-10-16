using UnityEngine;

[CreateAssetMenu(fileName = "PackData", menuName = "LegiiCapsule/PackData")]
public class PackData : ScriptableObject {
    [field: SerializeField] public CardData CardData { get; private set; }
}
