using Cinemachine;

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingGunSetUI : MonoBehaviour
{

    [SerializeField]GameObject go;
    [SerializeField]GameObject lightGunGo;
    [SerializeField]GameObject middleGunGo;
    [SerializeField]GameObject heavyGunGo;
    [SerializeField]Button purchaseLightButton;
    [SerializeField]Button purchaseHeavyButton;
    [SerializeField]Button purchaseMiddleButton;
    [SerializeField]Button backButton;
    [SerializeField]int lightCost = 15;
    [SerializeField]int middleCost = 32;
    [SerializeField]int heavyCost = 50;
    [SerializeField] private TextMeshProUGUI scoreNotEnoughText;

    public Camera viewCamera;
    public CinemachineVirtualCamera followCamera;
    public CinemachineVirtualCamera standCamera;
    private CinemachineVirtualCamera currentCamera;
    private bool isSwitching = false;

    private bool isPressedZKey;
    private int playerReCheckIsYes=0;
    private List<Enemy> listEnermy;
    private int count = 0;
    private Coroutine coroutine;


    private float radius = 20.0f;
    private float leftWidthStart = -2.5f;
    private float rightWidth = 10.0f;
    private float topHeight = 2f;
    private float bottomHeight = -5f;
    private float square = -19.0f;
    private bool fitstInShopping = true;
    private void Start()
    {
        currentCamera = followCamera;
        listEnermy = new List<Enemy>();
        listEnermy = FindAllEnemies();
        ShopCounter.Instance.OnInteractionWithShop += ShoppingGunSetUI_OnInteractionWithShop;
        InputMessage.Instance.OnSetGunQuicklypPerformed += ShoppingGunSetUI_OnSetGunQuicklypPerformed;
        InputMessage.Instance.OnSetGunQuicklypCancled += ShoppingGunSetUI_OnSetGunQuicklypCancled;
        //PlayerReCheckUI.Instance.OnPlayerClickReCheckButton += ShoppingGunSetUI_OnPlayerClickReCheckButton;
        purchaseLightButton.onClick.AddListener(()=>PurchaseGunButtonClick(lightGunGo,lightCost));
        purchaseMiddleButton.onClick.AddListener(()=>PurchaseGunButtonClick(middleGunGo,middleCost));
        purchaseHeavyButton.onClick.AddListener(()=>PurchaseGunButtonClick(heavyGunGo,heavyCost));
        backButton.onClick.AddListener(() => {
            scoreNotEnoughText.enabled = false;
            Hide();
            Time.timeScale = 1;
        });
        Hide();
        scoreNotEnoughText.enabled = false;
    }

    private void ShoppingGunSetUI_OnPlayerClickReCheckButton(object sender, PlayerReCheckUI.OnPlayerClickReCheckButtonArgs e)
    {
        playerReCheckIsYes = e.agreeInstantiatePosition;
    }

    private void ShoppingGunSetUI_OnSetGunQuicklypCancled(object sender, System.EventArgs e)
    {
        Debug.Log("z cancled");
        isPressedZKey = false;
    }

    private void ShoppingGunSetUI_OnSetGunQuicklypPerformed(object sender, System.EventArgs e)
    {
        Debug.Log("z pressed");
        isPressedZKey = true;
    }

    private void PurchaseGunButtonClick(GameObject go,int cost)
    {
        if(DeliveryManager.Instance.ChangeSuccessedScore(cost) < 0)
        {
            scoreNotEnoughText.enabled = true;
            return;
        }
        Time.timeScale = 1.0f;


        if (fitstInShopping)
        {
            SwitchToStandCamera();
            coroutine = StartCoroutine(WaitForSwitchCamera());
            
        }
        
        Instantiate(go, RandomPosition(), go.transform.rotation);
    }

    private IEnumerator WaitForSwitchCamera()
    {
        Time.timeScale = 1.0f;
        Debug.Log("in");
        listEnermy = FindAllEnemies();
        float speedTemp = 0;
        if (listEnermy.Count != 0)
        {
             speedTemp = listEnermy[0].GetComponent<UnityEngine.AI.NavMeshAgent>().speed;
        }
        foreach (Enemy e in listEnermy)
        {
            e.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 0;
        }
        Spawner.Instance.isSpawn = false;
        Player.Instance.canMoveByTop = false;
        backButton.enabled = false;
        yield return new WaitForSeconds(1.5f);

        Debug.Log("2s later");

        foreach (Enemy e in listEnermy)
        {
            e.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = speedTemp;
        }
        Spawner.Instance.isSpawn = true;
        Player.Instance.canMoveByTop = true;
        followCamera.gameObject.SetActive(true);
        StopCoroutine(coroutine);
        fitstInShopping = false;
        backButton.enabled = true;
        Hide();
    }

    private void SwitchToStandCamera()
    {
        Debug.Log("SwitchToStandCamera()");
        if(!isSwitching && currentCamera == followCamera)
        {
            isSwitching = true;
            followCamera.gameObject.SetActive(false);
        }
    }

    private List<Enemy> FindAllEnemies()
    {
        listEnermy.Clear();
        Enemy[] temp = FindObjectsOfType<Enemy>();
        
        foreach(Enemy e in temp)
        {
            listEnermy.Add(e);
        }
        //Debug.Log("listEnermy.size(): " + listEnermy.Count);
        return listEnermy;
    }

    private void ShoppingGunSetUI_OnInteractionWithShop(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private Vector3 RandomPosition()
    {
        Vector3 result = Vector3.zero;
        result.y = 1.0f;
        float temp,temp2;
        List<float> listTwoDirections = new List<float>();
        temp = Random.Range(leftWidthStart, rightWidth);
        result.x = temp;
        temp = Random.Range(bottomHeight, topHeight);
        temp2 = Random.Range(-25.0f,-17.0f);
        Debug.Log(temp2);
        listTwoDirections.Add(temp);
        listTwoDirections.Add(temp2);
        result.z= listTwoDirections[Random.Range(0,2)];
        Debug.Log(result);
        return result;
    }
}


