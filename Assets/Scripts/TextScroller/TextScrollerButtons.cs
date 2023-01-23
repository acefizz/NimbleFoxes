using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextScrollerButtons : MonoBehaviour
{
    [SerializeField] TextScroller ts;

    public void ResetButton()
    {
        // Reset the text to the original position.
        ts.ResetPosition();
    }

    public void PauseButton()
    {
        // Pause the text.
        ts.Pause();
    }

    public void SkipButton()
    {
        // Skip the text and go to next scene.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ResetGame()
    {
        // Reset the game.
        SceneManager.LoadScene(0);
    }
}
