using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KeyMappingUI : MonoBehaviour
{
    public static KeyMappingUI Instance { get; private set; }

    [SerializeField] private KeyMappingUI keyMappingUI;

    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private Button moveUpButtion;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private Button moveDownButtion;
    [SerializeField] private Button resetButtion;
    [SerializeField] private Button saveButtion;



    private void Awake()
    {
        Instance = this;
        //Hide();
        moveUpButtion.onClick.AddListener(() =>
        {
            moveUpText.text = null;
            RebindBinding(InputMessage.Binding.Move_Up);
        });

        moveDownButtion.onClick.AddListener(() =>
        {
            moveDownText.text = null;
            RebindBinding(InputMessage.Binding.Move_Down);
        });

        resetButtion.onClick.AddListener(() =>
        {
            Debug.Log("in reset");
            InputMessage.Instance.PlayerPrefsMappingReset();
        });

        saveButtion.onClick.AddListener(() =>
        {
            Debug.Log("in save");

            InputMessage.Instance.PlayerPrefsMappingSave();
        });

        UpdateVisual();
    }

    public void UpdateVisual()
    {
        moveUpText.text = InputMessage.Instance.GetBindingText(InputMessage.Binding.Move_Up);
        moveDownText.text = InputMessage.Instance.GetBindingText(InputMessage.Binding.Move_Down);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void RebindBinding(InputMessage.Binding binding)
    {
        InputMessage.Instance.RebindBinding(binding,()=> { UpdateVisual(); });
    }
}
