using UnityEngine;
using System.Collections;

public class TouchMessage : MonoBehaviour {

    public string Message = "";
    public bool Focus = true;

    private PointOfInterest pointOfInterest;

    void Awake() {
        pointOfInterest = GetComponent<PointOfInterest>();
    }

	void OnMouseDown() {
        Toolbox.Instance.ShowMessage(Message, pointOfInterest, Focus);
    }
}
