using UnityEngine;
using UnityEngine.UI;

public class ThrowDice : MonoBehaviour
{
    [SerializeField] private GameObject DiceObject;
    private Rigidbody DiceRigitBody;

    private GameObject fieldobject;
    private InputField field;
    private Rigidbody CollisionRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        fieldobject = GameObject.Find("InputField");
        field = fieldobject.GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        //DiceObject = Instantiate(ShellPrefab, transform.position + new Vector3(0, 0.25f, -0.05f), ShellPrefab.transform.rotation);
        DiceRigitBody = DiceObject.GetComponent<Rigidbody>();
       
        CollisionRigidBody = collision.rigidbody;
        CollisionRigidBody.AddForce(CollisionRigidBody.transform.up * (-1), ForceMode.Impulse);
    }
}
