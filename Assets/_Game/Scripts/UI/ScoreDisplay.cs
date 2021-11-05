using UnityEngine;
using ScriptableObjectArchitecture;
using UnityEngine.UI;
using System;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private IntReference redScore;
    [SerializeField] private IntReference blueScore;

    [SerializeField] private Text redScoreText;
    [SerializeField] private Text blueScoreText;

    private Action OnRedScoreChanged;
    private Action OnBlueScoreChanged;

    private void Awake()
    {
        OnRedScoreChanged = delegate { UpdateScores(); };
        OnBlueScoreChanged = delegate { UpdateScores(); };
    }

    private void OnEnable()
    {
        redScore.AddListener(OnRedScoreChanged);
        blueScore.AddListener(OnBlueScoreChanged);
    }

    private void UpdateScores()
    {
        redScoreText.text = redScore.Value.ToString();
        blueScoreText.text = blueScore.Value.ToString();
    }
}
