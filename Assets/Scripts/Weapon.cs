using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Weapon : MonoBehaviour {

    private Collider col;
    public Entity Owner;
    public float BaseDamage;

	// Use this for initialization
	void Start () {
        col = GetComponent<Collider>();
        col.isTrigger = true;
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
