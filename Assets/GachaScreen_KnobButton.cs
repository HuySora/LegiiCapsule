using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GachaScreen_KnobButton : GachaScreen_MonoBehaviour {
    [field: Header("Data")]
    [field: SerializeField] public UnityEvent<GachaScreen_KnobButton> KnobClicked { get; private set; }
    [field: SerializeField] public UnityEvent<GachaScreen_KnobButton> KnobFinished { get; private set; }
    [field: SerializeField] public float TweenRotateTime { get; private set; } = 1.5f;
    [field: Header("Runtime")]
    [field: SerializeField] public Button Button { get; private set; }
    [field: SerializeField] public RectTransform RectTransform { get; private set; }
    private Tween m_TweenSpin;
    private Vector3 m_OriginalRotation;
    private void Awake() {
        Button = GetComponent<Button>();
        RectTransform = GetComponent<RectTransform>();
        m_OriginalRotation = RectTransform.localRotation.eulerAngles;
    }
    private void OnEnable() {
        Button.onClick.AddListener(OnKnobClicked);
    }
    private void OnDisable() {
        Button.onClick.RemoveListener(OnKnobClicked);
    }
    private void OnKnobClicked() {
        bool tweenIsComplete = m_TweenSpin == null ? false :
            m_TweenSpin.IsActive() ? m_TweenSpin.IsPlaying() : false;
        if (tweenIsComplete) {
            return;
        }
        KnobClicked?.Invoke(this);
        Vector3 newRotation = new Vector3(RectTransform.localRotation.x, RectTransform.localRotation.y, RectTransform.localRotation.z - 360);
        m_TweenSpin = RectTransform.DORotate(newRotation, TweenRotateTime, RotateMode.LocalAxisAdd).SetEase(Ease.OutBack).OnComplete(() => {
            KnobFinished?.Invoke(this);
        });
    }
}