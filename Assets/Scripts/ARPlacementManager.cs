using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARAnchorManager))]
public class ARPlacementManager : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField]
    private GameObject cubePrefab;

    [Header("Managers")]
    [SerializeField]
    private ARPlaneManager planeManager;

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI instructionText;

    private ARRaycastManager raycastManager;
    private ARAnchorManager anchorManager;

    private GameObject spawnedCube;

    private static readonly List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        anchorManager = GetComponent<ARAnchorManager>();
    }

    private void Update()
    {
        if (spawnedCube != null)
            return;

        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase != TouchPhase.Began)
            return;

        
        if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            ARPlane plane = hits[0].trackable as ARPlane;

            if (plane == null)
                return;

            ARAnchor anchor = anchorManager.AttachAnchor(plane, hitPose);

            Vector3 spawnPosition = hitPose.position + Vector3.up * 0.05f;

            if (anchor != null)
            {
                spawnedCube = Instantiate(
                    cubePrefab,
                    spawnPosition,
                    hitPose.rotation,
                    anchor.transform);
            }
            else
            {
                spawnedCube = Instantiate(
                    cubePrefab,
                    spawnPosition,
                    hitPose.rotation);
            }

           
            if (instructionText != null)
            {
                instructionText.gameObject.SetActive(false);
            }

            
            if (planeManager != null)
            {
                planeManager.enabled = false;

                foreach (ARPlane detectedPlane in planeManager.trackables)
                {
                    detectedPlane.gameObject.SetActive(false);
                }
            }
        }
    }
}