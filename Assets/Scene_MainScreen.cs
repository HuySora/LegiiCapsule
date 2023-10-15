using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Scene_MainScreen : Scene_Base {
    [field: Header("References")]
    [field: SerializeField] public Button StartButton { get; private set; }
    [field: SerializeField] public UnityEvent StartButtonClickedEvent { get; private set; }
    [field: SerializeField] public Button TutorialButton { get; private set; }
    [field: SerializeField] public UnityEvent TutorialButtonClickedEvent { get; private set; }
    [field: SerializeField] public Button ExitButton { get; private set; }
    [field: SerializeField] public UnityEvent ExitButtonClickedEvent { get; private set; }
    private void Start() {
        StartButton.onClick.AddListener(() => StartButtonClickedEvent.Invoke());
        TutorialButton.onClick.AddListener(() => TutorialButtonClickedEvent.Invoke());
        ExitButton.onClick.AddListener(() => ExitButtonClickedEvent.Invoke());
    }
}
