using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetEaterAnimationEvents : MonoBehaviour
{
    public MainMenuController mainMenuController;

    public void DisableUI()
    {
        mainMenuController.UIGroup.SetActive(false);
    }
}
