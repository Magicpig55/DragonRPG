using UnityEngine;
using System.Collections;

public class DamageSource {
    public GameObject Attacker;
    public GameObject Victim;
    public float Damage;

    public DamageSource(GameObject att, GameObject vic, float dam) {
        Attacker = att;
        Victim = vic;
        Damage = dam;
    }
}
