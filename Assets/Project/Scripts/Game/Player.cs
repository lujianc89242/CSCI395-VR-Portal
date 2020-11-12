using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Portal")]
    public GameObject portalPrefab;
    public RenderTexture[] renderTextures;

    [Header("Grabbable objects")]
    public float grabbingDistance = 1f;
    public float throwingForce = 4f;

    public Action onCollectOrb;

    private GrabbableObject grabbableObject;
    private Camera playerCamera;
    private List<Portal> portals;
    private bool shouldUseFirstPortal = true;

    // Start is called before the first frame update
    void Start()
    {
        portals = new List<Portal>();
        playerCamera = transform.GetComponentInChildren<Camera>();
        
    }

    // Update is called once per frame
    void Update()
    {
        // Clicking logic for grabbing objects and shooting portals
        bool interactedWithObject = false;
        if (Input.GetMouseButtonDown(0))
        {
            // If holding grabbable object, release it.
            if (grabbableObject != null)
            {
                Release();
                interactedWithObject = true;
            }
            else
            {
                // Raycast for grabbable objects.
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, grabbingDistance))
                {
                    // Check if looking at grabbable object
                    if (hit.transform.GetComponent<GrabbableObject>() != null)
                    {
                        GrabbableObject targetObject = hit.transform.GetComponent<GrabbableObject>();
                        // Hold the object.
                        if (grabbableObject == null)
                        {
                            Hold(targetObject);
                            interactedWithObject = true;
                        }
                    }
                }
            }

            
        }

        // Logic for spawning portals.
        if(Input.GetMouseButtonDown(0) && interactedWithObject == false  )
        {
            // Perform the raycast.
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit))
            {
                Debug.Log(hit.transform.name);
                if(hit.transform.GetComponent<PortalArea>() != null)
                {
                    //Vector3 spawnPoint = hit.point;
                    SpawnPortal(hit.point, hit.normal, hit.transform.GetComponent<PortalArea>());
                }
            }
        }

        // Logic for holding the grabbable object.
        if (grabbableObject != null)
        {
            grabbableObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward * grabbingDistance;
        }
    }

    private void Hold (GrabbableObject targetObject)
    {
        grabbableObject = targetObject;
        grabbableObject.GetComponent<Collider>().enabled = false;
        grabbableObject.GetComponent<Rigidbody>().useGravity = false;
    }
    private void Release()
    {
        grabbableObject.GetComponent<Collider>().enabled = true;
        grabbableObject.GetComponent<Rigidbody>().useGravity = true;
        grabbableObject.GetComponent<Rigidbody>().AddForce(playerCamera.transform.forward * throwingForce);
        grabbableObject = null;
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if(otherCollider.GetComponent<Orb> () != null)
        {
            if(onCollectOrb != null)
            {
                onCollectOrb();
            }
        }
        if(otherCollider.GetComponent<Portal>() != null)
        {
            CharacterController cc = GetComponent<CharacterController>();

            //Debug.Log("Entered a portal!");
            Portal enterPortal = otherCollider.GetComponent<Portal> ();
			Portal exitPortal = (enterPortal == portals [0] ? portals [1] : portals [0]);

            cc.enabled = false;
			transform.position = exitPortal.transform.position + exitPortal.transform.forward;
            
            cc.enabled = true;
            //transform.position = Vector3.zero;
        }
    }

    private void SpawnPortal (Vector3 spawnPoint, Vector3 normal, PortalArea area)
    {
        Portal currentPortal;

        if(portals.Count < 2)
        {
            GameObject portalObject = Instantiate(portalPrefab);
            currentPortal = portalObject.GetComponent<Portal>();
            // Disable the camera so it doesn't render in the device's display.
            currentPortal.GetComponentInChildren<Camera>().enabled = false;
            portals.Add (currentPortal);

            // If added both portals, activate the cameras, set their
			// target textures as the project's render textures and,
			// finally, set the textures of the portals' materials.
            if(portals.Count == 2){
                portals[0].GetComponentInChildren<Camera> ().enabled = true;
				portals[1].GetComponentInChildren<Camera> ().enabled = true;

				portals[0].GetComponentInChildren<Camera> ().targetTexture = renderTextures [0];
				portals[1].GetComponentInChildren<Camera> ().targetTexture = renderTextures [1];

				portals[0].GetComponent<Renderer>().material.SetTexture("_MainTex", renderTextures[1]);
				portals[1].GetComponent<Renderer>().material.SetTexture("_MainTex", renderTextures[0]);
            }
        }
        else
        {
            currentPortal = portals[shouldUseFirstPortal ? 0 : 1];
            shouldUseFirstPortal = !shouldUseFirstPortal;
        }

        currentPortal.transform.position = spawnPoint;
        currentPortal.transform.forward = normal;

    }
}
