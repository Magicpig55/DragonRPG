using UnityEngine;
using System.Collections;

public class InputControlBase : MonoBehaviour {
    public bool Enabled {
        get {
            return _enabled;
        }
        set {
            _enabled = value;
            foreach(Animator child in Children) {
                child.SetBool("enabled", _enabled);
            }
        }
    }
    private bool _enabled = true;

    private Animator[] Children;

    void Start() {
        Children = GetComponentsInChildren<Animator>();
    }
}
