using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 60;
    }
}
