﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(EntityHealth))]
[RequireComponent(typeof(Entity))]
public class EntityHealthRenderer : MonoBehaviour {

    public const string NAME_DISPLAY_FORMAT = "{0} <size=24>{1}</size>";

    private EntityHealth entityHealth;
    private Entity entity;

    private RectTransform hpBar;
    private float hpBarBase;

    private Text hpText;

    private GameObject Healthbar;

    private GameObject entityHealthPrefab;

    private Camera mainCam;

    public Vector3 HPBarOffset = Vector3.zero;

	// Use this for initialization
	void Start () {
        entityHealthPrefab = Resources.Load("prefabs/ui/EnemyHealthbar") as GameObject;
        Healthbar = Instantiate(entityHealthPrefab) as GameObject;
        Healthbar.transform.SetParent(transform, false);
        hpBar = Healthbar.transform.Find("Healthbar").gameObject.GetComponent<RectTransform>();
        hpText = Healthbar.GetComponentInChildren<Text>();

        entityHealth = GetComponent<EntityHealth>();
        entity = GetComponent<Entity>();

        hpText.text = string.Format(NAME_DISPLAY_FORMAT, entity.DispName, entity.Level);

        hpBarBase = hpBar.rect.width;

        mainCam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        hpBar.sizeDelta = Vector2.Lerp(hpBar.sizeDelta, new Vector2(hpBarBase * entityHealth.Percentage, -50), Time.deltaTime * 5f);
        Healthbar.transform.forward = mainCam.transform.forward;
        Healthbar.transform.localPosition = HPBarOffset;
    }
}