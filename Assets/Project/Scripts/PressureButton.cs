using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureButton : MonoBehaviour
{
    public TriggableObject targetObject;
    public GameObject model;
    public float pressureHeight;
    public float pressedDuration = 1f;
    public float pressedSpeed = 3f;

    private Vector3 targetPosition;
    private float pressedTimer;
    private bool pressed;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // Hold the button down for few seconds
        pressedTimer -= Time.deltaTime;
        if(pressedTimer > 0)
        {
            targetPosition = new Vector3(0, pressureHeight, 0);
            if(pressed == false)
            {
                pressed = true;
                OnPress();
            }
        }
        else
        {
            targetPosition = Vector3.zero;
            if (pressed == true)
            {
                pressed = false;
                OnUnpress();
            }
        }
        model.transform.localPosition = Vector3.Lerp(model.transform.localPosition, targetPosition, Time.deltaTime * pressedSpeed);
    }

    void OnTriggerStay(Collider otherCollider)
    {
        // Only the player and grabbable can press the button
        if(otherCollider.GetComponent<Player>() != null || otherCollider.GetComponent<GrabbableObject>() != null)
        {
            pressedTimer = pressedDuration;
        }
    }

    private void OnPress()
    {
        model.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.green);
        targetObject.OnTrigger();
    }
    private void OnUnpress()
    {
        model.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.red);
        targetObject.OnUntrigger();
    }
}
