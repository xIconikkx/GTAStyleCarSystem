using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Vehicle_Trigger : MonoBehaviour
{
    [Header("Vehicle System Reference")]
    public Vehicle_System vehSystem;

    private void Start()
    {
        if(vehSystem == null)
        {
            Debug.LogError("Vehicle System Should Be Referenced, Ill fix this though :)");
            vehSystem = GetComponentInParent<Vehicle_System>();
        }
    }

    private void OnTriggerEnter(Collider obj)
    {
        if(obj.gameObject.tag == "Player")
        {
            vehSystem.UpdateTrigger(true, obj.gameObject);
            Debug.Log("Enter Vehicle Door Trigger");
        }
    }

    private void OnTriggerExit(Collider obj)
    {
        if(obj.gameObject.tag == "Player")
        {
            vehSystem.UpdateTrigger(false, obj.gameObject);
            Debug.Log("Exited Vehicle Door Trigger");
        }
    }
}
