using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableRotate : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.1f;
    private float angle;
    public bool isMoney;
    void Update()
    {
        if (isMoney) { 
        angle = (angle + speed) % 360f;
        transform.localRotation = Quaternion.Euler(new Vector3(45f, angle, 0f));
        }
        else {
            angle = (angle + speed) % 360f;
            transform.localRotation = Quaternion.Euler(new Vector3(-128.84f, angle, -84.918f));
        }
    }
}
