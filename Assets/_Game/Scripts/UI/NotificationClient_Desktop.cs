using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NotificationClient_Desktop : MonoBehaviour
{
    [SerializeField] private float messageShowDelay = 2;
    [SerializeField] private float messageTransitionDelay = 0.2f;

    [SerializeField] private GameObject displayPanel;
    [SerializeField] private Text displayText;

    private WaitForSeconds messageShow;
    private WaitForSeconds messageTransition;

    private void Awake()
    {
        if (displayText == null || displayPanel == null)
        {
            Debug.LogError("No text/panel found for notification display!", this);
        }

        messageShow = new WaitForSeconds(messageShowDelay);
        messageTransition = new WaitForSeconds(messageTransitionDelay);
    }

    // want this to display a message for a few seconds, then fade, then display the next if there is one

    // if no more it shouldn't keep polling

    public void CheckNotifs()
    {
        
    }

    private IEnumerator ShowMessageRoutine()
    {
        string notif = string.Empty;
        if (NotificationManager.Instance.GetNextNotification(ref notif))
        {
            displayText.text = notif;
            displayPanel.SetActive(true);
            yield return messageShow;
            displayPanel.SetActive(false);
            displayText.text = "";
        }
        else { }
       
    }
}
