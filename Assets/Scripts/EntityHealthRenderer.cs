using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(EntityHealth))]
[RequireComponent(typeof(Entity))]
public class EntityHealthRenderer : MonoBehaviour {

    public const string NAME_DISPLAY_FORMAT = "{0} <size=24>{1}</size>";

    private EntityHealth entityHealth;
    private Entity entity;
    private Text hpText;
    public GameObject Healthbar;
    private GameObject entityHealthPrefab;
    private Animator HealthbarAnimator;
    private Camera mainCam;
    public Vector3 HPBarOffset = Vector3.zero;

	// Use this for initialization
	void Start () {
        entityHealthPrefab = Resources.Load("prefabs/ui/EnemyHealthbar") as GameObject;
        Healthbar = Instantiate(entityHealthPrefab) as GameObject;
        HealthbarAnimator = Healthbar.GetComponent<Animator>();
        Healthbar.transform.SetParent(transform, false);
        hpText = Healthbar.GetComponentInChildren<Text>();

        entityHealth = GetComponent<EntityHealth>();
        entity = GetComponent<Entity>();

        hpText.text = string.Format(NAME_DISPLAY_FORMAT, entity.DispName, entity.Level);

        mainCam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        Healthbar.transform.forward = mainCam.transform.forward;
        Healthbar.transform.localPosition = HPBarOffset;
        HealthbarAnimator.SetFloat("hp_percentage", Mathf.Lerp(HealthbarAnimator.GetFloat("hp_percentage"), entityHealth.Percentage, Time.deltaTime * 3f));
        HealthbarAnimator.SetFloat("hp_percInv", Mathf.Lerp(3f, 1f, Mathf.Clamp01(entityHealth.Percentage * 4f)));
    }
}
