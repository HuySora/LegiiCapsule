using DG.Tweening;
using UnityEngine;

public class GachaScreen_PocketController : GachaScreen_MonoBehaviour {
    [field: Header("References")]
    [field: SerializeField] public GachaScreen_Coin Coin { get; private set; }
    [field: SerializeField] public RectTransform SpawnRectTransform { get; private set; }
    [field: SerializeField] public RectTransform StationaryRectTransform { get; private set; }
    [field: Header("Data")]
    [field: SerializeField] public float TweenToStationaryPositionTime { get; private set; } = 1f;
    private bool m_CoinSpawned;
    private void Update() {
        if (m_CoinSpawned) {
            return;
        }
        if (EconomyManager.Coin <= 0) {
            return;
        }
        SpawnCoin();
    }
    public void ConsumeCoin(GachaScreen_Coin coin) {
        EconomyManager.Instance.RemoveCoin(1);
        Coin.gameObject.SetActive(false);
        Coin.ResetState();
        Coin.RectTransform.DOAnchorPos(SpawnRectTransform.anchoredPosition, 0f);
        m_CoinSpawned = false;
    }
    private void SpawnCoin() {
        m_CoinSpawned = true;
        Coin.gameObject.SetActive(true);
        Coin.RectTransform.DOAnchorPos(SpawnRectTransform.anchoredPosition, 0f).OnComplete(() => {
            Coin.RectTransform.DOAnchorPos(StationaryRectTransform.anchoredPosition, TweenToStationaryPositionTime);
        });
    }
}
