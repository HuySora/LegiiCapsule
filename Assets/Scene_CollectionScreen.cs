using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Scene_CollectionScreen : Scene_Base {
    [field: Header("References")]
    [field: SerializeField] public CollectionScreen_CollectionController CollectionController { get; private set; }
    [field: SerializeField] public Button BackButton { get; private set; }
    [field: SerializeField] public UnityEvent BackButtonClickedEvent { get; private set; }  
    private void Start() {
        BackButton.onClick.AddListener(() => BackButtonClickedEvent.Invoke());
    }
    public override void Open() {
        CollectionController.Initialize();
        base.Open();
    }
}
