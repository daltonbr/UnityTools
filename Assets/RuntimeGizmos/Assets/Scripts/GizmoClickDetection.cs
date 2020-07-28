using UnityEngine;
using System;
using System.Collections.Generic;

/// Detect if the attached gameObject is being clicked upon by the user, applied
/// to an individual gizmo handle/element.
public class GizmoClickDetection : MonoBehaviour
{
    /// Have we already click on one handle?
    public static bool AlreadyClicked;

    /// Camera that has been set to the gizmo layer
    public Camera gizmoCamera;

    /// Layer upon which gizmos sit
    public LayerMask gizmoLayer;

    /// Targets to detect clicks upon, and highlight when a click is detected
    public GameObject[] targets;

    /// Highlight material
    public Material highlight;

    /// Dictionary of previous materials before a highlight
    public Dictionary<MeshRenderer, Material> previousMaterials;

    /// Is the user currently pressing
    [HideInInspector]
    public bool pressing = false;

    /// Is the user pressing the plane area?
    [HideInInspector]
    public bool pressingPlane = false;
    
    public void Awake()
    {
        previousMaterials = new Dictionary<MeshRenderer, Material>();
    }
    
    /// Checks for hits on the target objects, highlighting when found
    public void Update ()
    {
        // If the left mouse button is pressed      
        if (!AlreadyClicked && Input.GetMouseButtonDown(0))
        {
            // Detect the object(s) the user has clicked
            Ray ray = gizmoCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, gizmoLayer);
            bool detected = false;
            pressingPlane = false;

            // Check if object are our targets (skipping the collision if the renderer isn't enabled)
            foreach (RaycastHit hit in hits)
            {
                if (Array.IndexOf(targets, hit.collider.gameObject) >= 0)
                {
                    if (!hit.collider.gameObject.GetComponent<Renderer>().enabled) continue;
                    if (hit.collider.gameObject.name.Contains("_plane_")) pressingPlane = true;
                    detected = true;
                    pressing = true;
                }
            }

            if (detected)
            {
                // Store the current materials of the targets, then highlight them
                previousMaterials?.Clear();

                foreach (GameObject target in targets)
                {
                    try
                    {
                        foreach (MeshRenderer renderer in target.GetComponentsInChildren<MeshRenderer>(false)) {
                            previousMaterials[renderer] = renderer.sharedMaterial;
                            renderer.material = highlight;
                        }
                    }
                    catch (NullReferenceException exception)
                    {
                        // Perhaps no previous materials could be found?
                    }
                }
                AlreadyClicked = true;
            }
        }
        else if (Input.GetMouseButtonUp(0) && previousMaterials.Count > 0)
        {
            // If the left mouse button was released and we haven't un-highlighted yet
            foreach (MeshRenderer renderer in previousMaterials.Keys)
            {
                renderer.material = previousMaterials[renderer];
            }
            previousMaterials.Clear();
            pressing = false;
            pressingPlane = false;
            AlreadyClicked = false;
        }
	}

}
