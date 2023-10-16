using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GachaScreen_Coin : GachaScreen_MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    [field: Header("References")]
    [field: SerializeField] public Canvas Canvas { get; private set; }
    [field: Header("Data")]
    [field: SerializeField] public float TweenToOriginalPositionTime { get; private set; } = 0.5f;
    [field: Header("Runtime")]
    [field: SerializeField] public RectTransform RectTransform { get; private set; }
    [field: SerializeField] public Image Image { get; private set; }
    [field: SerializeField] public bool Dragging { get; private set; }
    private Vector2 m_OriginalPosition;
    private void Awake() {
        RectTransform = GetComponent<RectTransform>();
        Image = GetComponent<Image>();
    }
    public void OnBeginDrag(PointerEventData eventData) {
        Dragging = true;
        Image.raycastTarget = false;
        m_OriginalPosition = RectTransform.anchoredPosition;
    }
    public void OnDrag(PointerEventData eventData) {
        RectTransform.anchoredPosition += eventData.delta / Canvas.scaleFactor;
    }
    public void OnEndDrag(PointerEventData eventData) {
        ResetState();
        RectTransform.DOAnchorPos(m_OriginalPosition, TweenToOriginalPositionTime);
    }
    public void ResetState() {
        Dragging = false;
        Image.raycastTarget = true;
    }
}