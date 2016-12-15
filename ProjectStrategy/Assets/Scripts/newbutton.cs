using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;

public class newbutton : MonoBehaviour {

	public void loadGame1()
    {
        //saver mySaver = this.gameObject.AddComponent<saver>();
        if(File.Exists(Application.persistentDataPath + "/strategygame.save"))
        {
            saver.loadGame();
        }
        else
        {
            SceneManager.LoadScene("Main");
        }
    }
}
