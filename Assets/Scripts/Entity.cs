using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

    // Name for use in display
    public string DispName;

    public string Level {
        get {
            return "lvl " + skillLevel.ToString(); 
        }
    }
    private int skillLevel;

    public EntityHealth Health {
        get {
            return _health;
        }
    }
    public bool HasHealth {
        get {
            return _health != null;
        }
    }
    private EntityHealth _health;

    void Awake() {
        _health = GetComponent<EntityHealth>();
    }

    // Stuff like death animations and such will go here
    protected virtual void Die(DamageSource source) {

    }
    protected virtual void Die() {

    }

    protected void DoDamage(DamageSource source) {
        if(HasHealth) {
            TakeDamage(source);
            Health.Health -= source.Damage; // Do some testing here, check if side effects work
        }
    }

    // Take damage animations, check for death, resistance, etc.
    protected virtual void TakeDamage(DamageSource source) {
        
    }
}
