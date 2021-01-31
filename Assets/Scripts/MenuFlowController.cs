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

    [SerializeField]
    private PlayerController moveController = null;

    [Header("Project References")]
    [SerializeField]
    private GameFlowSettings gameFlow = null;

    private void Awake()
    {
        gameFlow.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameStateChange gameStateChange)
    {
        switch (gameStateChange)
        {
            case GameStateChange.TutorialStarted:
                OnTutorialStart();
                break;

            case GameStateChange.GameStarted:
                OnGameStart();
                break;

            case GameStateChange.OnGameEnd:
                OnGameEnd();
                break;

            default:
                break;
        }
    }

    private void OnGameStart()
    {
        if (!moveController.HasCameraAttachedToHead)
        {
            MoveCameraTowardsHead();
        }
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

    private void OnTutorialStart()
    {
        MoveCameraTowardsHead();
    }

    private void PlaceCameraAtComputerPosition()
    {
        cameraTransform.position = menuTransform.position;
        moveController.SetCameraAttachedToHead(false);
    }

    private void MoveCameraTowardsHead()
    {
        LTBezierPath path = new LTBezierPath(new Vector3[] { menuTransform.position, bezierTransform.position, bezierTransform.position, head.position });

        LeanTween.move(cameraTransform.gameObject, path, bezierTime)
            .setEaseInOutSine()
            .setOnComplete(() => moveController.SetCameraAttachedToHead(true));
    }

    [System.Serializable]
    public class CameraAttachedToHeadEvent : UnityEvent<bool> { }
}