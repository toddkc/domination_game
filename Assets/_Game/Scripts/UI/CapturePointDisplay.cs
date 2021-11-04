using UnityEngine;

public class CapturePointDisplay : MonoBehaviour
{
    [SerializeField] private GameObject captureProgressDisplay;
    [SerializeField] private Transform capturePointsDisplay;
    [SerializeField] private GameObject capturePointItem;

    private void Start()
    {
        var points = FindObjectsOfType<CapturePoint>();
        foreach (var point in points)
        {
            var item = Instantiate(capturePointItem, capturePointsDisplay);
            item.GetComponent<CapturePointDisplayItem>().SetupDisplayPoint(point);
        }
    }
}
