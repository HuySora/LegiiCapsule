using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
public partial class GachaManager : MonoBehaviour {
    [MenuItem("Assets/Create/LegiiCapsule/Automate/AutoCreateCapsule")]
    public static void AutoCreateCapsule() {
        Queue<string> assetGuidQueue = new Queue<string>();
        string selectionPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        // If the selection is a folder
        if (ProjectWindowUtil.IsFolder(Selection.activeInstanceID)) {
            Debug.LogError("Can't create data from this type of asset");
            // We do nothing here
            return;
        }
        // If the selection is not a Sprite
        else if (AssetDatabase.LoadAssetAtPath<Sprite>(selectionPath) == null) {
            Debug.LogError("Can't create data from this type of asset");
            // We do nothing here
            return;
        }
        else {
            foreach (var selection in Selection.assetGUIDs) {
                assetGuidQueue.Enqueue(selection);
            }
        }
        while (assetGuidQueue.TryDequeue(out string assetGuid)) {
            string path = AssetDatabase.GUIDToAssetPath(assetGuid);
            // CardData
            CardData cardData = ScriptableObject.CreateInstance<CardData>();
            cardData.Initialize(AssetDatabase.LoadAssetAtPath<Sprite>(path));
            // PackData
            PackData packData = ScriptableObject.CreateInstance<PackData>();
            packData.Initialize(cardData);
            // CapsuleData
            CapsuleData capsuleData = ScriptableObject.CreateInstance<CapsuleData>();
            capsuleData.Initialize(null, packData);
            // Create .asset file , select, rename
            string folderPath = Path.GetDirectoryName(path);
            string fileName = Path.GetFileNameWithoutExtension(path);
            ProjectWindowUtil.CreateAsset(cardData, $"{folderPath}/CardData_{fileName}.asset");
            ProjectWindowUtil.CreateAsset(packData, $"{folderPath}/PackData_{fileName}.asset");
            ProjectWindowUtil.CreateAsset(capsuleData, $"{folderPath}/CapsuleData_{fileName}.asset");
        }
    }
}
#endif

public partial class GachaManager : MonoBehaviour {
    public static GachaManager Instance;
    [field: Header("Data")]
    [field: SerializeField] public float PinkCapsuleWeight { get; private set; }
    [field: SerializeField] public List<CapsuleData> PinkCapsuleList { get; private set; }
    [field: SerializeField] public float VioletCapsuleWeight { get; private set; }
    [field: SerializeField] public List<CapsuleData> VioletCapsuleList { get; private set; }
    [field: SerializeField] public float BlueCapsuleWeight { get; private set; }
    [field: SerializeField] public List<CapsuleData> BlueCapsuleList { get; private set; }
    private void Awake() {
        Instance = this;
    }
    public CapsuleData GetRandomCapsuleData() {
        // Random rarities
        float[] weights = new float[] {
            PinkCapsuleWeight,
            VioletCapsuleWeight,
            BlueCapsuleWeight
        };
        for (int i = 1; i < weights.Length; i++) {
            weights[i] += weights[i - 1];
        }
        int choosedRarity = 0;
        float rndFloat = Random.Range(0f, weights[weights.Length - 1]);
        for (choosedRarity = 0; choosedRarity < weights.Length; choosedRarity++) {
            if (weights[choosedRarity] > rndFloat) {
                break;
            }
        }
        // Random capsules
        int rndInt = 0;
        switch (choosedRarity) {
            // Violet
            case 1:
                rndInt = Random.Range(0, VioletCapsuleList.Count);
                return VioletCapsuleList[rndInt];
            // Blue
            case 2:
                rndInt = Random.Range(0, BlueCapsuleList.Count);
                return BlueCapsuleList[rndInt];

            // Default to lowest rarity (Pink)
            default:
            // Pink
            case 0:
                rndInt = Random.Range(0, VioletCapsuleList.Count);
                return PinkCapsuleList[rndInt];
        }
    }
}
