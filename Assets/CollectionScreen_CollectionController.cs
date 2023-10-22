using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionScreen_CollectionController : CollectionScreen_MonoBehaviour {
    [field: Header("References")]
    [field: SerializeField] public Image CardViewImage { get; private set; }
    [field: SerializeField] public Transform CollectedCardPanelTransform { get; private set; }
    [field: SerializeField] public Button LeftButton { get; private set; }
    [field: SerializeField] public Button RightButton { get; private set; }
    [field: Header("Data")]
    [field: SerializeField] public CollectionScreen_CollectedCardPage CollectedCardPagePrefab { get; private set; }
    [field: Header("Runtime")]
    [field: SerializeField] public List<CollectionScreen_CollectedCardPage> CollectedCardPageList { get; private set; }
    [field: SerializeField] public int ActivePageId { get; private set; }
    public void Initialize() {
        // TODO: Only refresh updated cards
        foreach (Transform childTransform in CollectedCardPanelTransform.transform) {
            Destroy(childTransform.gameObject);
        }
        CollectedCardPageList.Clear();
        CardViewImage.DOFade(0f, 0);
        // Do nothing if card count is zero
        if (CollectionManager.Instance.CollectedCardList.Count == 0) {
            return;
        }
        // First page
        CollectionScreen_CollectedCardPage page = Instantiate(CollectedCardPagePrefab, CollectedCardPanelTransform);
        CollectedCardPageList.Add(page);
        CollectionManager.Instance.Sort();
        foreach (var collectedCardData in CollectionManager.Instance.CollectedCardList) {
            CollectionScreen_CollectedCardButton card;
            // Add to existed page first
            if (page.TryAddCard(collectedCardData, out card)) {
                // TODO: Move this to somewhere else?
                card.Button.onClick.RemoveAllListeners();
                card.Button.onClick.AddListener(() => OnCardClicked(card));
                continue;
            }
            // Create new page since previous page is full
            page = Instantiate(CollectedCardPagePrefab, CollectedCardPanelTransform);
            CollectedCardPageList.Add(page);
            // Add to newly created page
            if (page.TryAddCard(collectedCardData, out card)) {
                // TODO: Move this to somewhere else?
                card.Button.onClick.RemoveAllListeners();
                card.Button.onClick.AddListener(() => OnCardClicked(card));
                continue;
            }
            Debug.LogError("[CollectionScreen_CollectionController] Refresh() >>> Somehow newly created CollectionScreen_CollectedCardPage is already full?");
        }
        // Deactive all other page except for the first one
        for (int i = 1; i < CollectedCardPageList.Count; i++) {
            CollectedCardPageList[i].gameObject.SetActive(false);
        }
        ActivePageId = 0;
        // Default to first page, first card
        CardViewImage.sprite = CollectedCardPageList[ActivePageId].CollectedCardButtonList[0].CollectedCardData.CardData.Sprite;
        CardViewImage.DOFade(1f, 1);
        // Button callbacks
        LeftButton.onClick.RemoveAllListeners();
        LeftButton.onClick.AddListener(() => {
            ActivePageId = Math.Clamp(--ActivePageId, 0, CollectedCardPageList.Count - 1);
            for (int i = 0; i < CollectedCardPageList.Count; i++) {
                CollectedCardPageList[i].gameObject.SetActive(ActivePageId == i);
            }
        });
        RightButton.onClick.RemoveAllListeners();
        RightButton.onClick.AddListener(() => {
            ActivePageId = Math.Clamp(++ActivePageId, 0, CollectedCardPageList.Count - 1);
            for (int i = 0; i < CollectedCardPageList.Count; i++) {
                CollectedCardPageList[i].gameObject.SetActive(ActivePageId == i);
            }
        });
    }
    private void OnCardClicked(CollectionScreen_CollectedCardButton card) {
        CardViewImage.DOKill();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(CardViewImage.DOFade(0f, 0.25f));
        sequence.AppendCallback(() => CardViewImage.sprite = card.CollectedCardData.CardData.Sprite);
        sequence.Append(CardViewImage.DOFade(1f, 0.5f));
    }
}
