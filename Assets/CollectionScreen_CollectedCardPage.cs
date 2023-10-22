using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectionScreen_CollectedCardPage : CollectionScreen_MonoBehaviour {
    [field: Header("References")]
    [field: SerializeField] public List<CollectionScreen_CollectedCardButton> CollectedCardButtonList { get; private set; }
    private void OnValidate() {
        CollectedCardButtonList = GetComponentsInChildren<CollectionScreen_CollectedCardButton>(true).ToList();
        foreach (var card in CollectedCardButtonList) {
            card.gameObject.SetActive(false);
        }
    }
    public bool TryAddCard(CollectedCardData data, out CollectionScreen_CollectedCardButton card) {
        card = null;
        // We use image active state to check for slot availability here
        foreach (var c in CollectedCardButtonList) {
            if (!c.gameObject.activeSelf) {
                c.Initialize(data);
                c.gameObject.SetActive(true);
                card = c;
                return true;
            }
        }
        return false;
    }
}