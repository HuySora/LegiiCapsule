using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GachaScreen_CoinDropArea : GachaScreen_MonoBehaviour, IDropHandler {
    public UnityEvent<GachaScreen_Coin> OnCoinDrop;
    public void OnDrop(PointerEventData data) {
        if (!data.pointerDrag.TryGetComponent(out GachaScreen_Coin coin)) {
            return;
        }
        OnCoinDrop?.Invoke(coin);
    }
}