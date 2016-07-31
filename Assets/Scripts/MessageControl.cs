using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Animator))]
public class MessageControl : MonoBehaviour {

    private Button butt;
    private Animator anim;
    private Text text;

    public string Text {
        get {
            return lines[currentLine];
        }
        set {
            lines = value.Split(new string[] { "<nl>" }, StringSplitOptions.None);
            for(int i = 0; i < lines.Length; i++) {
                lines[i] = lines[i].Trim();
            }
            currentLine = 0;
        }
    }
    private string[] lines;
    private int currentLine;
    private int currentChar;

    private float waitTime = 0.0f;
    private float timer = 0.0f;

    private const float slowModeWait = 0.25f;
    
    private float textDispDelay {
        get {
            return Toolbox.Options.TextDispDelay;
        }
    }

    private bool slowMode = false;
    private bool noSkip = false;
    private bool noWait = false;
    private bool waitingForInput = false;
    private bool lineComplete = false;
    private bool complete = false;
    private bool waitingForAnim = false;
    private bool persist = false;

    private string dispText {
        get {
            return text.text;
        }
        set {
            text.text = value;
        }
    }
    private string currentText {
        get {
            return lines[currentLine];
        }
    }

	// Use this for initialization
	void Start () {
        butt = GetComponent<Button>();
        anim = GetComponent<Animator>();
        butt.onClick.AddListener(() => Clicked());
        text = GetComponentInChildren<Text>();
        currentChar = 0;
        waitTime = textDispDelay;
        dispText = "";
	}

    void Update () {
        if (!waitingForInput && !waitingForAnim) {
            timer += Time.deltaTime;
            if (timer >= waitTime) {
                if (currentChar >= currentText.Length) {
                    if (noWait) {
                        anim.SetTrigger("change");
                        waitingForAnim = true;
                        currentLine++;
                        currentChar = 0;
                        if (!persist) {
                            noSkip = false;
                            noWait = false;
                            slowMode = false;
                        }
                    } else { 
                        waitingForInput = true;
                        lineComplete = true;
                    }
                    if (currentLine == lines.Length - 1)
                        complete = true;
                } else {
                    char c = currentText[currentChar];
                    if (c == '<') {
                        string s = currentText.Substring(currentChar);
                        string q = s.Substring(1, s.IndexOf(">") - 1);
                        print(q);
                        if (q.StartsWith("wait")) {
                            float i;
                            string st = q.Substring(q.IndexOf(':') + 1);
                            float.TryParse(st, out i);
                            waitTime = i;
                        } else {
                            switch (q) {
                                case "inputwait": waitingForInput = true; break;
                                case "noskip": noSkip = true; break;
                                case "/noskip": noSkip = false; break;
                                case "slow": slowMode = true; break;
                                case "/slow": slowMode = false; break;
                                case "dontwait": noWait = true; break;
                                case "/dontwait": noWait = false; break;
                                case "persist": persist = true; break;
                                case "/persist": persist = false; break;
                            }
                            waitTime = slowMode ? slowModeWait : textDispDelay;
                        }
                        timer = 0;
                        currentChar += s.IndexOf(">") + 1;
                    } else {
                        dispText += c;
                        waitTime = slowMode ? slowModeWait : textDispDelay;
                        timer = 0;
                        currentChar++;
                    }
                }
            }
        }
    }

    void Clicked () {
        if (waitingForInput) {
            waitingForInput = false;
            waitTime = slowMode ? slowModeWait : textDispDelay;
            timer = 0;
            if (complete) {
                anim.SetTrigger("close");
                Destroy(gameObject, 1);
                Toolbox.InputControl.enabled = true;
                Toolbox.Player.Interacting = false;
                Toolbox.Player.ShowHealth = true;
            } else if (lineComplete) {
                anim.SetTrigger("change");
                lineComplete = false;
                waitingForAnim = true;
                currentLine++;
                currentChar = 0;
                if(!persist) {
                    noSkip = false;
                    noWait = false;
                    slowMode = false;
                }
            }
        } else {
            if (!noSkip) {
                waitingForInput = true;
                dispText = currentText;
                dispText = Regex.Replace(dispText, "\\<.*?\\>", "");
                lineComplete = true;
                if (currentLine == lines.Length - 1)
                    complete = true;
            }
        }
    }

    void SwitchText() {
        dispText = "";
        waitingForAnim = false;
    }
}
