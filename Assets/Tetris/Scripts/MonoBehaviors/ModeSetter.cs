using UnityEngine;
using System.Collections;

public class ModeSetter : MonoBehaviour
{

    public void SetMode(int modeNum)
    {
        GameModeSettings.SelectedMode = (GameModes) modeNum;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
