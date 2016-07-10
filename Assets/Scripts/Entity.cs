using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

    // Name for use in display
    public string DispName;

    private Animator hpAnimator;

    public string Level {
        get {
            return "lvl " + SkillLevel.ToString(); 
        }
    }
    public int SkillLevel;

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

    public void AttemptDamage(DamageSource source) {
        DoDamage(source);
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

            if(hpAnimator == null) {
                hpAnimator = GetComponent<EntityHealthRenderer>().Healthbar.GetComponent<Animator>();
            }
            hpAnimator.SetTrigger("blink");
        }
    }

    // Take damage animations, check for death, resistance, etc.
    protected virtual void TakeDamage(DamageSource source) {
        
    }
}
