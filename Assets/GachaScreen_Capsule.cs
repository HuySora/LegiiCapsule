using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GachaScreen_Capsule : GachaScreen_MonoBehaviour {
    [Header("Data")]
    public UnityEvent<GachaScreen_Capsule> OnCapsuleClicked;
    [field: Header("Runtime")]
    [field: SerializeField] public RectTransform RectTransform { get; private set; }
    [field: SerializeField] public Image CapsuleImage { get; private set; }
    [field: SerializeField] public Button CapsuleButton { get; private set; }
    [field: SerializeField] public CapsuleData CapsuleData { get; private set; }
    private void Awake() {
        RectTransform = GetComponent<RectTransform>();
        CapsuleImage = GetComponent<Image>();
        CapsuleButton = GetComponent<Button>();
    }
    private void OnEnable() {
        CapsuleButton.onClick.AddListener(InvokeOnCapsuleClicked);
    }
    private void OnDisable() {
        CapsuleButton.onClick.RemoveListener(InvokeOnCapsuleClicked);
    }
    private void InvokeOnCapsuleClicked() {
        OnCapsuleClicked?.Invoke(this);
    }
    public void Initialize(CapsuleData data) {
        CapsuleData = data;
        CapsuleImage.sprite = data.CapsuleSprite;
    }
}