﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Random = UnityEngine.Random;

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

    [SerializeField] private Camera ARCamera;

    private Vector2 TouchPosition;
    private GameObject fieldObject;
    private InputField field;

    public bool ChooseObject = false;

    void Start()
    {
        ARRaycastManagerScript = FindObjectOfType<ARRaycastManager>();

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

            fieldObject = GameObject.Find("InputField");
            field = fieldObject.GetComponent<InputField>();

            int dicesCount = 1;
			bool countFieldIsInteger = int.TryParse(field.text, out dicesCount);

            if (countFieldIsInteger && dicesCount > 0)
            {
                for (int i = 0; i < dicesCount; i++)
                {
                    Instantiate(ObjectToSpawn, new Vector3((float)(hits[0].pose.position.x + 0.1 * (i + 1)), hits[0].pose.position.y + 1, hits[0].pose.position.z), Random.rotation);
                }
            } 
            else
			{
                Instantiate(ObjectToSpawn, new Vector3(hits[0].pose.position.x, hits[0].pose.position.y + 1, hits[0].pose.position.z), Random.rotation);
            }

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

            // Move Object

            if (touch.phase == TouchPhase.Moved && Input.touchCount == 1 )
            {
                ARRaycastManagerScript.Raycast(TouchPosition, hits, TrackableType.Planes);
                SelectedObject.transform.position = hits[0].pose.position;
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
