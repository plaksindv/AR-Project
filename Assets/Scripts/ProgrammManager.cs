using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ProgrammManager : MonoBehaviour
{
    private ARRaycastManager ARRaycastManagerScript;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    [Header("Put your planeMarker here")]
    [SerializeField] private GameObject PlaneMarkerPrefab;
    public GameObject ObjectToSpawn;
    [Header("Put ScrollView here")]
    public GameObject ScrollView;
    private GameObject SelectedObject;
    [SerializeField] GameObject MaketShell;
    [SerializeField] private GameObject EndText;

    [SerializeField] private Camera ARCamera;

    private Vector2 TouchPosition;

    private Quaternion YRotation;

    public bool ChooseObject = false;
    public bool Rotation;
    public bool Recharging;

    public int Strikes;

    void Start()
    {
        ARRaycastManagerScript = FindObjectOfType<ARRaycastManager>();

        EndText.SetActive(false);

        PlaneMarkerPrefab.SetActive(false);
        ScrollView.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (ChooseObject)
        {
            ShowMarkerAndSetObject();
        }

        MoveObjectAndRotation();

        if (Strikes > 2)
        {
            EndText.SetActive(true);
        }


        if (Recharging)
        {
            MaketShell.SetActive(false);
        }
        else
        {
            MaketShell.SetActive(true);
        }

    }

    void ShowMarkerAndSetObject()
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        ARRaycastManagerScript.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);

        // show marker
        if (hits.Count > 0)
        {
            PlaneMarkerPrefab.transform.position = hits[0].pose.position;
            PlaneMarkerPrefab.SetActive(true);
        }
       // set object
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Instantiate(ObjectToSpawn, hits[0].pose.position, ObjectToSpawn.transform.rotation);
            MaketShell = GameObject.Find("MaketShell");
            ChooseObject = false;
            PlaneMarkerPrefab.SetActive(false);
        }
    }

    void MoveObjectAndRotation()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            TouchPosition = touch.position;
            
            // Select object
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = ARCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;

                if (Physics.Raycast(ray, out hitObject))
                {
                    if (hitObject.collider.CompareTag("UnSelected"))
                    {
                        hitObject.collider.gameObject.tag = "Selected";
                    }
                }
            }

            SelectedObject = GameObject.FindWithTag("Selected");

            if (touch.phase == TouchPhase.Moved && Input.touchCount == 1 )
            {
               // Rotate object with one finger
                if (Rotation)
                {
                    YRotation = Quaternion.Euler(0f, -touch.deltaPosition.x * 0.1f, 0f);
                    SelectedObject.transform.rotation = YRotation * SelectedObject.transform.rotation;
                }
               // Move Object
                else
                {
                    ARRaycastManagerScript.Raycast(TouchPosition, hits, TrackableType.Planes);
                    SelectedObject.transform.position = hits[0].pose.position;
                }
            }
            // Rotate object with 2 fingers
            if (Input.touchCount == 2)
            {
                Touch touch1 = Input.touches[0];
                Touch touch2 = Input.touches[1];

                if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
                {
                    float DistanceBetweenTouches = Vector2.Distance(touch1.position, touch2.position);
                    float prevDistanceBetweenTouches = Vector2.Distance(touch1.position - touch1.deltaPosition, touch2.position - touch2.deltaPosition);
                    float Delta = DistanceBetweenTouches - prevDistanceBetweenTouches;

                    if (Mathf.Abs(Delta) > 0)
                    {
                        Delta *= 0.1f;
                    }
                    else
                    {
                        DistanceBetweenTouches = Delta = 0;
                    }
                    YRotation = Quaternion.Euler(0f, -touch1.deltaPosition.x * Delta, 0f);
                    SelectedObject.transform.rotation = YRotation * SelectedObject.transform.rotation;
                }

            }
            // Deselect object
            if (touch.phase == TouchPhase.Ended)
            {
                if (SelectedObject.CompareTag("Selected"))
                {
                    SelectedObject.tag = "UnSelected";
                }
            }
        }
    }
}
