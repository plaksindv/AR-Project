using UnityEngine;
using UnityEngine.UI;

public class ThrowDice : MonoBehaviour
{
    [SerializeField] private GameObject DiceObject;
    private Rigidbody CollisionRigidBody;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {       
        CollisionRigidBody = collision.rigidbody;
        CollisionRigidBody.AddForce(CollisionRigidBody.transform.up * (-1), ForceMode.Impulse);
    }
}
