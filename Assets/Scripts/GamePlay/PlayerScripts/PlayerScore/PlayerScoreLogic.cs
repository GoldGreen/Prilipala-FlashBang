using System;
using UnityEngine;

public class PlayerScoreLogic : MonoBehaviour
{
    private IDisposable subscriber;
    private Score score;

    [SerializeField] private PlayerScoreGraphics graphics;
    [SerializeField] private float frequencyOfUpdatingScore = 0.5f;
    [SerializeField] private float frequencyOfAddingScore = 0.5f;

    private long addingScore;
    public long ScoreBy1Time { get; set; }

    private new Transform transform;
    private long currentScore = 0;

    private float lastY = 0;

    private void Awake()
    {
        transform = GetComponent<Transform>();
        score = DB.Data.Score;
        score.UpdateScore();
        subscriber = score.OnDataChanged.Subscribe(SetData);
        addingScore = ScoreBy1Time / 2;

        CoroutineT.InfiniteBefore(AddScore, frequencyOfAddingScore).Start(this);

        CoroutineT.InfiniteBefore(UpdateScore, frequencyOfUpdatingScore).Start(this);

        CoroutineT.InfiniteBefore(IncreaseScoreBy1Time, 10).Start(this);
    }

    public long Score => score.CurrentValue;

    private void UpdateScore()
    {
        score.AddScore(currentScore);
        currentScore = 0;
    }

    private void AddScore()
    {
        if (transform.position.y > lastY)
        {
            currentScore += ScoreBy1Time;
            lastY = transform.position.y;
        }
    }

    private void IncreaseScoreBy1Time()
    {
        ScoreBy1Time += addingScore;
    }

    private void SetData(Score score)
    {
        graphics.SetData(score.CurrentValue);
    }

    private void OnDestroy()
    {
        subscriber.Dispose();
    }
}