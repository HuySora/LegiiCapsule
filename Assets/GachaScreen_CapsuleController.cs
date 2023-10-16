using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class GachaScreen_CapsuleController : GachaScreen_MonoBehaviour {
    [field: Header("References")]
    [field: SerializeField] public GachaScreen_CoinDropArea CoinDropArea { get; private set; }
    [field: SerializeField] public GachaScreen_KnobButton KnobButton { get; private set; }
    [field: SerializeField] public GachaScreen_Capsule Capsule { get; private set; }
    [field: SerializeField] public RectTransform SpawnRectTransform { get; private set; }
    [field: SerializeField] public RectTransform StationaryRectTransform { get; private set; }
    [field: Header("Data")]
    [field: SerializeField] public float CapsuleTweenToStationaryPositionTime { get; private set; } = 1f;
    [field: SerializeField] public float PinkCapsuleWeight { get; private set; }
    [field: SerializeField] public List<CapsuleData> PinkCapsuleList { get; private set; }
    [field: SerializeField] public float VioletCapsuleWeight { get; private set; }
    [field: SerializeField] public List<CapsuleData> VioletCapsuleList { get; private set; }
    [field: SerializeField] public float BlueCapsuleWeight { get; private set; }
    [field: SerializeField] public List<CapsuleData> BlueCapsuleList { get; private set; }
    private int m_Coin;
    private void Start() {
        Capsule.gameObject.SetActive(false);
    }
    private void OnEnable() {
        CoinDropArea.OnCoinDrop.AddListener(OnCoinDropped);
        KnobButton.OnKnobClicked.AddListener(OnKnobClicked);
        Capsule.OnCapsuleClicked.AddListener(OnCapsuleClicked);
    }
    private void OnDisable() {
        CoinDropArea.OnCoinDrop.RemoveListener(OnCoinDropped);
        KnobButton.OnKnobClicked.RemoveListener(OnKnobClicked);
        Capsule.OnCapsuleClicked.RemoveListener(OnCapsuleClicked);
    }
    private void OnCoinDropped(GachaScreen_Coin coin) {
        m_Coin++;
        PocketController.ConsumeCoin(coin);
    }
    private void OnKnobClicked(GachaScreen_KnobButton knob) {
        if (m_Coin <= 0) {
            Debug.Log("GachaScreen_CapsuleController: m_Coin <= 0");
            return;
        }
        if (Capsule.gameObject.activeSelf) {
            return;
        }
        m_Coin--;
        Capsule.gameObject.SetActive(true);
        // Initialize capsule data
        Capsule.Initialize(GetRandomCapsuleData());
        // Tween
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
    private CapsuleData GetRandomCapsuleData() {
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
    private void OnCapsuleClicked(GachaScreen_Capsule capsule) {
        if (PackController.IsTweening) {
            return;
        }
        PackController.Initialize(capsule.CapsuleData);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(capsule.CapsuleImage.DOFade(0f, 1f));
        sequence.AppendCallback(() => capsule.gameObject.SetActive(false));
        sequence.Append(capsule.CapsuleImage.DOFade(1f, 0f));
    }
}
