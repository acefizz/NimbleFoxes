using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
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
        button = GetComponent<Button>();
    }
    public void OnMouseEnter()
    {
        source.PlayOneShot(soundEffect);
        ColorBlock colors = button.colors;
        colors.normalColor = highlightColor;
        button.colors = colors;
    }
    public void OnMouseExit()
    {
        ColorBlock colors = button.colors;
        colors.normalColor = defaultColor;
        button.colors = colors;
    }
}

