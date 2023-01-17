using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    [SerializeField] GunSetup zoomCamera;
    public Camera PovCam;
    public float FieldOfView;
    private float baseFOV;

    // Update is called once per frame
    private void Start()
    {

        PovCam = Camera.main;
        baseFOV = PovCam.fieldOfView;
    }
    void Update()
    {
        if (!zoomCamera)
            return;
        GetFOV();
        if (Input.GetButton("Zoom"))
        {
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, FieldOfView, Time.deltaTime * 5);
        }
        else
        {
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, baseFOV, Time.deltaTime * 5);
        }
    }
    void GetFOV()
    {
        FieldOfView = GameManager.instance.playerScript.FieldOfView;
    }

}
