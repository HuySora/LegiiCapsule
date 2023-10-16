using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GachaScreen_PackController : GachaScreen_MonoBehaviour {
    [field: Header("References")]
    [field: SerializeField] public Image CardImage { get; private set; }
    [field: SerializeField] public RectTransform PackRectTransform { get; private set; }
    [field: SerializeField] public RectTransform SpawnRectTransform { get; private set; }
    [field: SerializeField] public RectTransform StationaryRectTransform { get; private set; }
    [field: Header("Data")]
    [field: SerializeField] public float CapsuleTweenToStationaryPositionTime { get; private set; } = 2f;
    [field: SerializeField] public float CapsuleTweenShakeTime { get; private set; } = 3f;
    [field: SerializeField] public float CapsuleTweenToSpawnPositionTime { get; private set; } = 2f;
    [field: Header("Runtime")]
    [field: SerializeField] public bool IsTweening { get; private set; }
    private Sequence m_OldPackSequence;
    private Sequence m_NewPackSequence;
    private void Start() {
        CardImage.gameObject.SetActive(false);
        PackRectTransform.gameObject.SetActive(false);
    }
    private void Update() {
        bool oldIsComplete = m_OldPackSequence == null ? false :
            m_OldPackSequence.IsActive() ? m_OldPackSequence.IsPlaying() : false;
        bool newIsComplete = m_NewPackSequence == null ? false :
            m_NewPackSequence.IsActive() ? m_NewPackSequence.IsPlaying() : false;
        IsTweening = oldIsComplete || newIsComplete;
    }
    public void Initialize(CapsuleData capsuleData) {
        // Old pack
        m_OldPackSequence = DOTween.Sequence();
        m_OldPackSequence.Append(CardImage.DOFade(0f, CapsuleTweenToStationaryPositionTime));
        m_OldPackSequence.AppendCallback(() => CardImage.gameObject.SetActive(false));
        m_OldPackSequence.Append(CardImage.DOFade(1f, 0f));
        // New pack
        m_NewPackSequence = DOTween.Sequence();
        m_NewPackSequence.AppendCallback(() => PackRectTransform.gameObject.SetActive(true));
        m_NewPackSequence.Append(PackRectTransform.DOAnchorPos(SpawnRectTransform.anchoredPosition, 0f));
        m_NewPackSequence.Append(PackRectTransform.DOAnchorPos(StationaryRectTransform.anchoredPosition, CapsuleTweenToStationaryPositionTime));
        Vector3 shakeRotation = new Vector3(PackRectTransform.localRotation.x, PackRectTransform.localRotation.y, PackRectTransform.localRotation.z + Random.Range(-20f, 20f));
        m_NewPackSequence.Append(PackRectTransform.DOShakeRotation(CapsuleTweenShakeTime, shakeRotation));
        m_NewPackSequence.AppendCallback(() => {
            CardImage.sprite = capsuleData.PackData.CardData.Sprite;
            CardImage.gameObject.SetActive(true);
        });
        m_NewPackSequence.Append(PackRectTransform.DOAnchorPos(SpawnRectTransform.anchoredPosition, CapsuleTweenToSpawnPositionTime));
        m_NewPackSequence.AppendCallback(() => PackRectTransform.gameObject.SetActive(false));
    }
}
