using UnityEngine;

public class TeamSetup : MonoBehaviour
{
    [Header("Red Team")]
    [SerializeField] private Transform redSpawn;
    [SerializeField] private GameObject redPlayerPrefab;

    [Header("Blue Team")]
    [SerializeField] private Transform blueSpawn;
    [SerializeField] private GameObject bluePlayerPrefab;

    [Header("Settings")]
    [SerializeField] private int teamSize;

    /// <summary>
    /// Spawn players for each team randomly inside their spawn area.
    /// </summary>
    private void Start()
    {
        for (int i = 0; i < teamSize; i++)
        {
            var _point = (Random.insideUnitSphere * 5) + redSpawn.transform.position;
            _point.y = 0;
            Instantiate(redPlayerPrefab, _point, Quaternion.identity);
        }

        for (int i = 0; i < teamSize; i++)
        {
            var _point = (Random.insideUnitSphere * 5) + blueSpawn.transform.position;
            _point.y = 0;
            Instantiate(bluePlayerPrefab, _point, Quaternion.identity);
        }
    }
}
