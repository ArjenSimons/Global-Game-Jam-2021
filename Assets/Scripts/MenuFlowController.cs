using System;
using UnityEngine;
using UnityEngine.Events;

public class MenuFlowController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float bezierTime = 0.75f;

    [Header("Scene Reference")]
    [SerializeField]
    private Transform cameraTransform = null;

    [SerializeField]
    private Transform head = null;

    [SerializeField]
    private Transform menuTransform = null;

    [SerializeField]
    private Transform bezierTransform = null;

    [Header("Project References")]
    [SerializeField]
    private GameFlowSettings gameFlow = null;

    [Header("Events")]
    [SerializeField]
    private CameraAttachedToHeadEvent cameraAttachedToHead = null;

    private void Awake()
    {
        gameFlow.OnGameStart += OnGameStart;
        gameFlow.OnGameEnd += OnGameEnd;
    }

    private void OnGameEnd()
    {
        PlaceCameraAtComputerPosition();
    }

    private void Start()
    {
        if (gameFlow.StartAtComputer)
        {
            PlaceCameraAtComputerPosition();
        }
    }

    private void OnGameStart()
    {
        MoveCameraTowardsHead();
    }

    private void PlaceCameraAtComputerPosition()
    {
        cameraTransform.position = menuTransform.position;
        cameraAttachedToHead.Invoke(false);
    }

    private void MoveCameraTowardsHead()
    {
        LTBezierPath path = new LTBezierPath(new Vector3[] { menuTransform.position, bezierTransform.position, bezierTransform.position, head.position });

        LeanTween.move(cameraTransform.gameObject, path, bezierTime)
            .setEaseInOutSine()
            .setOnComplete(() => cameraAttachedToHead.Invoke(true));
    }

    [System.Serializable]
    public class CameraAttachedToHeadEvent : UnityEvent<bool> { }
}