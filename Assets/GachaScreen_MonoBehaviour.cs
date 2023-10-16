using UnityEngine;

public class GachaScreen_MonoBehaviour : GameBehaviour {
    public GachaScreen_PocketController PocketController => Scene_GachaScreen.Instance.PocketController;
    public GachaScreen_CapsuleController CapsuleController => Scene_GachaScreen.Instance.CapsuleController;
    public GachaScreen_PackController PackController => Scene_GachaScreen.Instance.PackController;
}