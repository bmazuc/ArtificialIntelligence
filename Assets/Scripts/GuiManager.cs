using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GuiManager : MonoBehaviour {

    private static GuiManager instance;

    public static GuiManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GuiManager>();
            }

            return instance;
        }
    }

    public FormationManager playerFormationMotionManager;

    private GameObject formationButtons;
    private Button actualFormationButton;

    void Start () {
		formationButtons = GameObject.Find("FormationPanel");
        formationButtons.SetActive(false);
        actualFormationButton = formationButtons.transform.Find("RectangleButton").GetComponent<Button>();
    }
	
	void Update () {
		
	}

    public void ShowFormationPanel(bool state)
    {
        formationButtons.SetActive(state);
    }

    public void CircleButton()
    {
        Button button = formationButtons.transform.Find("CircleButton").GetComponent<Button>();

        ColorBlock block = button.colors;
        block.disabledColor = Color.red;
        block.highlightedColor = Color.red;
        button.colors = block;

        block.disabledColor = Color.white;
        block.highlightedColor = Color.white;
        actualFormationButton.colors = block;
        actualFormationButton = button;

        if (playerFormationMotionManager)
            playerFormationMotionManager.SetFormation(E_Formation.CIRCLE);
    }

    public void RectangleButton()
    {
        Button button = formationButtons.transform.Find("RectangleButton").GetComponent<Button>();

        ColorBlock block = button.colors;
        block.disabledColor = Color.red;
        block.highlightedColor = Color.red;
        button.colors = block;

        block.disabledColor = Color.white;
        block.highlightedColor = Color.white;
        actualFormationButton.colors = block;
        actualFormationButton = button;

        if (playerFormationMotionManager)
            playerFormationMotionManager.SetFormation(E_Formation.RECTANGLE);
    }
}
