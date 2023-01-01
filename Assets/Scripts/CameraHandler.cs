using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private float moveSpeed = 30f;
    [SerializeField] private float zoomSpeed = 4f;
    [SerializeField] private float minZoom = 10f;
    [SerializeField] private float maxZoom = 30f;

    private float orthographicSize;
    private float targetOrthographicSize;


    private void Start()
    {
        orthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        targetOrthographicSize = orthographicSize;
    }

    private void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        var moveDirection = new Vector3(x, y).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private void HandleZoom()
    {
        targetOrthographicSize += -Input.mouseScrollDelta.y * zoomSpeed;
        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, minZoom, maxZoom);

        // smooth the zoom
        orthographicSize = Mathf.Lerp(orthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);

        cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographicSize;
    }
}
