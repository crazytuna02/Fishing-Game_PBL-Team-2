using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroMenuController : MonoBehaviour
{
    public Button[] menuButtons; // Assign buttons in the inspector
    private int selectedIndex = 0;

    void Start()
    {
        // Set the first button as selected
        UpdateButtonSelection();
    }

    void Update()
    {
        HandleNavigation();
        HandleSelection();
    }

    void HandleNavigation()
    {
        // Navigate down (S or Joystick Down)
        if (Input.GetKeyDown(KeyCode.S) || Input.GetAxis("Vertical") < -0.5f)
        {
            selectedIndex = (selectedIndex + 1) % menuButtons.Length;
            UpdateButtonSelection();
        }

        // Navigate up (W or Joystick Up)
        if (Input.GetKeyDown(KeyCode.W) || Input.GetAxis("Vertical") > 0.5f)
        {
            selectedIndex = (selectedIndex - 1 + menuButtons.Length) % menuButtons.Length;
            UpdateButtonSelection();
        }
    }

    void HandleSelection()
    {
        // Confirm selection (Space or Joystick Button 0)
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Submit"))
        {
            menuButtons[selectedIndex].onClick.Invoke();
        }
    }

    void UpdateButtonSelection()
    {
        // Highlight the currently selected button
        for (int i = 0; i < menuButtons.Length; i++)
        {
            ColorBlock colors = menuButtons[i].colors;
            colors.normalColor = (i == selectedIndex) ? Color.yellow : Color.white;
            menuButtons[i].colors = colors;
        }
    }
}
