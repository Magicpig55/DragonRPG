using UnityEngine;
using System.Collections;

public class PointOfInterest : MonoBehaviour {

    public float NoticeDistance = 10f;
    public float Priority = 0;

    void Awake () {
        PlayerControl pc = FindObjectOfType<PlayerControl>() as PlayerControl;
        pc.NotifyOfInterest(this);
    }

    public override string ToString() {
        return transform.position.ToString();
    }
}
