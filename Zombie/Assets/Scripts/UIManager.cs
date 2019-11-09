using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject debugTools;

    public Image crosshair;
    public Text timerText;


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backslash)) //Toggle debug tools
            debugTools.SetActive(!debugTools.activeSelf);
    }

    public static void SetTimerText(float time)
    {
        if (!Instance.debugTools.activeSelf) return;

        int min = Mathf.FloorToInt(time / 60);
        int sec = Mathf.FloorToInt(time % 60);
        Instance.timerText.text = min.ToString("00") + ":" + sec.ToString("00");
    }


    public static void SetCrosshairOpacity(float opacity)
    {
        Color color = Instance.crosshair.color;
        color.a = opacity;
        Instance.crosshair.color = color;
    }
}
