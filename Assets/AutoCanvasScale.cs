using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class AutoCanvasScale : MonoBehaviour {

    public Vector2 MaxResolution;
    public Vector2 MinResolution;
    public float HideDistance;
    public float FullResDistance;

    private Camera mainCam;
    private RectTransform rectTransform;

	// Use this for initialization
	void Start () {
        mainCam = Camera.main;
        rectTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        float hDistSqr = HideDistance * HideDistance;
        float fDistSqr = FullResDistance * FullResDistance;
        Vector3 dirTo = transform.position - mainCam.transform.position;
        float dSqr = dirTo.sqrMagnitude;
        float p = Mathf.Clamp01((dSqr - fDistSqr) / (hDistSqr - fDistSqr));
        rectTransform.sizeDelta = Vector2.Lerp(MaxResolution, MinResolution, p);
	}
}
