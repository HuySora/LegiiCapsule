using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Scene_GachaScreen : Scene_Base {
    public static Scene_GachaScreen Instance;
    [field: Header("References")]
    [field: SerializeField] public GachaScreen_PocketController PocketController { get; private set; }
    [field: SerializeField] public GachaScreen_CapsuleController CapsuleController { get; private set; }
    [field: SerializeField] public GachaScreen_PackController PackController { get; private set; }
    [field: SerializeField] public Button ExitButton { get; private set; }
    [field: SerializeField] public UnityEvent ExitButtonClickedEvent { get; private set; }
    [field: SerializeField] public Button CollectionButton { get; private set; }
    [field: SerializeField] public UnityEvent CollectionButtonClickedEvent { get; private set; }
    private void Awake() {
        Instance = this;
    }
    private void Start() {
        ExitButton.onClick.AddListener(() => ExitButtonClickedEvent.Invoke());
        CollectionButton.onClick.AddListener(() => CollectionButtonClickedEvent.Invoke());
    }
}
