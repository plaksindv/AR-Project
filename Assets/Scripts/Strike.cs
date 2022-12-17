using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strike : MonoBehaviour
{
    private ProgrammManager ProgrammManagerScript;
    private bool killed = false;

    private void Start()
    {
        ProgrammManagerScript = FindObjectOfType<ProgrammManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!killed && collision.gameObject.name == "Shell(Clone)")
        {
            ProgrammManagerScript.Strikes += 1;
            killed = true;
        }
    }

}
