using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Scene_GachaScreen : Scene_Base {
    [field: Header("References")]
    [field: SerializeField] public Button ExitButton { get; private set; }
    [field: SerializeField] public UnityEvent ExitButtonClickedEvent { get; private set; }
    private void Start() {
        ExitButton.onClick.AddListener(() => ExitButtonClickedEvent.Invoke());
    }
}
