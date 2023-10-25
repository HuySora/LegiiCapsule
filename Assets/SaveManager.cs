using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SaveData {
    public int Coin;
    public List<string> CardDataCodeList = new List<string>();
}
public class SaveManager : CoreBehaviour {
    public static SaveManager Instance;
    private void Awake() {
        Instance = this;
    }
    private void Start() {
        Load();
    }
    private void OnApplicationQuit() {
        Save();
    }
    public void Load() {
        string jsonData = PlayerPrefs.GetString(Application.productName + "_SaveData", JsonUtility.ToJson(new SaveData()));
        SaveData data = JsonUtility.FromJson<SaveData>(jsonData);
        // This should be set not add
        EconomyManager.AddCoin(data.Coin);
        foreach (string cardDataCode in data.CardDataCodeList) {
            // TODO: Hard-coded btw
            string[] splits = cardDataCode.Split('_');
            string cardType = splits[0];
            int cardIndex = int.Parse(splits[1]);
            int cardCount = int.Parse(splits[2]);
            // Pink
            if (cardType == "P") {
                for (int i = 0; i < cardCount; i++) {
                    CollectionManager.AddCardData(GachaManager.PinkCapsuleList[cardIndex].PackData.CardData);
                }
            }
            if (cardType == "V") {
                for (int i = 0; i < cardCount; i++) {
                    CollectionManager.AddCardData(GachaManager.VioletCapsuleList[cardIndex].PackData.CardData);
                }
            }
            if (cardType == "B") {
                for (int i = 0; i < cardCount; i++) {
                    CollectionManager.AddCardData(GachaManager.BlueCapsuleList[cardIndex].PackData.CardData);
                }
            }
        }
    }
    public void Save() {
        SaveData data = new SaveData();
        data.Coin = EconomyManager.Coin;
        // TODO: Optimize this save/load
        foreach (var collectedCardData in CollectionManager.CollectedCardList) {
            string cardDataCode = string.Empty;
            // Pink
            for (int i = 0; i < GachaManager.PinkCapsuleList.Count; i++) {
                CapsuleData pinkCapsule = GachaManager.PinkCapsuleList[i];
                if (collectedCardData.CardData == pinkCapsule.PackData.CardData) {
                    cardDataCode = $"P_{i}_{collectedCardData.Count}";
                    break;
                }
            }
            // Violet
            if (cardDataCode == string.Empty) {
                for (int i = 0; i < GachaManager.VioletCapsuleList.Count; i++) {
                    CapsuleData violetCapsule = GachaManager.VioletCapsuleList[i];
                    if (collectedCardData.CardData == violetCapsule.PackData.CardData) {
                        cardDataCode = $"V_{i}_{collectedCardData.Count}";
                        break;
                    }
                }
            }
            // Blue
            if (cardDataCode == string.Empty) {
                for (int i = 0; i < GachaManager.BlueCapsuleList.Count; i++) {
                    CapsuleData blueCapsule = GachaManager.BlueCapsuleList[i];
                    if (collectedCardData.CardData == blueCapsule.PackData.CardData) {
                        cardDataCode = $"B_{i}_{collectedCardData.Count}";
                        break;
                    }
                }
            }
            data.CardDataCodeList.Add(cardDataCode);
        }
        string jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(Application.productName + "_SaveData", jsonData);
    }
}
