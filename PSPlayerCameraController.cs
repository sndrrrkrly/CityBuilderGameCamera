using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace Project.Star {
    public class PSPlayerCameraController : MonoBehaviour {
        /* Variables */

        public float normalMovementSpeed;
        public float fastMovementSpeed;

        public float movementSpeed;
        public float movementTime;

        public float rotationAmount;

        /* References */

        public static PSPlayerCameraController instance;

        public Transform followTransform;
        public Transform cameraTransform;

        public Vector3 cameraNewPosition;
        public Quaternion cameraNewRotation;

        public Vector3 cameraNewZoom;
        public Vector3 cameraZoomAmount;

        public Vector3 dragStartPosition;
        public Vector3 dragCurrentPosition;

        public Vector3 rotateStartPosition;
        public Vector3 rotateCurrentPosition;

        #region Start

            private void Start () {
                instance = this;

                cameraNewPosition = transform.position;
                cameraNewRotation = transform.rotation;

                cameraNewZoom = cameraTransform.localPosition;
            }

        #endregion

        #region Update

            private void Update () {
                if (followTransform != null) {
                    transform.position = followTransform.position;
                } else {
                    CalculateMovementInput();
                    CalculateMouseInput();
                }

                if (Input.GetKeyDown(KeyCode.Escape)) {
                    followTransform = null;
                }
            }

        #endregion

        #region Calculate
        
            private void CalculateMouseInput () {
                /* Movement (Mouse) */

                if (Input.mouseScrollDelta.y != 0) {
                    cameraNewZoom += Input.mouseScrollDelta.y * cameraZoomAmount;
                } 

                if (Input.GetMouseButtonDown(0)) {
                    Plane plane = new Plane (Vector3.up, Vector3.zero);
                    Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

                    float entry;

                    if (plane.Raycast(ray, out entry)) {
                        dragStartPosition = ray.GetPoint(entry);
                    }
                }

                if (Input.GetMouseButton(0)) {
                    Plane plane = new Plane (Vector3.up, Vector3.zero);
                    Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

                    float entry;

                    if (plane.Raycast(ray, out entry)) {
                        dragCurrentPosition = ray.GetPoint(entry);
                        cameraNewPosition = transform.position + dragStartPosition - dragCurrentPosition;
                    }
                }

                if (Input.GetMouseButtonDown(2)) {
                    rotateStartPosition = Input.mousePosition;
                }

                if (Input.GetMouseButton(2)) {
                    rotateCurrentPosition = Input.mousePosition;

                    Vector3 difference = rotateStartPosition - rotateCurrentPosition;
                    rotateStartPosition = rotateCurrentPosition;
                
                    cameraNewRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
                }
            }

            private void CalculateMovementInput () {
                #region Calculate cameraMovementSpeed

                    if (Input.GetKey(KeyCode.LeftShift)) {
                        movementSpeed = fastMovementSpeed;
                    } else {
                        movementSpeed = normalMovementSpeed;
                    }

                #endregion

                #region Calculate cameraNewPosition

                    /* Movement (W A S D) */

                    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                        cameraNewPosition += (transform.forward * movementSpeed);
                    }

                    if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                        cameraNewPosition += (transform.forward * -movementSpeed);
                    }

                    if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                        cameraNewPosition += (transform.right * movementSpeed);
                    }

                    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                        cameraNewPosition += (transform.right * -movementSpeed);
                    }

                    /* Rotation */

                    if (Input.GetKey(KeyCode.Q)) {
                        cameraNewRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
                    }

                    if (Input.GetKey(KeyCode.E)) {
                        cameraNewRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
                    }

                    /* Zoom In and Zoom Out */

                    if (Input.GetKey(KeyCode.R)) {
                        cameraNewZoom += cameraZoomAmount;
                    }

                    if (Input.GetKey(KeyCode.F)) {
                        cameraNewZoom -= cameraZoomAmount;
                    }

                #endregion

                transform.position = Vector3.Lerp (transform.position, cameraNewPosition, Time.deltaTime * movementTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, cameraNewRotation, Time.deltaTime * movementTime);

                cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, cameraNewZoom, Time.deltaTime * movementTime);
            }

        #endregion
    }
}