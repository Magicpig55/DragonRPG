using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Weapon : MonoBehaviour {

    private Collider collider;
    public Entity Owner;
    public float BaseDamage;

	// Use this for initialization
	void Start () {
        collider = GetComponent<Collider>();
        collider.isTrigger = true;
	}
	void OnTriggerEnter (Collider other) {
        Entity entity = other.GetComponent<Entity>();
        if(entity != null) {
            if (entity != Owner) {
                entity.AttemptDamage(new DamageSource(gameObject, entity.gameObject, BaseDamage));
            }
        }
    }
}
