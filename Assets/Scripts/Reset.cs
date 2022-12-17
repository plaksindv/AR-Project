using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    private ProgrammManager ProgrammManagerScript;

    
    void Start()
    {
        ProgrammManagerScript = FindObjectOfType<ProgrammManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        ProgrammManagerScript.Recharging = false;
    }
}
