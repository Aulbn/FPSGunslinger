using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Image crosshair;

    private void Awake()
    {
        Instance = this;
    }

    public static void SetCrosshairOpacity(float opacity)
    {
        Color color = Instance.crosshair.color;
        color.a = opacity;
        Instance.crosshair.color = color;
    }
}
