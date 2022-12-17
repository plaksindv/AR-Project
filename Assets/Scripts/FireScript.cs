using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireScript : MonoBehaviour
{
    private ProgrammManager ProgrammManagerScript;

    private Button button;
    private GameObject Beam;
    private Rigidbody BeamRigidBody;
    public float force;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(FireFunction);
        ProgrammManagerScript = FindObjectOfType<ProgrammManager>();
    }

    // Update is called once per frame
    void FireFunction()
    {
        Beam = GameObject.Find("Beam");
        BeamRigidBody = Beam.GetComponent<Rigidbody>();
        
        if (!ProgrammManagerScript.Recharging)
        {
            BeamRigidBody.AddForce(BeamRigidBody.transform.up * force, ForceMode.Impulse);
            ProgrammManagerScript.Recharging = true;
        }
    }
}
