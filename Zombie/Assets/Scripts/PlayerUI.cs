using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;


public class PlayerUI : MonoBehaviour
{
    [HideInInspector] public MultiplayerEventSystem eventSystem;
    private PlayerController player;

    public Image crosshair;
    public GameObject menu;
    public Selectable firstMenuItem;

    public void SetUp(PlayerController player, MultiplayerEventSystem eventSystem)
    {
        this.player = player;
        this.eventSystem = eventSystem;
        eventSystem.playerRoot = gameObject;
    }

    public void ToggleMenu()
    {
        menu.SetActive(!menu.activeSelf);
        eventSystem.SetSelectedGameObject(menu.activeSelf ? firstMenuItem.gameObject : null); 
    }

    public void SetCrosshairOpacity(float opacity)
    {
        Color color = crosshair.color;
        color.a = opacity;
        crosshair.color = color;
    }

}
