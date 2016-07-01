using UnityEngine;
using System.Collections;

public class EntityHealth : MonoBehaviour {

    public float MaxHealth;
    public float Health;

    public float Percentage {
        get {
            return Health / MaxHealth;
        }
    }
    public bool Overhealed {
        get {
            return Health > MaxHealth;
        }
    }
    public bool Dead {
        get {
            return Health <= 0;
        }
    }
}
