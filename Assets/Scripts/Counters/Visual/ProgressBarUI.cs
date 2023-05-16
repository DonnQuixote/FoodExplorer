using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image bar_image;
    private IHasProgress hasProgress;
    private void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null)
        {
            Debug.LogError("Game Object " + hasProgressGameObject + " does not have a component that implements" +
                "IhasProgress");
        }
        hasProgress.OnProcessChanged += HasProgress_OnProcessChanged;

        bar_image.fillAmount = 0f;

        Hide();
    }

    private void HasProgress_OnProcessChanged(object sender, IHasProgress.OnProcessChangedEventArgs e)
    {
        bar_image.fillAmount = e.progressNormalized;

        if(e.progressNormalized == 0 || e.progressNormalized >= 1f)
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
