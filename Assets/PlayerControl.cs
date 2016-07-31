using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class PlayerControl : Entity {

    private CharacterController cc;
    private Camera mainCam;
    private Vector3 moveDir = Vector3.zero;
    private float sideLook = 0.0f;
    private float upLook = 0.0f;
    private int Tick = 0;

    public float RunSpeed = 10.0f;
    public float Speed = 6.0f;
    public float WalkClamp = 0.5f;
    public float JumpSpeed = 8.0f;
    public float Gravity = 20.0f;

    public Animator PlayerAnim = null;
    public MovementInput InputController = null;
    public TurningInput TurningController = null;
    public Transform LowCamera = null;
    public Transform MidCamera = null;
    public Transform HighCamera = null;
    public Transform ActionCamera = null;
    public Transform CamHolder = null;
    public Transform Head = null;

    private Vector3 interactPos = Vector3.zero;
    private Quaternion interactRot = Quaternion.identity;

    private Animator PlayerHealthBar;

    public PointOfInterest Interest = null;

    private bool Attacking = false;

    public bool Interacting {
        get {
            return _interacting;
        }
        set {
            _interacting = value;
            if (Interest != null && _interacting == true) {
                Transform t = Head;
                Transform i = Interest.transform;
                bool left = mainCam.WorldToScreenPoint(i.position).x < mainCam.pixelWidth / 2;
                Vector3 dirToInterest = (t.position - i.position);
                float dist = dirToInterest.magnitude;
                float halfDist = dist / 2;
                dirToInterest.Normalize();
                Transform p = new GameObject().transform;
                Vector3 midPoint = (t.position + i.position) / 2;
                p.position = midPoint;
                p.LookAt(i);
                p.position += p.right * (halfDist * 1.5f) * (left ? -1 : 1);
                p.RotateAround(midPoint, p.up, 70f * (left ? -1 : 1));
                p.LookAt(midPoint);
                interactPos = p.position;
                interactRot = p.rotation;
                Destroy(p.gameObject);
            }
        }
    }
    private bool _interacting = false;

    public bool TurningEnabled {
        get {
            return TurningController.enabled;
        }
        set {
            TurningController.enabled = value;
        }
    }
    public bool ShowHealth {
        get {
            return PlayerHealthBar.GetBool("showing");
        }
        set {
            PlayerHealthBar.SetBool("showing", value);
        }
    }
    public Targettable Target {
        get {
            return _target;
        }
        set {
            _target = value;
            CameraLock = true;
        }
    }
    private Targettable _target;
    public bool Targetting {
        get {
            return Target != null;
        }
        set {
            if (!value) {
                Target = null;
                CameraLock = false;
            }
        }
    }

    private Transform interestDir = null;
    private List<PointOfInterest> pointsOfInterest = new List<PointOfInterest>();

    private InputControlBase inputControlBase;

    public bool CameraLock = false;

	// Use this for initialization
	void Start () {
        cc = GetComponent<CharacterController>();
        mainCam = Camera.main;
        if(PlayerAnim == null) {
            throw (new System.Exception("Player Control MUST have Animator linked!"));
        }
        if(InputController == null) {
            throw (new System.Exception("Player Control MUST have InputController linked!"));
        }
        if (TurningController == null) {
            throw (new System.Exception("Player Control MUST have TurningController linked!"));
        }
        PlayerAnim.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y);
        PlayerHealthBar = GameObject.Find("PlayerHealth").GetComponent<Animator>();
        TurningInput.Swipe += SwipeEvent;
	}
    void Awake () {
        inputControlBase = FindObjectOfType<InputControlBase>();
    }
	
	// Update is called once per frame
	void Update () {
        // Directly related to camera look position
        if (!CameraLock) {
            CamHolder.Rotate(new Vector3(0f, TurningController.Delta.x) * 360f);

            upLook += TurningController.Delta.y * 2;
            upLook = Mathf.Clamp(upLook, -1.0f, 1.0f);
            if (Interacting == false) {
                if (upLook > 0) {
                    mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, Vector3.Lerp(MidCamera.position, LowCamera.position, upLook), Time.deltaTime * 5f);
                    mainCam.transform.rotation = Quaternion.Slerp(mainCam.transform.rotation, Quaternion.Lerp(MidCamera.rotation, LowCamera.rotation, upLook), Time.deltaTime * 5f);

                } else {
                    mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, Vector3.Lerp(MidCamera.position, HighCamera.position, Mathf.Abs(upLook)), Time.deltaTime * 5f);
                    mainCam.transform.rotation = Quaternion.Slerp(mainCam.transform.rotation, Quaternion.Lerp(MidCamera.rotation, HighCamera.rotation, Mathf.Abs(upLook)), Time.deltaTime * 5f);
                }
            } else {
                mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, interactPos, Time.deltaTime * 5f);
                mainCam.transform.rotation = Quaternion.Slerp(mainCam.transform.rotation, interactRot, Time.deltaTime * 5f);
            }
        } else if (Targetting) {
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, ActionCamera.position, Time.deltaTime * 5f);
            mainCam.transform.rotation = Quaternion.Slerp(mainCam.transform.rotation, Quaternion.Slerp(ActionCamera.rotation, Quaternion.LookRotation(Target.transform.position - ActionCamera.position), .5f), Time.deltaTime * 5f);
        }

        // Movement
        if (!Attacking) {
            Vector3 InputVec = new Vector3(InputController.Value.x, 0, InputController.Value.y);
            if (cc.isGrounded) {
                moveDir = InputVec;
                if (Targetting)
                    moveDir = PlayerAnim.transform.TransformDirection(moveDir);
                else
                    moveDir = CamHolder.TransformDirection(moveDir);
                moveDir *= RunSpeed;
            }
            if (Targetting) {
                Vector3 dir = Target.transform.position - transform.position;
                dir.y = 0;
                PlayerAnim.transform.rotation = Quaternion.LookRotation(dir);
                CamHolder.transform.rotation = PlayerAnim.transform.rotation;
                PlayerAnim.SetFloat("velocity_x", Mathf.Lerp(PlayerAnim.GetFloat("velocity_x"), InputVec.x, Time.deltaTime * 5f));
                PlayerAnim.SetFloat("velocity_z", Mathf.Lerp(PlayerAnim.GetFloat("velocity_z"), InputVec.z, Time.deltaTime * 5f));

                Vector3 p = ActionCamera.localPosition;
                Vector3 r = ActionCamera.localEulerAngles;
                if (InputVec.x < 0) {
                    p.x = -1 * Mathf.Abs(p.x);
                    r.y = 21;
                } else if (InputVec.x > 0) {
                    p.x = Mathf.Abs(p.x);
                    r.y = 339;
                }
                ActionCamera.localPosition = p;
                ActionCamera.localEulerAngles = r;
            } else {
                Vector3 tv = new Vector3(moveDir.x, 0, moveDir.z);
                if (tv != Vector3.zero) {
                    print(moveDir);
                    PlayerAnim.transform.rotation = Quaternion.Slerp(PlayerAnim.transform.rotation, Quaternion.LookRotation(tv), Time.deltaTime * 5f);
                }
                PlayerAnim.SetFloat("velocity_z", Mathf.Lerp(PlayerAnim.GetFloat("velocity_z"), InputVec.magnitude, Time.deltaTime * 5f));
            }
            moveDir.y -= Gravity * Time.deltaTime;
            cc.Move(moveDir * Time.deltaTime);
        }


        // Player character look direction TODO: use Vector3.Dot
        float lookdist = CamHolder.eulerAngles.y - PlayerAnim.transform.eulerAngles.y;
        if (Mathf.Abs(lookdist) < 90f) {
            sideLook = lookdist / 90f;
        } else {
            sideLook = 0;
        }
        // Points of Interest
        if (Tick % 20 == 0) {
            Interest = FindBestInterest();
        }
        if (Interest == null && !Targetting) {
            PlayerAnim.SetFloat("look_x", Mathf.Lerp(PlayerAnim.GetFloat("look_x"), sideLook, Time.deltaTime * 5f));
            PlayerAnim.SetFloat("look_z", Mathf.Lerp(PlayerAnim.GetFloat("look_z"), upLook, Time.deltaTime * 5f));
        } else {
            Vector3 dirToInterest;
            if (Targetting)
                dirToInterest = (Target.transform.position - Head.position).normalized;
            else
                dirToInterest = (Interest.transform.position - Head.position).normalized;
            float dForward = Vector3.Dot(dirToInterest, Head.forward);
            if (dForward > 0) {
                float slook = Vector3.Dot(dirToInterest, Head.right);
                float ulook = Vector3.Dot(dirToInterest, Head.up);
                PlayerAnim.SetFloat("look_x", Mathf.Lerp(PlayerAnim.GetFloat("look_x"), slook, Time.deltaTime * 5f));
                PlayerAnim.SetFloat("look_z", Mathf.Lerp(PlayerAnim.GetFloat("look_z"), ulook, Time.deltaTime * 5f));
            } else {
                Interest = FindBestInterest();
            }
        }

        // DEBUG
        if(Input.GetKeyDown(KeyCode.F)) {
            Toolbox.Instance.ShowMessage("Ass Pass Last Fast\nMahnigga");
        }

        Tick++;
    }

    private void SwipeEvent(TurningInput.SwipeDirection direction) {
        if (Targetting) {
            Attacking = true;
            switch (direction) {
                case TurningInput.SwipeDirection.Up: PlayerAnim.SetTrigger("swing_up"); break;
                case TurningInput.SwipeDirection.Down: PlayerAnim.SetTrigger("swing_down"); break;
                case TurningInput.SwipeDirection.Left: PlayerAnim.SetTrigger("swing_left"); break;
                case TurningInput.SwipeDirection.Right: PlayerAnim.SetTrigger("swing_right"); break;
            }
        }
    }
    public void DoneAttacking() {
        Attacking = false;
    }

    private PointOfInterest FindBestInterest() {
        foreach(PointOfInterest p in pointsOfInterest) {
            if(IsBestPointofInterest(p)) {
                return p;
            }
        }
        return null;
    }
    private bool IsBestPointofInterest(PointOfInterest p) {
        Transform t = p.transform;
        Vector3 dirToTarget = t.position - transform.position;
        float dSqrToTarget = dirToTarget.sqrMagnitude;
        if (dSqrToTarget < p.NoticeDistance * p.NoticeDistance) { // Within spotting distance
            float dot = Vector3.Dot(dirToTarget, PlayerAnim.transform.forward);
            if (dot > 0) { // Infront of player
                if (Interest == null) {
                    return true;
                } else {
                    if (p.Priority >= Interest.Priority) {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void NotifyOfInterest(PointOfInterest p) {
        pointsOfInterest.Add(p);
    }
    public void RemoveInterest(PointOfInterest p) {
        pointsOfInterest.Remove(p);
    }
}
