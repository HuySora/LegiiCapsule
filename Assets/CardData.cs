using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "LegiiCapsule/CardData")]
public class CardData : ScriptableObject {
    [field: SerializeField] public Sprite Sprite { get; private set; }
}
