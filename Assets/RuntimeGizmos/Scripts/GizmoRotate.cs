using UnityEngine;

namespace RuntimeGizmos
{
    /// Handle Rotate Gizmo at Runtime
    public class GizmoRotate : MonoBehaviour
    {
        /// Rotation speed scalar
        public float rotationSpeed = 75.0f;

        /// X torus of gizmo
        public GameObject xTorus;

        /// Y torus of gizmo
        public GameObject yTorus;

        /// Z torus of gizmo
        public GameObject zTorus;

        /// Target for rotation
        public GameObject rotateTarget;

        /// Array of detector scripts stored as [x, y, z]
        private GizmoClickDetection[] detectors;

        public void Awake()
        {
            // Get the click detection scripts
            detectors = new GizmoClickDetection[3];
            detectors[0] = xTorus.GetComponent<GizmoClickDetection>();
            detectors[1] = yTorus.GetComponent<GizmoClickDetection>();
            detectors[2] = zTorus.GetComponent<GizmoClickDetection>();

            // Set the same position for the target and the gizmo
            transform.position = rotateTarget.transform.position;
        }

        public void Update()
        {
            for (int i = 0; i < 3; i++)
            {
                if (Input.GetMouseButton(0) && detectors[i].pressing)
                {
                    // Rotation angle
                    float delta = (Input.GetAxis("Mouse X") - Input.GetAxis("Mouse Y")) * (Time.deltaTime);
                    delta *= rotationSpeed;

                    switch (i)
                    {
                        // X Axis
                        case 0:
                            rotateTarget.transform.Rotate(Vector3.right, delta);
                            gameObject.transform.Rotate(Vector3.right, delta);
                            break;

                        // Y Axis
                        case 1:
                            rotateTarget.transform.Rotate(Vector3.up, delta);
                            gameObject.transform.Rotate(Vector3.up, delta);
                            break;

                        // Z Axis
                        case 2:
                            rotateTarget.transform.Rotate(Vector3.forward, delta);
                            gameObject.transform.Rotate(Vector3.forward, delta);
                            break;
                    }

                    break;
                }
            }
        }

    }
}
