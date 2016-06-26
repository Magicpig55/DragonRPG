using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DebugFPS : MonoBehaviour {

    private Text text;

    float updateInterval = 0.5f;
    private float accum = 0.0f;
    private int frames = 0;
    private float timeleft;

    void Start() {
        text = GetComponent<Text>();
        timeleft = updateInterval;
    }

	// Update is called once per frame
	void Update () {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;
	    
        if(timeleft <= 0f) {
            text.text = "" + (accum / frames).ToString("f2");
            timeleft = updateInterval;
            accum = 0;
            frames = 0;
        }
	}
}
