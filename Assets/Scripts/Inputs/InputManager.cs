using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // There should be only one InputManager in the scene
    private static InputManager instance;
    public static InputManager Instance {  get { return instance; } }

    // Action schemes
    private RunnerInputAction actionScheme;

    // Configuration
    [SerializeField] private float sqrSwipeDeadzone = 50.0f;

    #region Public Properties
    public Vector2 TouchPosition { get { return touchPosition; } }
    public Vector2 StartDrag { get { return startDrag; } }
    public bool Tap { get { return tap; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }
    #endregion

    #region Private Properties
    private Vector2 touchPosition;
    private Vector2 startDrag;
    private bool tap;
    private bool swipeLeft;
    private bool swipeRight;
    private bool swipeUp;
    private bool swipeDown;
    #endregion

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        SetupControl();
    }
    private void LateUpdate()
    {
        ResetInputs();
    }
    private void ResetInputs()
    {
        tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;
    }
    private void SetupControl()
    {
        actionScheme = new RunnerInputAction();

        // Register different actions
        actionScheme.Gameplay.Tap.performed += ctx => OnTap(ctx);
        actionScheme.Gameplay.TouchPosition.performed += ctx => OnPosition(ctx);
        actionScheme.Gameplay.StartDrag.performed += ctx => OnStartDrag(ctx);
        actionScheme.Gameplay.EndDrag.performed += ctx => OnEndDrag(ctx);
    }

    private void OnEndDrag(InputAction.CallbackContext ctx)
    {
        Vector2 delta = touchPosition - startDrag;
        float sqrDistance = delta.sqrMagnitude;
        Debug.Log("TESTENDDRAG");
        Debug.Log("SQRDISTANCE: " + sqrDistance + " AND DEADZONE: " + sqrSwipeDeadzone);
        // Confirmed swipe
        if (sqrDistance > sqrSwipeDeadzone)
        {
            float x = Mathf.Abs(delta.x);
            float y = Mathf.Abs(delta.y);
            if (x > y) // Left or Right
            {
                if(delta.x > 0)
                {
                    swipeRight = true;
                    Debug.Log("TESTRIGHT");
                }
                else
                {
                    swipeLeft = true;
                    Debug.Log("TESTLEFT");
                }
            }
            else // Up or Down
            {
                if(delta.y > 0)
                {
                    swipeUp = true;
                    Debug.Log("TESTUP");
                }
                else
                {
                    swipeDown = true;
                    Debug.Log("TESTDOWN");
                }
            }
        }
    }

    private void OnStartDrag(InputAction.CallbackContext ctx)
    {
        startDrag = touchPosition;
        Debug.Log("TESTSTARTDRAG");
    }

    private void OnPosition(InputAction.CallbackContext ctx)
    {
        touchPosition = ctx.ReadValue<Vector2>();
        Debug.Log("TESTONPOS");
    }

    private void OnTap(InputAction.CallbackContext ctx)
    {
        tap = true;
        Debug.Log("TESTTAP");
    }

    private void OnEnable()
    {
        actionScheme.Enable();
    }
    private void OnDisable()
    {
        actionScheme.Disable();
    }

}
