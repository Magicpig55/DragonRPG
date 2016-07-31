using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class TurningInput : MonoBehaviour {

    bool Tracking = false;

    private RectTransform tf;

    public CanvasScaler canvas;
    public Vector2 Delta;

    private int finger;
    private float wscale;
    private float hscale;

    public bool Disabled = false;

    public enum SwipeDirection {
        Up, Right, Down, Left
    }

    // Use this for initialization
    void Start () {
        tf = GetComponent<RectTransform>();
        Delta = Vector2.zero;
        wscale = canvas.referenceResolution.x / Screen.width;
        hscale = canvas.referenceResolution.y / Screen.height;
	}

    public static event Action<SwipeDirection> Swipe;
    public float SwipeDistance = 50f;
    private bool swipeEventSent;
    private Vector2 swipeStartPos;
	
	// Update is called once per frame
	void Update () {
        EventSystem eventSystem = EventSystem.current;

#if UNITY_STANDALONE || UNITY_WEBPLAYER
        Value = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE

        if (!eventSystem.alreadySelecting && !Disabled) {
            for (int i = 0; i < Input.touchCount; i++) {
                Touch touch = Input.GetTouch(i);
                if (touch.phase == TouchPhase.Began) {
                    if (Helper.RectTransformToScreenSpace(tf).Contains(touch.position) && !Tracking) {
                        Tracking = true;
                        swipeStartPos = touch.position;
                        finger = touch.fingerId;
                        eventSystem.SetSelectedGameObject(tf.gameObject);
                    }
                }
                if (Tracking) {
                    if (touch.fingerId == finger) {
                        Vector2 t = touch.deltaPosition;
                        if (!swipeEventSent && Swipe != null) {
                            Vector2 p = touch.position - swipeStartPos;
                            if (p.sqrMagnitude > SwipeDistance * SwipeDistance) {
                                if (Mathf.Abs(p.x) > Mathf.Abs(p.y)) {
                                    if (p.x > 0)
                                        Swipe(SwipeDirection.Right);
                                    else
                                        Swipe(SwipeDirection.Left);
                                } else {
                                    if (p.y > 0)
                                        Swipe(SwipeDirection.Up);
                                    else
                                        Swipe(SwipeDirection.Down);
                                }
                                print("Swipe!");
                                swipeEventSent = true;
                            }
                        }
                        t /= (wscale * hscale) / 2;
                        t.x /= tf.rect.width;
                        t.y /= tf.rect.height;
                        Delta = t;
                    }
                }
                if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && touch.fingerId == finger) {
                    Tracking = false;
                    swipeEventSent = false;
                    Delta = Vector2.zero;
                }
            }
        }
#endif
    }
}
