using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public static Player Instance { get; set; }

    

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCount selectedCounter2;
    }


    [SerializeField]private int speed = 10;
    [SerializeField] private float rotateSpeed = 15.0f;
    [SerializeField] InputMessage inputMessage;
    private Vector3 oldInteractDir = Vector3.zero;
    //[SerializeField] ClearCount clearCount;
    private bool isWalking;
    [SerializeField] LayerMask countersLayerMask;
    private ClearCount selectedCounter;


    private void Awake()
    {
        if (Instance !=null)
        {
            Debug.LogError("There has being more than one Player");
        }
        Instance = this;
    }
    private void Start()
    {
        inputMessage.OnInteractAction += InputMessage_OnInteractAction;
    }

    private void InputMessage_OnInteractAction(object sender, System.EventArgs e)
    {

        Debug.Log("°´ÏÂE¼ü");
        Vector2 inputData = inputMessage.InputMovementNormalized();
        Vector3 moveDir = new Vector3(inputData.x, 0, inputData.y);
        if (moveDir != Vector3.zero)
        {
            oldInteractDir = moveDir;
        }
        float interactionDistance = 2.0f;
        if (Physics.Raycast(transform.position, oldInteractDir, out RaycastHit raycastHit, interactionDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out ClearCount clearCount))
            {
                clearCount.Interaction();
            }
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();

    }

    private void HandleInteractions()
    {
        Vector2 inputData = inputMessage.InputMovementNormalized();
        Vector3 moveDir = new Vector3(inputData.x, 0, inputData.y);
        if (moveDir != Vector3.zero)
        {
            oldInteractDir = moveDir;
        }
       
        float interactionDistance = 2.0f;
        if(Physics.Raycast(transform.position, oldInteractDir, out RaycastHit raycastHit, interactionDistance,countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out ClearCount clearCount))
            {
                if (clearCount != selectedCounter)
                {
                    SetSelectedCounter(clearCount);
                }
                //clearCount.Interaction();
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else{
            SetSelectedCounter(null);
        }
        Debug.Log(selectedCounter);
    }

    private void HandleMovement()
    {
        Vector2 inputData = inputMessage.InputMovementNormalized();
        Vector3 moveDir = new Vector3(inputData.x, 0, inputData.y);

        isWalking = moveDir != Vector3.zero;
        //Debug.LogFormat("inputData is {0}", inputData);

        float moveDistance = Time.deltaTime * speed;
        float playerHeight = 2f;
        float playerRadius = .7f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius,
            moveDir, moveDistance);

        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            bool canMoveX = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius,
            moveDirX, moveDistance);

            if (canMoveX)
            {
                transform.position += moveDirX * moveDistance;
            }
            else
            {
                Vector3 moveDirY = new Vector3(0, 0, moveDir.z).normalized;
                bool canMoveY = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius,
            moveDirY, moveDistance);

                if (canMoveY)
                {
                    transform.position += moveDirY * moveDistance;
                }
                else
                {
                    //can't move any directions;
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        transform.forward = Vector3.Lerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    public void SetSelectedCounter(ClearCount clearCount)
    {
        this.selectedCounter = clearCount;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter2 = selectedCounter
        });
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
