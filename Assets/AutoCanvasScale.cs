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

        //Rect oRect = GUIRectWithObject(gameObject);
        //rectTransform.sizeDelta = Vector2.Min(MaxResolution, oRect.size);


    }

    //public static Rect GUIRectWithObject(GameObject go) {
    //    Vector3 cen = go.GetComponent<Renderer>().bounds.center;
    //    Vector3 ext = go.GetComponent<Renderer>().bounds.extents;
    //    Vector2[] extentPoints = new Vector2[8]
    //     {
    //           WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z-ext.z)),
    //           WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z-ext.z)),
    //           WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z+ext.z)),
    //           WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z+ext.z)),
    //           WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z-ext.z)),
    //           WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z-ext.z)),
    //           WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z+ext.z)),
    //           WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z+ext.z))
    //     };
    //    Vector2 min = extentPoints[0];
    //    Vector2 max = extentPoints[0];
    //    foreach (Vector2 v in extentPoints) {
    //        min = Vector2.Min(min, v);
    //        max = Vector2.Max(max, v);
    //    }
    //    return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
    //}

    //public static Vector2 WorldToGUIPoint(Vector3 world) {
    //    Vector2 screenPoint = Camera.main.WorldToScreenPoint(world);
    //    screenPoint.y = (float)Screen.height - screenPoint.y;
    //    return screenPoint;
    //}
}
