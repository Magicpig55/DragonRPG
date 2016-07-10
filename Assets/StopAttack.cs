using UnityEngine;
using System.Collections;

public class StopAttack : StateMachineBehaviour {

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Toolbox.Instance.Player.DoneAttacking();
    }
}
