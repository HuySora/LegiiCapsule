using System.Collections.Generic;
using UnityEngine;

public class CollectionManager : MonoBehaviour {
    public static CollectionManager Instance;
    // TODO: CollectedCardList should be Dictionary for better performance (use List for Inspector views)
    [field: SerializeField] public List<CollectedCardData> CollectedCardList { get; private set; }
    private void Awake() {
        Instance = this;
    }
    public void AddCardData(CardData cardData) {
        for (int i = 0; i < CollectedCardList.Count; i++) {
            if (cardData == CollectedCardList[i].CardData) {
                CollectedCardList[i].Count++;
                return;
            }
        }
        CollectedCardList.Add(new CollectedCardData() {
            CardData = cardData,
            Count = 1
        });
    }
    public void Sort() {
        CollectedCardList.Sort((left, right) => {
            return left.CardData.name.CompareTo(right.CardData.name);
        });
    }
}
