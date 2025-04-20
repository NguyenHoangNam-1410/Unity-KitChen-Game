using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;
    private IHasProgress hasProgress;

    private void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null)
        {
            Debug.LogError("Game Obj: " + hasProgressGameObject + " does not implemented!");
            return;
        }
        hasProgress.OnProgessChanged += HasProgress_OnProgessChanged;
        barImage.fillAmount = 0f;
        Hide();
    }

    private void HasProgress_OnProgessChanged(object sender, IHasProgress.OnProgessChangedEventArg e)
    {
        barImage.fillAmount = e.progressNormalize;

        if (e.progressNormalize == 0f || e.progressNormalize == 1f)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
