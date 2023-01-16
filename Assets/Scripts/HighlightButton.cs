using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightButton : MonoBehaviour
{
    Color defaultColor;
    Color highlightColor;
    [SerializeField] Button button;
    [SerializeField] AudioClip soundEffect;
    [SerializeField] AudioSource source;

    private void Start()
    {
        defaultColor = button.colors.normalColor;
    }
    private void OnMouseEnter()
    {
        source.PlayOneShot(soundEffect);
        ColorBlock colors = button.colors;
        colors.normalColor = highlightColor;
        button.colors = colors;
    }
    private void OnMouseExit()
    {
        ColorBlock colors = button.colors;
        colors.normalColor = defaultColor;
        button.colors = colors;
    }
}
