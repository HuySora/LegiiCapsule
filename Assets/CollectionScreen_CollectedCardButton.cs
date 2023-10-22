using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CollectionScreen_CollectedCardButton : CollectionScreen_MonoBehaviour {
    [field: Header("References")]
    [field: SerializeField] public Image Image { get; private set; }
    [field: SerializeField] public Button Button { get; private set; }
    [field: Header("Data")]
    [field: SerializeField] public UnityEvent<CollectionScreen_CollectedCardButton> OnCardClicked { get; private set; }
    [field: Header("Runtime")]
    [field: SerializeField] public CollectedCardData CollectedCardData { get; private set; }
    private void OnValidate() {
        Image = GetComponent<Image>();
        Button = GetComponent<Button>();
    }
    private void OnEnable() {
        Button.onClick.AddListener(InvokeOnCardClicked);
    }
    private void OnDisable() {
        Button.onClick.RemoveListener(InvokeOnCardClicked);
    }
    private void InvokeOnCardClicked() {
        OnCardClicked?.Invoke(this);
    }
    public void Initialize(CollectedCardData data) {
        CollectedCardData = data;
        Image.sprite = data.CardData.Sprite;
    }
}