using TMPro;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    TextMeshPro scoreText;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshPro>();
        UpdateText();
    }

    public void AddToScore(int toAdd)
    {
        score += toAdd;
        UpdateText();
    }

    private void UpdateText()
    {
        scoreText.text = "Score: " + score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
