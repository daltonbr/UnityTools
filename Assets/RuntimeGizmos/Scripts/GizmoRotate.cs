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
        private GizmoClickDetection[] _detectors;

        private Quaternion _initialRotation;

        public void Awake()
        {
            // Get the click detection scripts
            _detectors = new GizmoClickDetection[3];
            _detectors[0] = xTorus.GetComponent<GizmoClickDetection>();
            _detectors[1] = yTorus.GetComponent<GizmoClickDetection>();
            _detectors[2] = zTorus.GetComponent<GizmoClickDetection>();

            // Set the same position for the target and the gizmo
            transform.position = rotateTarget.transform.position;

            _initialRotation = gameObject.transform.rotation;
        }

        public void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                gameObject.transform.rotation = _initialRotation;
            }
            
            if (!Input.GetMouseButton(0)) return;
            
            for (int i = 0; i < 3; i++)
            {
                if (_detectors[i].pressing)
                {
                    // Rotation angle
                    float delta = (Input.GetAxis("Mouse X") - Input.GetAxis("Mouse Y")) * (Time.deltaTime);
                    delta *= rotationSpeed;

                    switch (i)
                    {
                        // X Axis
                        case 0:
                            rotateTarget.transform.Rotate(Vector3.right, delta, Space.World);
                            gameObject.transform.Rotate(Vector3.right, delta);
                            break;

                        // Y Axis
                        case 1:
                            rotateTarget.transform.Rotate(Vector3.down, delta, Space.World);
                            gameObject.transform.Rotate(Vector3.down, delta);
                            break;

                        // Z Axis
                        case 2:
                            rotateTarget.transform.Rotate(Vector3.back, delta, Space.World);
                            gameObject.transform.Rotate(Vector3.back, delta);
                            break;
                    }

                    break;
                }
            }
        }

    }
}
