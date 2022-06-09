using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody myBody;
    
    private SwerveInputSystem swerveInputSystem;
    [SerializeField]
    private float swerveSpeed = 0.5f;
    [SerializeField]
    private float maxSwerveAmount = 1f;

    public float moveSpeed = 10f;
    private Vector3 moveTemp;
    [SerializeField]
    private float minX, maxX;

    void Awake()
    {
        myBody = GetComponent<Rigidbody>();
  
        swerveInputSystem = GetComponent<SwerveInputSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        movePlayer();
    }
    void movePlayer()
    {
        float swerveAmount = Time.deltaTime * swerveSpeed * swerveInputSystem.MoveFactorX;
        swerveAmount = Mathf.Clamp(swerveAmount, -maxSwerveAmount, maxSwerveAmount);
        transform.Translate(swerveAmount, 0, 0);
    }
}
