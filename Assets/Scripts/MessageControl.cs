using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Animator))]
public class MessageControl : MonoBehaviour {

    private Button butt;
    private Animator anim;
    private Text text;
    private Text nexttext;

    public string Text {
        get {
            return lines[currentLine];
        }
        set {
            lines = value.Split(new string[] { "//" }, StringSplitOptions.None);
            for(int i = 0; i < lines.Length; i++) {
                lines[i] = lines[i].Trim();
            }
            currentLine = 0;
        }
    }
    private string[] lines;
    private int currentLine;

	// Use this for initialization
	void Start () {
        butt = GetComponent<Button>();
        anim = GetComponent<Animator>();
        butt.onClick.AddListener(() => Clicked());
        Text[] t = GetComponentsInChildren<Text>();
        text = t[0];
        nexttext = t[1];
        text.text = lines[currentLine];
	}

    void Clicked () {
        if (currentLine == lines.Length - 1) {
            anim.SetTrigger("close");
            Destroy(gameObject, 1);
            Toolbox.Instance.InputControl.Enabled = true;
            Toolbox.Instance.Player.Interacting = false;
        } else {
            nexttext.text = lines[currentLine + 1];
            anim.SetTrigger("change");
            currentLine++;
        }
    }

    void SwitchText() {
        text.text = nexttext.text;
    }
}
