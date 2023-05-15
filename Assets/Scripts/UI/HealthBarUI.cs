using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasHealthGameObject;
    [SerializeField] private Image bar_image;
    private LivingEntity livingEntity;
    private void Start()
    {
        livingEntity = hasHealthGameObject.GetComponent<LivingEntity>();
        if (livingEntity == null)
        {
            Debug.LogError("Game Object " + hasHealthGameObject + " does not have a component that implements" +
                "IhasProgress");
        }
        //Debug.Log(hasHealthGameObject.name);
        livingEntity.OnHealthChange += LivingEntity_OnHealthChange; ;

        bar_image.fillAmount = 1f;

        Hide();
    }

    private void LivingEntity_OnHealthChange(object sender, LivingEntity.OnHealthChangeArgs e)
    {
        bar_image.fillAmount = e.health;

        if (e.health == 0 || e.health >= 1f)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }


    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}
