using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText = null;

    protected virtual void Update()
    {
        _scoreText.text = ScoreManager.Score.ToString();
    }
}
