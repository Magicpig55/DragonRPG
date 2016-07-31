using UnityEngine;
using System.Collections;

public class InputControlBase : MonoBehaviour {
    public bool Enabled {
        get {
            return _enabled;
        }
        set {
            _enabled = value;
            turningInput.Disabled = !_enabled;
            foreach(Animator child in Children) {
                child.SetBool("enabled", _enabled);
            }
        }
    }
    private bool _enabled = true;

    private TurningInput turningInput;

    private Animator[] Children;

    public void DisableTurningInput() {
        turningInput.Disabled = true;
    }
    public void EnableTurningInput() {
        turningInput.Disabled = false;
    }
    void Awake() {
        turningInput = GetComponentInChildren<TurningInput>();
        Children = GetComponentsInChildren<Animator>();
    }
}
