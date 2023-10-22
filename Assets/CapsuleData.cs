using UnityEngine;

[CreateAssetMenu(fileName = "CapsuleData", menuName = "LegiiCapsule/CapsuleData")]
public class CapsuleData : ScriptableObject {
    [field: SerializeField] public Sprite CapsuleSprite { get; private set; }
    [field: SerializeField] public PackData PackData { get; private set; }
    public void Initialize(Sprite capsuleSprite, PackData packData) {
        CapsuleSprite = capsuleSprite;
        PackData = packData;
    }
}
