using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextScroller : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI pauseButtonText;
    [SerializeField, Range(0, 100)] int speed;
    Vector3 originalPos;
    bool isPaused = false;

    void Start()
    {
        originalPos = text.transform.position;
    }

    // Update is called once per frame
    public void ResetPosition()
    {
        text.transform.position = originalPos;
    }

    public void Pause()
    {
        if(!pauseButtonText)
        {
            return;
        }

        isPaused = !isPaused;
        if (isPaused)
        {
            pauseButtonText.text = "Resume";
        }
        else
        {
            pauseButtonText.text = "Pause";
        }
    }

    void Update()
    {
        if (isPaused)
            return;
        
        text.transform.position += new Vector3(0, speed * Time.deltaTime, 0);
    }
}
