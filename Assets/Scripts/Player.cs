using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : LivingEntity,IKitchenObjectParent
{
    private const float PLAYER_RUNSH_TIME = 0.2F;
    public Camera viewCamera;
    [SerializeField] private Transform groundTransform;
    [SerializeField] private GunController gunController;
    public static Player Instance { get; set; }
    
    //private GameObject plane;
    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter2;
    }

    public event EventHandler<OnExtinguishFireArgs> OnExtinguishFire;
    public class OnExtinguishFireArgs : EventArgs
    {
        public Node node;
    }


    [SerializeField]private int moveSpeed = 7;
    [SerializeField]private int runSpeed = 14;
    [SerializeField] private float rotateSpeed = 15.0f;
    [SerializeField] InputMessage inputMessage;

    [SerializeField] private Transform kitchenObjectHoldPoint;

    private Vector3 oldInteractDir = Vector3.zero;

    private bool isWalking;
    private bool isRunning = false;
    private bool directionMoveIsNotIgnited = false;
    private float timeRun = PLAYER_RUNSH_TIME;
    [SerializeField] LayerMask selectedMask;
    [SerializeField] LayerMask moveMask;
    private BaseCounter selectedCounter;
    private KitchenObject onTheGroundSelectedKO;
    private KitchenObject kitchenObject;

    private Vector3 moveDiretion;

    [SerializeField]private KitchenObject fireExtinguish;

    private bool isPressedFirePrepared;
    private bool isGunHanding;
    public bool canMoveByTop = true;
    private void Awake()
    {
        if (Instance !=null)
        {
            Debug.LogError("There has being more than one Player");
        }
        Instance = this;
    }
    protected override void Start()
    {
        base.Start();
        //plane = GameObject.Find("Plane");
        inputMessage.OnInteractAction += InputMessage_OnInteractAction;
        inputMessage.OnInteract_CutAction += InputMessage_OnInteract_CutAction;
        inputMessage.OnRush += InputMessage_OnRush;
        inputMessage.OnThrow += InputMessage_OnThrow;
        inputMessage.OnExtinguishFire += InputMessage_OnExtinguishFire;
        inputMessage.OnFireGunChanging += Player_OnFireChanging;
    }

    private void Player_OnFireChanging(object sender, EventArgs e)
    {
        if(isPressedFirePrepared)
        {
            isPressedFirePrepared = false;
        }
        else
        {
            isPressedFirePrepared = true;
        }
    }

    private void InputMessage_OnExtinguishFire(object sender, EventArgs e)
    {
        if (HasKitchenObject() && GetKitchenObject() == fireExtinguish)
        {

            if (!directionMoveIsNotIgnited)
            {
                Node node = Grid.Instance.NodeFromWorldPoint(transform.position + moveDiretion * Grid.Instance.nodeRadius * 2);
                OnExtinguishFire?.Invoke(this, new OnExtinguishFireArgs
                {
                    node = node
                });
            }
        }
    }

    private void InputMessage_OnThrow(object sender, EventArgs e)
    {
        onTheGroundSelectedKO = null;
        if (HasKitchenObject())
        {
            if(!GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                //carrying something but not the plate
                //Debug.Log("something in hand,throw it");

                Vector2 inputData = inputMessage.InputMovementNormalized();
                Vector3 moveDir = new Vector3(inputData.x, 0, inputData.y);
                Rigidbody rigidbody = GetKitchenObject().transform.gameObject.GetComponent<Rigidbody>();
                if (!rigidbody)
                {
                    rigidbody = GetKitchenObject().transform.gameObject.AddComponent<Rigidbody>();
                    rigidbody.mass = 1.0f;
                    rigidbody.drag = 0.2f;
                }
                GetKitchenObject().transform.SetParent(groundTransform);
                this.ClearKitchenObject();
                rigidbody.AddForce(moveDir * 13.0f, ForceMode.Impulse);
            }
            else
            {
                //plate is in hand
            }
           
        }
        else
        {
            //Debug.Log("nothing in hand");
        }
    }

    private void InputMessage_OnRush(object sender, EventArgs e)
    {
        if (isWalking && !isRunning)
        {
            isRunning = true;
            timeRun = PLAYER_RUNSH_TIME;

            //Vector2 inputData = inputMessage.InputMovementNormalized();
            //Vector3 moveDir = new Vector3(inputData.x, 0, inputData.y);
            ////transform.forward = Vector3.Lerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed * 3);

            //transform.position = Vector3.Lerp(transform.position, transform.position + moveDir * 8.0f, Time.deltaTime * rotateSpeed);
        }
        else
        {
            isRunning = false;
        }
    }

    private void InputMessage_OnInteract_CutAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interaction_Cut(this);
        }
    }

    private void InputMessage_OnInteractAction(object sender, System.EventArgs e)
    {

        //Vector2 inputData = inputMessage.InputMovementNormalized();
        //Vector3 moveDir = new Vector3(inputData.x, 0, inputData.y);
        //if (moveDir != Vector3.zero)
        //{
        //    oldInteractDir = moveDir;
        //}
        //float interactionDistance = 2.0f;
        //if (Physics.Raycast(transform.position, oldInteractDir, out RaycastHit raycastHit, interactionDistance, countersLayerMask))
        //{
        //    if(raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
        //    {
        //        baseCounter.Interaction(this);
        //    }
        //}
        if (selectedCounter != null)
        {
            selectedCounter.Interaction(this);

            //Because picking up ingredients from the ground and interacting with the counter are both controlled by button J, there may be some logical irregularities when there are KitchenObjects around the counter. Therefore, the following restrictions are made
            //
            //if (selectedCounter.HasKitchenObject()|| HasKitchenObject()
            //    && (selectedCounter is ContainerCounter||
            //    selectedCounter is CuttingCounter || selectedCounter is StokeCounters ||
            //    selectedCounter is PlateCounter))
            //{
            //    onTheGroundSelectedKO = null;
            //}

            if (!HasKitchenObject())
            {
                if (selectedCounter.HasKitchenObject())
                {
                    onTheGroundSelectedKO = null;
                }
            }
            else
            {
                onTheGroundSelectedKO = null;
            }
        }

        if (onTheGroundSelectedKO != null)
        {
            Destroy(onTheGroundSelectedKO.gameObject.GetComponent<Rigidbody>());
            onTheGroundSelectedKO.gameObject.transform.rotation = onTheGroundSelectedKO.origionRatatoin;
            onTheGroundSelectedKO.SetKitchenObjectParent(this);
            onTheGroundSelectedKO = null;
        }
    }

    private void Update()
    {
        if (canMoveByTop)
        {
            HandleMovement();
        }
        HandleInteractions();

        if (isPressedFirePrepared)
        {
            HandleFire();
        }

    }

    public void LookAt()
    {

        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);

        Plane groudPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groudPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            //Debug.DrawLine(ray.origin, point, Color.red);

            Vector3 lookAtPosition = new Vector3(point.x, transform.position.y, point.z);
            transform.LookAt(lookAtPosition);
        }

    }
    
    private void HandleFire()
    {
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);

        Plane groudPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groudPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            //Debug.DrawLine(ray.origin, point, Color.red);

            Vector3 lookAtPosition = new Vector3(point.x, transform.position.y, point.z);
            transform.LookAt(lookAtPosition);
        }

        // Weapon input
        if (Input.GetMouseButton(0))
        {
            gunController.Shoot();
        }
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
        if(Physics.Raycast(transform.position, oldInteractDir, out RaycastHit raycastHit, interactionDistance,selectedMask))
        {
            //Debug.Log(raycastHit.collider.name);
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
                //clearCount.Interaction();
            }
            else if(raycastHit.transform.TryGetComponent(out KitchenObject kitchenObject))
            {
                if (!HasKitchenObject())
                {
                    onTheGroundSelectedKO = kitchenObject;
                    //Destroy(kitchenObject.gameObject.GetComponent<Rigidbody>());
                    //kitchenObject.SetKitchenObjectParent(this);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else{
            SetSelectedCounter(null);
        }
        //Debug.Log(this.name + ":"+ selectedCounter);
    }

    private void HandleMovement()
    {
        Vector2 inputData = inputMessage.InputMovementNormalized();
        Vector3 moveDir = new Vector3(inputData.x, 0, inputData.y);
        isGunHanding = gunController.startingGun.activeSelf;
        isWalking = moveDir != Vector3.zero;
        if (isWalking)
        {
            moveDiretion = moveDir;
        }

        float moveDistance = Time.deltaTime *(isRunning?runSpeed:moveSpeed) *(isGunHanding?0.75f:1.0f);
        float playerHeight = 2f;
        float playerRadius = .7f;

        directionMoveIsNotIgnited = !(Grid.Instance.NodeFromWorldPoint(transform.position + moveDir * Grid.Instance.nodeRadius * 2).state == Node.State.Ignited);
        //Debug.Log("Fire: " + canMoveIfFire);
        //
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius,
            moveDir, moveDistance, moveMask) && directionMoveIsNotIgnited;

        if (!canMove && directionMoveIsNotIgnited)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            bool canMoveX = moveDirX.x!=0&&!Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius,
            moveDirX, moveDistance);

            if (canMoveX)
            {
                transform.position += moveDirX * moveDistance;
            }
            else
            {
                Vector3 moveDirY = new Vector3(0, 0, moveDir.z).normalized;
                bool canMoveY = moveDirY.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius,
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


        //I found that there is a delay in the rotation of the head when turning 180 degrees, so make it turn to the right track faster
        if (Vector3.Dot(transform.forward.normalized,moveDir) == -1)
        {
            transform.forward = Vector3.Lerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed * 3);
        }
        transform.forward = Vector3.Lerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);

        if (isRunning && timeRun >0) 
        {
            timeRun -= Time.deltaTime;
        }
        if (timeRun < 0)
        {
            isRunning = false;
        }
       
    }

    public void SetSelectedCounter(BaseCounter clearCount)
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

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }



    private void OnCollisionEnter(Collision collision)
    {
       //Debug.Log("OnCollisionEnter:  " + collision.gameObject.name);
    }
}
