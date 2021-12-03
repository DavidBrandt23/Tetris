using TMPro;
using UnityEngine;

public class GameOverScript : MonoBehaviour
{
    TextMeshPro scoreText;
    GameObject backButton;

    // Start is called before the first frame update
    void Start()
    {

        scoreText = GameObject.Find("GameOverText").GetComponent<TextMeshPro>();
        backButton = GameObject.Find("GameOverBack");
        scoreText.enabled = false;
        backButton.SetActive(false);
    }

    public void OnGameOver()
    {
        scoreText.enabled = true;
        backButton.SetActive(true);
    }
}
