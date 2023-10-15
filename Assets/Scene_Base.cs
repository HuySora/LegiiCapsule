using UnityEngine;

public class Scene_Base : MonoBehaviour {
    public virtual void Open() {
        gameObject.SetActive(true);
    }
    public virtual void Close() {
        gameObject.SetActive(false);
    }
}
