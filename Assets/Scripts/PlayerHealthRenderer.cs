using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(EntityHealth))]
public class PlayerHealthRenderer : MonoBehaviour {

    private Image uiImage;
    private RectTransform Healthbar;
    private Text hpText;
    private GameObject oHealthbar;
    private EntityHealth health;

    private float fullWidth;

	// Use this for initialization
	void Start () {
        oHealthbar = GameObject.Find("PlayerHealth").gameObject;
        health = GetComponent<EntityHealth>();

        hpText = oHealthbar.transform.Find("Text").GetComponent<Text>();
        Healthbar = oHealthbar.transform.Find("Healthbar").GetComponent<RectTransform>();
        uiImage = oHealthbar.transform.Find("Portrait").GetComponent<Image>();

        fullWidth = Healthbar.rect.width;
	}
	
	// Update is called once per frame
	void Update () {
        Healthbar.sizeDelta = Vector2.Lerp(Healthbar.sizeDelta, new Vector2(fullWidth * health.Percentage, Healthbar.sizeDelta.y), Time.deltaTime * 3f);
        hpText.text = string.Format("{0}/{1}", health.Health, health.MaxHealth);
	}
}
