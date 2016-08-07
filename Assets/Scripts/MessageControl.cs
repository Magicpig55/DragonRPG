using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
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

    private LabelLocation ReadLocation {
        set {
            currentLine = value.Line;
            currentChar = value.Char;
            locChanged = true;
        }
    }
    private bool locChanged = false;

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

    public struct LabelLocation {
        public int Line;
        public int Char;
        public LabelLocation(int line, int car) {
            Line = line;
            Char = car;
        }
        public override string ToString() {
            return string.Format("{0} {1}", Line, Char);
        }
    }

    private bool foundLabels = false;
    private Dictionary<string, LabelLocation> labels = new Dictionary<string, LabelLocation>();

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
        if(!foundLabels) {
            for (int i = 0; i < lines.Length; i++) {
                string line = lines[i];
                if(line.Contains("<label:")) {
                    string t = line.Substring(line.IndexOf("<label:"));
                    string lbl = t.Substring(t.IndexOf(':') + 1, t.IndexOf('>') - t.IndexOf(':'));
                    lbl = lbl.Substring(0, lbl.Length - 1);
                    labels.Add(lbl, new LabelLocation(i, line.IndexOf("<label:") + t.IndexOf('>') + 1));
                }
            }
            foundLabels = true;
        }
        if (!waitingForInput && !waitingForAnim) {
            timer += Time.deltaTime;
            locChanged = false;
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
                        string keyword;
                        string[] arguments = new string[2];
                        if (q.Contains(":")) {
                            string[] t = q.Split(':');
                            keyword = t[0];
                            Array.Copy(t, 1, arguments, 0, t.Length - 1);
                        } else {
                            keyword = q;
                        }
                        switch (keyword) {
                            case "inputwait": waitingForInput = true; break;
                            case "noskip": noSkip = true; break;
                            case "/noskip": noSkip = false; break;
                            case "slow": slowMode = true; break;
                            case "/slow": slowMode = false; break;
                            case "dontwait": noWait = true; break;
                            case "/dontwait": noWait = false; break;
                            case "persist": persist = true; break;
                            case "/persist": persist = false; break;
                            case "clear":
                                waitingForAnim = true;
                                anim.SetTrigger("change");
                                break;
                            case "close":
                                anim.SetTrigger("close");
                                Destroy(gameObject, 1);
                                Toolbox.InputControl.enabled = true;
                                Toolbox.Player.Interacting = false;
                                Toolbox.Player.ShowHealth = true;
                                break;
                            case "wait":
                                float i;
                                float.TryParse(arguments[0], out i);
                                waitTime = i;
                                break;
                            case "goto":
                                ReadLocation = labels[arguments[0]];
                                break;
                        }
                        waitTime = slowMode ? slowModeWait : textDispDelay;
                        timer = 0;
                        if (!locChanged)
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
