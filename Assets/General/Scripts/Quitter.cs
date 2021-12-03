using UnityEngine;

public class Quitter : MonoBehaviour
{
    public bool QuitOnEscape;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (QuitOnEscape && Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
