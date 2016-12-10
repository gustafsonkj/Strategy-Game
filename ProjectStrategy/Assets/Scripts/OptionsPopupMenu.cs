using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsPopupMenu : Menu
{
	protected override void Init()
	{
		AddItem("Exit Map");
        AddItem("Game Info");
        AddItem("Save Game");

		ButtonStyle.contentOffset = new Vector2(4, 0);
	}

	protected override void OnButtonPress(string item)
	{
		switch (item)
		{
		case "Exit Map":
			Application.LoadLevel("MainMenu");
			break;
        case "Save Game":
            DataRetriever.saveAllData();
            saver.saveGame();
            break;
		}

		Hide();
	}
}