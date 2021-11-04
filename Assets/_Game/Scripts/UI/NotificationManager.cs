using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    [SerializeField] private float messageShowDelay = 5;
    [SerializeField] private float messageTransitionDelay = 1f;

    [SerializeField] private GameObject displayPanel;
    [SerializeField] private Text displayText;

    [SerializeField] private bool debugNotifications = false;

    private WaitForSeconds messageShow;
    private WaitForSeconds messageTransition;

    public static NotificationManager Instance;

    private Queue<string> notifQueue = null;
    private bool isRunning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            notifQueue = new Queue<string>();
            messageShow = new WaitForSeconds(messageShowDelay);
            messageTransition = new WaitForSeconds(messageTransitionDelay);
            AddNotification("Notifications enabled.");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //// TODO: remove this
    //int testCounter = 0;
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        testCounter++;
    //        AddNotification("test " + testCounter);
    //    }
    //}
    //// end TODO

    public bool AddNotification(string notif)
    {
        if (string.IsNullOrEmpty(notif))
        {
            return false;
        }

        if (debugNotifications) Debug.Log(notif);
        
        notifQueue.Enqueue(notif);

        if (!isRunning)
        {
            StartCoroutine(ShowMessageRoutine());
        }

        Debug.Log("Notification added: " + notif);

        return true;
    }

    public bool GetNextNotification(ref string notif)
    {
        if (notifQueue.Count == 0)
        {
            return false;
        }

        notif = notifQueue.Dequeue();
        return true;
    }

    public bool ClearNotifications()
    {
        notifQueue.Clear();
        return true;
    }

    private IEnumerator ShowMessageRoutine()
    {
        isRunning = true;
        string notif = string.Empty;

        while (GetNextNotification(ref notif))
        {
            yield return messageTransition;
            displayText.text = notif;
            displayPanel.SetActive(true);
            yield return messageShow;
            displayPanel.SetActive(false);
            displayText.text = "";
        }
        Debug.Log("No more notifs.");
        isRunning = false;
    }
}
