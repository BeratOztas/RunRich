using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using DG.Tweening;

public class PlayerRunner : MonoBehaviour
{
    [SerializeField] private Transform localMover;
    [SerializeField] private Transform localMoverTarget;
    [SerializeField] private PathCreator pathCreator;
  
    [SerializeField] private SimpleAnimancer animancer;
    [SerializeField] private PlayerSwerve playerSwerve;

    [Header("Animations")]
    [SerializeField] private string idleAnimName = "Idle";
    [SerializeField] private float idleAnimSpeed = 1f;

    [SerializeField] private string sadWalkAnim_Name = "Sad Walking";
    [SerializeField] private float sadWalkAnim_speed = 0.5f;

    [SerializeField] private string averageWalkAnim_Name = "Average Walking";
    [SerializeField] private float averagewalkAnim_speed = 2f;

    [SerializeField] private string richwalkAnim_Name = "Rich Walking";
    [SerializeField] private float richwalkAnim_speed = 2f;

    [SerializeField] private string sadSpinAnim_Name = "Sad Spinning";
    [SerializeField] private float sadSpinAnim_Speed = 1f;

    [SerializeField] private string goodSpinAnim_Name = "Good Spinning";
    [SerializeField] private float goodSpinAnim_Speed = 1f;

    [SerializeField] private string sadFinishAnim_Name = "Bad Finish";
    [SerializeField] private float sadFinishAnim_Speed = 1f;
    
    [SerializeField] private string goodFinishAnim_Name = "Good Finish";
    [SerializeField] private float goodFinishAnim_Speed = 1f;

    [SerializeField] private string currentAnimName = "Sad Walking";
    //sad ,walk ,rich, idle, spin happy, spin sad 
    //end of the game dance or sad
    [Space]
    [SerializeField] private float startDistance = 5f;
    [SerializeField] private float forwardSpeed = 1f;
    [SerializeField] private float strafeSpeed = 1f;
    [SerializeField] private float strafeLerpSpeed = 1f;
    [SerializeField] private float clampLocalX = 2f;
    [SerializeField] private float rotateSpeed = 100f;
    [SerializeField] private float rotateAngle = 100f;
    [Space]
    [SerializeField] private float dodgeBackDistance = 2f;
    [SerializeField] private float dodgeBackDuration = 2f;
    [SerializeField] private bool enabled = true;
    [SerializeField] private bool rotateEnabled = true;



    private Vector3 oldPosition;
    public float distance = 0;
    public bool running = true;
    private bool canSwerve = true;
    private bool dodgingBack = false;
    private Tweener forwardSpeedTween;

    

    void Awake()
    {
        playerSwerve.OnSwerve += PlayerSwerve_OnSwerve;
        distance = startDistance;
        oldPosition = localMoverTarget.position;
      
    }

    public void Init() {
        pathCreator = FindObjectOfType<PathCreator>();
    }
    void Update()
    {
        MoveForward();
        UpdatePath();
        FollowLocalMoverTarget();
        UpdateRotation();
        oldPosition = localMover.position;

       

    }
    void UpdateRotation() {
        if (!enabled)
            return;
        if (!rotateEnabled)
            return;

        Vector3 direction = localMoverTarget.position - oldPosition;
        
        animancer.GetAnimatiorTransform().forward = Vector3.Lerp(animancer.GetAnimatiorTransform().forward, direction, rotateSpeed * Time.deltaTime);
    }
    void FollowLocalMoverTarget() {
        if (!canSwerve)
            return;

        Vector3 nextPos = new Vector3(localMoverTarget.localPosition.x, localMoverTarget.localPosition.y, localMoverTarget.localPosition.z);
        localMover.localPosition = Vector3.Lerp(localMover.localPosition, nextPos, strafeLerpSpeed * Time.deltaTime);
        
    }


    public void SetSwerve(bool value)
    {
        canSwerve = value;
    }
    public void SetEnabled(bool value)
    {
        enabled = value;
    }
    public void SetRotateEnabled(bool value)
    {
        rotateEnabled = value;
    }

    void PlayerSwerve_OnSwerve(Vector2 direction) { 
        if(running&& canSwerve) {
            localMoverTarget.localPosition = localMoverTarget.localPosition + Vector3.right * direction.x * strafeSpeed * Time.deltaTime;
            ClampLocalPosition();
            
        }
    }
    void ClampLocalPosition() {
        Vector3 pos = localMoverTarget.localPosition;
        
        pos.x = Mathf.Clamp(pos.x, -clampLocalX, clampLocalX);
        localMoverTarget.localPosition = pos;
    }
    
    void MoveForward() {
        if (enabled && running && !dodgingBack)
            distance += forwardSpeed * Time.deltaTime;
    }
    public float GetForwardSpeed() {
        return forwardSpeed;
    }
    public void SetForwardSpeed(float value) { 
        if(forwardSpeedTween !=null)
        {
            forwardSpeedTween.Kill();
        }
        forwardSpeed = value;
    }

    public void SetForwardSpeed(float value,float duration) {
        if (forwardSpeedTween != null)
        {
            forwardSpeedTween.Kill();
        }
        forwardSpeedTween = DOTween.To(() => forwardSpeed, x => forwardSpeed = x, value, duration);
    }
    public void SetLocalRotation(Vector3 eulerAngles) {
        animancer.transform.localEulerAngles = eulerAngles;
    }
    


    void UpdatePath() {
        if (enabled) {
            transform.position = pathCreator.path.GetPointAtDistance(distance);
            transform.eulerAngles = pathCreator.path.GetRotationAtDistance(distance).eulerAngles + new Vector3(0f, 0f, 90f);

        }
    }
    public Transform GetAnimancerTransform() {
        return animancer.transform;
    }
    
    
    public void StartToRun() {
        if (enabled) {
            running = true;
            Debug.Log(running);
            sadWalkAnimation();
        }
    }

   

    public void PlayAnimation(string animName,float animSpeed) {
        animancer.PlayAnimation(animName);
        animancer.SetStateSpeed(animSpeed);
    }
    public void idleAnimation() {
        PlayAnimation(idleAnimName, idleAnimSpeed);
    }
    public void sadWalkAnimation() {
        PlayAnimation(sadWalkAnim_Name, sadWalkAnim_speed);
        currentAnimName = sadWalkAnim_Name;
    }
    public void averageWalkAnimation() {
        PlayAnimation(averageWalkAnim_Name, averagewalkAnim_speed);
        currentAnimName = averageWalkAnim_Name;
    }
    public void richWalkAnimation() {
        PlayAnimation(richwalkAnim_Name, richwalkAnim_speed);
        currentAnimName=richwalkAnim_Name;
    }
    public void sadSpinAnimation() {
        PlayAnimation(sadSpinAnim_Name, sadSpinAnim_Speed);
    }
    public void richSpinAnimation() {
        PlayAnimation(richwalkAnim_Name, richwalkAnim_speed);
    }
    public void sadFinishAnimation() {
        PlayAnimation(sadFinishAnim_Name, sadFinishAnim_Speed);
    }
    public void richFinishAnimation() {
        PlayAnimation(richwalkAnim_Name, richwalkAnim_speed);
    }
    public bool IsDodgingBack()
    {
        return dodgingBack;
    }

    public void DodgeBack()
    {
        StartCoroutine(DodgeBackProcess());
    }

    IEnumerator DodgeBackProcess()
    {
        canSwerve = false;
        running = false;
        animancer.PlayAnimation("Hit");

        yield return new WaitForSeconds(0.933f);

        animancer.PlayAnimation(currentAnimName);
        running = true;
        canSwerve = true;
    }


}
