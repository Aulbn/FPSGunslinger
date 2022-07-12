using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject debugTools;
    public Text timerText;
    public Image crosshair;
    public TextMeshProUGUI ammoText;

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

    public static void ToggleCrosshair(bool show, float time)
    {
        Instance.crosshair.CrossFadeAlpha(show?1:0, time, false);
    }

    public static void SetAmmoText (int mag, int total)
    {
        Instance.ammoText.text = mag + " / " + total;
    }
}
