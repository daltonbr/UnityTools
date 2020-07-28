using UnityEngine;
using UnityEngine.UI;

namespace RuntimeGizmos
{
    /// Handle View Gizmo (i.e. rotates the camera to a
    /// predefined position around a specified pivot; front, back, left etc.)
    public class GizmoView : MonoBehaviour
    {
        /// Container for all cameras to rotate around targetObject
        public GameObject cameraContainer;

        /// X handles of gizmo (left and right)
        public GameObject xHandle1, xHandle2;

        /// Y handles of gizmo (top and bottom)
        public GameObject yHandle1, yHandle2;

        /// Z handles of gizmo (front and back)
        public GameObject zHandle1, zHandle2;

        /// Center cube of gizmo (free rotate)
        public GameObject centerCube;

        /// Target object to rotate around (pivot)
        public GameObject targetObject;

        // Axis labels (i.e. "X", "Y", or "Z")
        public Text topLabel, sideLabel;

        // Array of detector scripts stored as [x1, x2, y1, y2, z1, z2, center]
        private GizmoClickDetection[] detectors;

        /// Time of last click (since startup, in seconds)
        private float lastClick;

        public void Awake()
        {
            lastClick = Time.realtimeSinceStartup;

            // Get the click detection scripts
            detectors = new GizmoClickDetection[7];
            detectors[0] =  xHandle1.GetComponent<GizmoClickDetection>();
            detectors[1] =  xHandle2.GetComponent<GizmoClickDetection>();
            detectors[2] =  yHandle1.GetComponent<GizmoClickDetection>();
            detectors[3] =  yHandle2.GetComponent<GizmoClickDetection>();
            detectors[4] =  zHandle1.GetComponent<GizmoClickDetection>();
            detectors[5] =  zHandle2.GetComponent<GizmoClickDetection>();
            detectors[6] = centerCube.GetComponent<GizmoClickDetection>();

            // Hide the handles currently facing front & back
            zHandle1.GetComponent<Renderer>().enabled = false;
            zHandle2.GetComponent<Renderer>().enabled = false;
        }

        public void Update() 
        {
            // If the user isn't clicking the center cube (for free rotate), and they've clicked recently
            // (within half a second), bug out.
            if (!detectors[6].pressing && (Time.realtimeSinceStartup - lastClick) < 0.5f) return;

            // Find the handle that the user has clicked then move the camera the corresponding positions
            for (int i = 0; i < detectors.Length; i++)
            {
                if (Input.GetMouseButton(0) && detectors[i].pressing)
                {
                    // Get the distance from the cameraContainer to the pivot (targetObject) so that we can
                    // maintain it after moving to another side of the pivot.
                    float distance = Vector3.Distance(targetObject.transform.position, cameraContainer.transform.position);
                    switch(i)
                    {
                        // Left most x handle, move to left view
                        case 0:

                            // Move the cameras to the left of the pivot then rotate the gizmo to match
                            cameraContainer.transform.position = new Vector3(
                                targetObject.transform.position.x - distance,
                                targetObject.transform.position.y,
                                targetObject.transform.position.z
                            );

                            gameObject.transform.localRotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);

                            // Update UI labels and hide handles now facing front and back (x handles)
                            sideLabel.text = "z";
                            topLabel.text = "y";
                            xHandle1.GetComponent<Renderer>().enabled = false;
                            xHandle2.GetComponent<Renderer>().enabled = false;
                            yHandle1.GetComponent<Renderer>().enabled = true;
                            yHandle2.GetComponent<Renderer>().enabled = true;
                            zHandle1.GetComponent<Renderer>().enabled = true;
                            zHandle2.GetComponent<Renderer>().enabled = true;
                            break;

                        // Right most x handle, move to right view
                        case 1:

                            // Move the cameras to the right of the pivot then rotate the gizmo to match
                            cameraContainer.transform.position = new Vector3(
                                targetObject.transform.position.x + distance,
                                targetObject.transform.position.y,
                                targetObject.transform.position.z
                            );

                            gameObject.transform.localRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);

                            // Update UI labels and hide handles now facing front and back (x handles)
                            sideLabel.text = "z";
                            topLabel.text = "y";
                            xHandle1.GetComponent<Renderer>().enabled = false;
                            xHandle2.GetComponent<Renderer>().enabled = false;
                            yHandle1.GetComponent<Renderer>().enabled = true;
                            yHandle2.GetComponent<Renderer>().enabled = true;
                            zHandle1.GetComponent<Renderer>().enabled = true;
                            zHandle2.GetComponent<Renderer>().enabled = true;
                            break;

                        // Top most y handle, move to top view
                        case 2:

                            // Move the cameras to the top of the pivot then rotate the gizmo to match
                            cameraContainer.transform.position = new Vector3(
                                targetObject.transform.position.x,
                                targetObject.transform.position.y + distance,
                                targetObject.transform.position.z
                            );

                            gameObject.transform.localRotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);

                            // Update UI labels and hide handles now facing front and back (y handles)
                            sideLabel.text = "x";
                            topLabel.text = "z";
                            xHandle1.GetComponent<Renderer>().enabled = true;
                            xHandle2.GetComponent<Renderer>().enabled = true;
                            yHandle1.GetComponent<Renderer>().enabled = false;
                            yHandle2.GetComponent<Renderer>().enabled = false;
                            zHandle1.GetComponent<Renderer>().enabled = true;
                            zHandle2.GetComponent<Renderer>().enabled = true;
                            break;

                        // Bottom most y handle, move to bottom view
                        case 3:

                            // Move the cameras to the bottom of the pivot then rotate the gizmo to match
                            cameraContainer.transform.position = new Vector3(
                                targetObject.transform.position.x,
                                targetObject.transform.position.y - distance,
                                targetObject.transform.position.z
                            );

                            gameObject.transform.localRotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);

                            // Update UI labels and hide handles now facing front and back (y handles)
                            sideLabel.text = "x";
                            topLabel.text = "z";
                            xHandle1.GetComponent<Renderer>().enabled = true;
                            xHandle2.GetComponent<Renderer>().enabled = true;
                            yHandle1.GetComponent<Renderer>().enabled = false;
                            yHandle2.GetComponent<Renderer>().enabled = false;
                            zHandle1.GetComponent<Renderer>().enabled = true;
                            zHandle2.GetComponent<Renderer>().enabled = true;
                            break;

                        // Forward most z handle, move to forward view
                        case 4:

                            // Move the cameras to the front of the pivot then rotate the gizmo to match
                            cameraContainer.transform.position = new Vector3(
                                targetObject.transform.position.x,
                                targetObject.transform.position.y,
                                targetObject.transform.position.z - distance
                            );

                            gameObject.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

                            // Update UI labels and hide handles now facing front and back (z handles)
                            sideLabel.text = "x";
                            topLabel.text = "y";
                            xHandle1.GetComponent<Renderer>().enabled = true;
                            xHandle2.GetComponent<Renderer>().enabled = true;
                            yHandle1.GetComponent<Renderer>().enabled = true;
                            yHandle2.GetComponent<Renderer>().enabled = true;
                            zHandle1.GetComponent<Renderer>().enabled = false;
                            zHandle2.GetComponent<Renderer>().enabled = false;
                            break;

                        // Backward most z handle, move to backward view
                        case 5:

                            // Move the cameras to the back of the pivot then rotate the gizmo to match
                            cameraContainer.transform.position = new Vector3(
                                targetObject.transform.position.x,
                                targetObject.transform.position.y,
                                targetObject.transform.position.z + distance
                            );

                            gameObject.transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);

                            // Update UI labels and hide handles now facing front and back (z handles)
                            sideLabel.text = "x";
                            topLabel.text = "y";
                            xHandle1.GetComponent<Renderer>().enabled = true;
                            xHandle2.GetComponent<Renderer>().enabled = true;
                            yHandle1.GetComponent<Renderer>().enabled = true;
                            yHandle2.GetComponent<Renderer>().enabled = true;
                            zHandle1.GetComponent<Renderer>().enabled = false;
                            zHandle2.GetComponent<Renderer>().enabled = false;
                            break;

                        // Center cube, free rotate
                        case 6:

                            // Move the gizmo and the target as the user drags the mouse
                            sideLabel.text = "";
                            topLabel.text  = "";

                            float deltaX = (Input.GetAxis("Mouse X") * (Time.deltaTime)) * 75.0f;
                            float deltaY = (Input.GetAxis("Mouse Y") * (Time.deltaTime)) * 75.0f;

                            cameraContainer.transform.RotateAround(targetObject.transform.position, Vector3.up, deltaX);
                            cameraContainer.transform.RotateAround(targetObject.transform.position, Vector3.right, -deltaY);

                            gameObject.transform.RotateAround(gameObject.transform.position, Vector3.up, -deltaX);
                            gameObject.transform.RotateAround(gameObject.transform.position, Vector3.right, deltaY);

                            // Show all gizmo handles
                            xHandle1.GetComponent<Renderer>().enabled = true;
                            xHandle2.GetComponent<Renderer>().enabled = true;
                            yHandle1.GetComponent<Renderer>().enabled = true;
                            yHandle2.GetComponent<Renderer>().enabled = true;
                            zHandle1.GetComponent<Renderer>().enabled = true;
                            zHandle2.GetComponent<Renderer>().enabled = true;
                            break;
                    }

                    // Rotate the camera to look at the target, store the time of the click
                    cameraContainer.transform.LookAt(targetObject.transform);
                    lastClick = Time.realtimeSinceStartup;

                    break;
                }
            }
        }

    }
}
