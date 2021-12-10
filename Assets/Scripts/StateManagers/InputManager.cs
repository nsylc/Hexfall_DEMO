using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public static event EventHandler<OnStateExitEventArgs> OnStateExit;
    public class OnStateExitEventArgs : EventArgs
    {
        public bool isClockwise;
    }
    private bool isStateActive = false;
    public bool isFirstInput = true;
    private Vector3 initialTouchPosition;
    private Vector3 finalTouchPosition;
    private bool isSelectedHexagon = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        ExplosionManager.OnStateExit += InputManager_OnStateExit;
    }

    private void InputManager_OnStateExit(object sender, ExplosionManager.OnStateExitEventArgs e)
    {
        if (e.stateToGo == States.Input)
            EnterState();
    }

    private void EnterState()
    {
        isStateActive = true;

        ConfigureForFirstInput();
    }

    private void ExitState(bool isClockwise)
    {
        isStateActive = false;
        OnStateExit?.Invoke(this, new OnStateExitEventArgs { isClockwise = isClockwise });
    }

    private void Update()
    {
        if (isStateActive)
        {
            OnTouch();
        }
    }

    private void OnTouch()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase.Equals(TouchPhase.Began))
            {
                Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                initialTouchPosition = position;

                if (position == null)
                    return;

                RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(position.x, position.y), Vector2.zero, 0f);

                if (hitInfo.collider == null || hitInfo.transform.gameObject.tag != "Hexagon")
                    return;

                isSelectedHexagon = SelectionManager.Instance.SelectedHexagons.Contains(hitInfo.transform.gameObject);
                if (!isSelectedHexagon)
                    SelectionManager.Instance.MakeSelection(hitInfo.transform.gameObject, hitInfo);

            }

            if (Input.GetTouch(0).phase.Equals(TouchPhase.Ended))
            {
                finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (isSelectedHexagon)
                {
                    isSelectedHexagon = false;
                    ExitState(IsClockWise());
                }
            }
        }
    }

    private bool IsClockWise()
    {
        Vector3 setInitialToOrigin = (initialTouchPosition - SelectionManager.Instance.GetSelectionCenter());
        Vector3 setFinalToOrigin = (finalTouchPosition - SelectionManager.Instance.GetSelectionCenter());
        int initialRegion = PositionToCoorRegion(setInitialToOrigin);
        int finalRegion = PositionToCoorRegion(setFinalToOrigin);
        bool isClockwise = true;
        if (initialRegion > finalRegion)
            isClockwise = false;
        if (initialRegion == 1 && finalRegion == 4)
            isClockwise = false;
        if (initialRegion == 4 && finalRegion == 1)
            isClockwise = false;
        return isClockwise;
    }

    private int PositionToCoorRegion(Vector3 setInitialToOrigin)
    {
        if (setInitialToOrigin.x >= 0 && setInitialToOrigin.y >= 0)
            return 1;
        if (setInitialToOrigin.x < 0 && setInitialToOrigin.y >= 0)
            return 4;
        if (setInitialToOrigin.x < 0 && setInitialToOrigin.y < 0)
            return 3;
        if (setInitialToOrigin.x >= 0 && setInitialToOrigin.y < 0)
            return 2;
        return 0;
    }

    

    public void ConfigureForFirstInput()
    {
        if (isFirstInput)
        {
            UserInterfaceManager.Instance.DisplayInGameUI();
            ScoreManager.StartScore();
            RepositionManager.Instance.SetSpeed(2.5f);
            isFirstInput = false;
        }

    }
}