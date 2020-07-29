using UnityEngine;

namespace RuntimeGizmos
{
    /// Handle Scale Gizmo at Runtime
    public class GizmoScale : MonoBehaviour
    {
        /// Scale speed scalar
        public float scaleSpeed = 7.5f;

        /// X handle of gizmo
        public GameObject xHandle;

        /// Y handle of gizmo
        public GameObject yHandle;

        /// Z handle of gizmo
        public GameObject zHandle;

        /// Components of each scaling handle
        public GameObject xCube, xCylinder, yCube, yCylinder, zCube, zCylinder;

        /// Center handle of gizmo
        public GameObject centerHandle;

        /// Target for scaling
        public GameObject scaleTarget;

        /// Array of detector scripts stored as [x, y, z, center]
        private GizmoClickDetection[] _detectors;

        /// Initial scale and position of gizmos
        private Vector3 _initialScaleCenterHandle;
        private Vector3 _initialPositionXCube;
        private Vector3 _initialPositionYCube;
        private Vector3 _initialPositionZCube;
        private Vector3 _initialPositionXCylinder;
        private Vector3 _initialPositionYCylinder;
        private Vector3 _initialPositionZCylinder;
        private Vector3 _initialScaleXCylinder;
        private Vector3 _initialScaleYCylinder;
        private Vector3 _initialScaleZCylinder;
        
        public void Awake()
        {
            // Get the initial state of gizmos
            _initialScaleCenterHandle = gameObject.transform.localScale; 
            _initialPositionXCube = xCube.transform.localPosition;
            _initialPositionYCube = yCube.transform.localPosition;
            _initialPositionZCube = zCube.transform.localPosition;
            _initialPositionXCylinder = xCylinder.transform.localPosition;
            _initialPositionYCylinder = yCylinder.transform.localPosition;
            _initialPositionZCylinder = zCylinder.transform.localPosition;
            _initialScaleXCylinder = xCylinder.transform.localScale;
            _initialScaleYCylinder = yCylinder.transform.localScale;
            _initialScaleZCylinder = zCylinder.transform.localScale;

            // Get the click detection scripts
            _detectors = new GizmoClickDetection[4];
            _detectors[0] = xHandle.GetComponent<GizmoClickDetection>();
            _detectors[1] = yHandle.GetComponent<GizmoClickDetection>();
            _detectors[2] = zHandle.GetComponent<GizmoClickDetection>();
            _detectors[3] = centerHandle.GetComponent<GizmoClickDetection>();

            // Set the same position for the target and the gizmo
            transform.position = scaleTarget.transform.position;
        }

        public void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                RescaleGizmoBack();
            }

            for (int i = 0; i < 4; i++)
            {
                if (Input.GetMouseButton(0) && _detectors[i].pressing)
                {
                    switch (i)
                    {
                        // X Axis
                        case 0:
                        {
                            // Scale along the X axis
                            float delta = Input.GetAxis("Mouse X") * (Time.deltaTime);
                            delta *= scaleSpeed;

                            if ((scaleTarget.transform.localScale.x - delta) <= 0.01f) return;
                            scaleTarget.transform.localScale += new Vector3(-delta, 0.0f, 0.0f);

                            // Scale the handle's cylinder then move the cube to its new end
                            float lengthBefore = xCylinder.GetComponent<Renderer>().bounds.size.x;

                            xCylinder.transform.localScale += new Vector3(0.0f, 0.0f, -delta);
                            xCylinder.GetComponent<MeshFilter>().mesh.RecalculateBounds();
                            float lengthAfter = xCylinder.GetComponent<Renderer>().bounds.size.x;

                            xCube.transform.position += new Vector3(lengthAfter - lengthBefore, 0.0f, 0.0f);

                            xCylinder.transform.position = new Vector3(
                                lengthAfter / 2.0f,
                                xCylinder.transform.position.y,
                                xCylinder.transform.position.z
                            );
                        }
                            break;

                        // Y Axis
                        case 1:
                        {
                            // Scale along the Y axis
                            float delta = Input.GetAxis("Mouse Y") * (Time.deltaTime);
                            delta *= scaleSpeed;

                            if ((scaleTarget.transform.localScale.y + delta) <= 0.01f) return;
                            scaleTarget.transform.localScale += new Vector3(0.0f, delta, 0.0f);

                            // Scale the handle's cylinder then move the cube to its new end
                            float lengthBefore =yCylinder.GetComponent<Renderer>().bounds.size.y;

                            yCylinder.transform.localScale += new Vector3(0.0f, 0.0f, delta);
                            yCylinder.GetComponent<MeshFilter>().mesh.RecalculateBounds();
                            float lengthAfter = yCylinder.GetComponent<Renderer>().bounds.size.y;

                            yCube.transform.position += new Vector3(0.0f, lengthAfter - lengthBefore, 0.0f);

                            yCylinder.transform.position = new Vector3(
                                yCylinder.transform.position.x,
                                lengthAfter / 2.0f,
                                yCylinder.transform.position.z
                            );
                        }
                            break;

                        // Z Axis
                        case 2:
                        {
                            // Scale along the Z axis
                            float delta = Input.GetAxis("Mouse X") * (Time.deltaTime);
                            delta *= scaleSpeed;

                            if ((scaleTarget.transform.localScale.z + delta) <= 0.01f) return;
                            scaleTarget.transform.localScale += new Vector3(0.0f, 0.0f, delta);

                            // Scale the handle's cylinder then move the cube to its new end
                            float lengthBefore = zCylinder.GetComponent<Renderer>().bounds.size.z;

                            zCylinder.transform.localScale += new Vector3(0.0f, 0.0f, delta);
                            zCylinder.GetComponent<MeshFilter>().mesh.RecalculateBounds();
                            float lengthAfter = zCylinder.GetComponent<Renderer>().bounds.size.z;

                            zCube.transform.position += new Vector3(0.0f, 0.0f, lengthAfter - lengthBefore);

                            zCylinder.transform.position = new Vector3(
                                zCylinder.transform.position.x,
                                zCylinder.transform.position.y,
                                lengthAfter / 2.0f
                            );
                        }
                            break;

                        // Center (uniform scale)
                        case 3:
                        {
                            float delta = (Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")) * (Time.deltaTime);
                            delta *= scaleSpeed;

                            if ((gameObject.transform.localScale.x + delta) <= (_initialScaleCenterHandle.x / 25.0f)) return;
                            if ((gameObject.transform.localScale.y + delta) <= (_initialScaleCenterHandle.y / 25.0f)) return;
                            if ((gameObject.transform.localScale.z + delta) <= (_initialScaleCenterHandle.z / 25.0f)) return;

                            scaleTarget.transform.localScale += new Vector3(delta, delta, delta);
                            gameObject.transform.localScale += new Vector3(delta, delta, delta);
                        }
                            break;
                    }

                    break;
                }
            }
        }

        private void RescaleGizmoBack()
        {
            gameObject.transform.localScale = _initialScaleCenterHandle;
            xCube.transform.localPosition = _initialPositionXCube;
            yCube.transform.localPosition = _initialPositionYCube;
            zCube.transform.localPosition = _initialPositionZCube;
            xCylinder.transform.localPosition = _initialPositionXCylinder;
            yCylinder.transform.localPosition = _initialPositionYCylinder;
            zCylinder.transform.localPosition = _initialPositionZCylinder;
            xCylinder.transform.localScale = _initialScaleXCylinder;
            yCylinder.transform.localScale = _initialScaleYCylinder;
            zCylinder.transform.localScale = _initialScaleZCylinder;
        }

    }
}
