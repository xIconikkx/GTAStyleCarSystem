using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Vehicle_System : MonoBehaviour
{
    [Space(5)]
    [Header("Vehicle Settings")]
    public List<MonoBehaviour> vehicleScripts = new List<MonoBehaviour>();
    public Transform exitPoint;
    public GameObject vehicleCamera;
    public CarSpeedValue SpeedValue = CarSpeedValue.MPH;

    [Space(5)]
    [Header("Vehicle Controls")]
    public KeyCode enterexitKey = KeyCode.E;
    public KeyCode hornKey = KeyCode.H;

    [Space(5)]
    [Header("Vehicle Headlights")]
    public List<GameObject> headlights = new List<GameObject>();
    public KeyCode headlightKey = KeyCode.L;

    [Space(5)]
    [Header("Indicator Settings")]
    public GameObject indicatorLeft;
    public GameObject indicatorRight;
    public KeyCode indicatorLeftKey = KeyCode.Alpha1;
    public KeyCode indicatorRightKey = KeyCode.Alpha2;
    public float indicatorFlashRate = 0.5f;

    [Space(5)]
    [Header("Vehicle UI")]
    public GameObject vehicleCanvas;
    public Text speedometer;    

    [Space(5)]
    [Header("Vehicle Sounds")]
    public AudioClip hornSound;

    //private stuff
    private bool inDoorTrigger;
    private bool insideVehicle;
    private bool leftIndicatorOn;
    private bool rightIndicatorOn;
    private bool headlightsOn;
    private float Timer;
    private Rigidbody carRg;
    private AudioSource aSource;

    private GameObject localPlayer;

    void Start()
    {
        if(vehicleScripts.Count <= 0)
        {
            Debug.LogError("Vehicle Scripts Should Be Attached so they can be disabled/enabled");
        }
        
        if(vehicleCamera == null)
        {
            Debug.LogError("Vehicle Camera should be attached so it can be disabled/enabled");
        }

        if(headlights.Count <= 0)
        {
            Debug.LogError("There are no headlights attached, so you cannot enable/disable them");
        }

        if(vehicleCanvas == null || speedometer == null)
        {
            Debug.LogError("Please make sure UI is attached to the script");
        }


        aSource = GetComponentInParent<AudioSource>();
        carRg = GetComponentInParent<Rigidbody>();

        indicatorLeft.SetActive(false);
        indicatorRight.SetActive(false);
        vehicleCamera.SetActive(false);
        vehicleCanvas.SetActive(false);
    }


    void Update()
    {
        if (Input.GetKeyDown(enterexitKey))
        {
            if (inDoorTrigger)
            {
                insideVehicle = true;
                VehicleControls(insideVehicle);
                inDoorTrigger = false;

                localPlayer.SetActive(false);

            }
            else if(insideVehicle)
            {
                insideVehicle = false;
                VehicleControls(insideVehicle);
                inDoorTrigger = false;

                localPlayer.transform.position = exitPoint.position;
                localPlayer.SetActive(true);
            }
        }

        if (Input.GetKeyDown(headlightKey) && insideVehicle)
        {
            foreach (GameObject light in headlights)
            {
                light.SetActive(!light.activeSelf);
            }
        }

        if (Input.GetKeyDown(indicatorLeftKey) && insideVehicle)
        {
            leftIndicatorOn = !leftIndicatorOn;
            rightIndicatorOn = false;
            indicatorLeft.SetActive(leftIndicatorOn);
            indicatorRight.SetActive(false);
        }
        else if (Input.GetKeyDown(indicatorRightKey) && insideVehicle)
        {
            rightIndicatorOn = !rightIndicatorOn;
            leftIndicatorOn = false;
            indicatorLeft.SetActive(false);
            indicatorRight.SetActive(rightIndicatorOn);
        }

        if (Input.GetKeyDown(hornKey) && !aSource.isPlaying && insideVehicle)
        {
            aSource.PlayOneShot(hornSound);
        }


        if(leftIndicatorOn || rightIndicatorOn)
        {
            Timer += Time.deltaTime;

            if(Timer >= indicatorFlashRate)
            {
                if (leftIndicatorOn)
                {
                    indicatorLeft.SetActive(!indicatorLeft.activeSelf);
                    Timer = 0;
                }
                else if (rightIndicatorOn)
                {
                    indicatorRight.SetActive(!indicatorRight.activeSelf);
                    Timer = 0;
                }
            }
        }

        if(speedometer != null)
        {
            if(SpeedValue == CarSpeedValue.MPH)
            {
                var mph = carRg.velocity.magnitude * 2.237;
                speedometer.text = "Speed: " + Mathf.RoundToInt((float)mph) + "MPH";
            }
            else if(SpeedValue == CarSpeedValue.KPH)
            {
                var kph = carRg.velocity.magnitude * 3.6;
                speedometer.text = "Speed: " + Mathf.RoundToInt((float)kph) + "KPH";
            }
            
        }
    }

    public void VehicleControls(bool controls)
    {
        foreach (MonoBehaviour script in vehicleScripts)
        {
            script.enabled = controls;
        }

        vehicleCamera.SetActive(controls);
        vehicleCanvas.SetActive(controls);
    }


    public void UpdateTrigger(bool triggerStatus, GameObject localP)
    {
        inDoorTrigger = triggerStatus;
        localPlayer = localP;
    }

    public enum CarSpeedValue
    {
        MPH,
        KPH
    }
}
