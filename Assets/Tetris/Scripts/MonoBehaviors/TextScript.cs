using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextScript : MonoBehaviour
{
    public TextMeshPro textComp;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetText(string text)
    {
        textComp.text = text;
    }

    public void StartFlash()
    {
        StartCoroutine("Flash");
        Invoke("StopFlash", 0.5f);
    }

    private void StopFlash()
    {
        StopCoroutine("Flash");
        textComp.enabled = true;
    }

    IEnumerator Flash()
    {
        while (true)
        {
            textComp.enabled = !textComp.enabled;
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
