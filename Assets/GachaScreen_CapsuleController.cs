using DG.Tweening;
using UnityEngine;

public class GachaScreen_CapsuleController : GachaScreen_MonoBehaviour {
    [field: Header("References")]
    [field: SerializeField] public GachaScreen_CoinDropArea CoinDropArea { get; private set; }
    [field: SerializeField] public AudioSource CoinDropAudioSource { get; private set; }
    [field: SerializeField] public Animator ContainedCapsulesController { get; private set; }
    [field: SerializeField] public GachaScreen_KnobButton KnobButton { get; private set; }
    [field: SerializeField] public GachaScreen_Capsule Capsule { get; private set; }
    [field: SerializeField] public AudioSource CapsuleDropDownAudioSource { get; private set; }
    [field: SerializeField] public RectTransform SpawnRectTransform { get; private set; }
    [field: SerializeField] public RectTransform StationaryRectTransform { get; private set; }
    [field: Header("Data")]
    [field: SerializeField] public float CapsuleTweenToStationaryPositionTime { get; private set; } = 1f;
    private int m_Coin;
    private void Start() {
        Capsule.gameObject.SetActive(false);
    }
    private void OnEnable() {
        CoinDropArea.OnCoinDrop.AddListener(OnCoinDropped);
        KnobButton.KnobClicked.AddListener(OnKnobClicked);
        KnobButton.KnobFinished.AddListener(OnKnobFinished);
        Capsule.OnCapsuleClicked.AddListener(OnCapsuleClicked);
    }
    private void OnDisable() {
        CoinDropArea.OnCoinDrop.RemoveListener(OnCoinDropped);
        KnobButton.KnobClicked.RemoveListener(OnKnobClicked);
        KnobButton.KnobFinished.RemoveListener(OnKnobFinished);
        Capsule.OnCapsuleClicked.RemoveListener(OnCapsuleClicked);
    }
    private void OnCoinDropped(GachaScreen_Coin coin) {
        m_Coin++;
        PocketController.ConsumeCoin(coin);
        CoinDropAudioSource.Play();
    }
    private void OnKnobClicked(GachaScreen_KnobButton knob) {
        if (m_Coin <= 0) {
            Debug.Log("GachaScreen_CapsuleController: m_Coin <= 0");
            return;
        }
        if (Capsule.gameObject.activeSelf) {
            return;
        }
        ContainedCapsulesController.SetBool("IsRolling", true);
    }
    private void OnKnobFinished(GachaScreen_KnobButton knob) {
        if (m_Coin <= 0) {
            Debug.Log("GachaScreen_CapsuleController: m_Coin <= 0");
            return;
        }
        if (Capsule.gameObject.activeSelf) {
            return;
        }
        ContainedCapsulesController.SetBool("IsRolling", false);
        m_Coin--;
        Capsule.gameObject.SetActive(true);
        // Initialize capsule data
        Capsule.Initialize(GachaManager.GetRandomCapsuleData());
        // Tween
        CapsuleDropDownAudioSource.Play();
        Capsule.RectTransform.rotation = Quaternion.Euler(0f, 0f, Random.Range(-360f, 360f));
        Vector3 newRotation = new Vector3(
            Capsule.RectTransform.localRotation.x,
            Capsule.RectTransform.localRotation.y,
            Capsule.RectTransform.localRotation.z - 360f
        );
        Capsule.RectTransform.DORotate(newRotation, CapsuleTweenToStationaryPositionTime, RotateMode.LocalAxisAdd);
        Capsule.RectTransform.DOAnchorPos(SpawnRectTransform.anchoredPosition, 0f).OnComplete(() => {
            Capsule.RectTransform.DOAnchorPos(StationaryRectTransform.anchoredPosition, CapsuleTweenToStationaryPositionTime);
        });
    }
    private void OnCapsuleClicked(GachaScreen_Capsule capsule) {
        if (PackController.IsTweening) {
            return;
        }
        // Add to collection
        CollectionManager.AddCardData(capsule.CapsuleData.PackData.CardData);
        PackController.Initialize(capsule.CapsuleData);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(capsule.CapsuleImage.DOFade(0f, 1f));
        sequence.AppendCallback(() => capsule.gameObject.SetActive(false));
        sequence.Append(capsule.CapsuleImage.DOFade(1f, 0f));
    }
}
