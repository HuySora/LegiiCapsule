using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "LegiiCapsule/CardData")]
public class CardData : ScriptableObject {
    [field: SerializeField] public Sprite Sprite { get; private set; }
    public void Initialize(Sprite sprite) {
        Sprite = sprite;
    }
}
