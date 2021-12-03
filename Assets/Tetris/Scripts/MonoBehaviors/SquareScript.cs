using System.Collections;
using UnityEngine;

public class SquareScript : MonoBehaviour
{
    public Sprite bgBorderSquare;
    public Sprite blockBorderSquare;
    public SpriteRenderer bgRender;
    public SpriteRenderer borderRender;
    public SpriteRenderer flashSquare;

    bool isFull;
    bool haveBG;
    bool hidden;

    // Start is called before the first frame update
    void Start()
    {
    }

    static public GameObject GetChildGameObject(GameObject fromGameObject, string withName)
    {
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts) { if (t.gameObject.name == withName) { return t.gameObject; } }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartFlash()
    {
        StartCoroutine("Flash");
        Invoke("StopFlash", 0.5f);
    }

    private void StopFlash()
    {
        StopCoroutine("Flash");
        flashSquare.enabled = false;
    }

    IEnumerator Flash()
    {
        while (true)
        {
            flashSquare.enabled = !flashSquare.enabled;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public bool GetIsFull()
    {
        return isFull;
    }

    public Color GetColor()
    {
        return bgRender.color;
    }

    public void SetHaveBG(bool val)
    {
        haveBG = val;
        SetFull(false);
    }

    public void SetHidden(bool val)
    {
        hidden = val;
        SetFull(false);
    }

    public void SetFull(bool val, Color? color = null)
    {
        if(color == null)
        {
            color = Color.black;
        }
        isFull = val;
        Color backgroundColor = new Color(0.1f, 0.1f, 0.1f);

        Color newColor = isFull ? (Color)color : backgroundColor;
        Sprite borderImage = isFull ? blockBorderSquare : bgBorderSquare;
        borderRender.sprite = borderImage;
        bgRender.color = newColor;
        borderRender.color = newColor;

        bool showAnything = true;
        if (hidden)
        {
            showAnything = false;
        }
        else if (!haveBG)
        {
            showAnything = isFull;
        }
        else
        {
            showAnything = true;
        }
        bgRender.enabled = showAnything;
        borderRender.enabled = showAnything;
    }
}
