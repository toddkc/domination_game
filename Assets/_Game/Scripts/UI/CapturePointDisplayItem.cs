using UnityEngine;
using UnityEngine.UI;

public class CapturePointDisplayItem : MonoBehaviour
{
    private CapturePoint linkedPoint;
    private Image image;
    private Text text;

    private void Awake()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
    }

    public void SetupDisplayPoint(CapturePoint point)
    {
        linkedPoint = point;
        text.text = linkedPoint.GetName.ToString();
        linkedPoint.OnPointCaptured += UpdateDisplay;
    }

    private void OnDisable()
    {
        linkedPoint.OnPointCaptured -= UpdateDisplay;
    }

    public void UpdateDisplay(Team team)
    {
        if (team == Team.Red)
        {
            image.color = Color.red;
        }
        else if (team == Team.Blue)
        {
            image.color = Color.blue;
        }
        else if (team == Team.None)
        {
            image.color = Color.green;
        }
    }
}
