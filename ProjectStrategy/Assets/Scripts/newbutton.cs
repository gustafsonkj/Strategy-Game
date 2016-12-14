using UnityEngine;
using System.Collections;

public class newbutton : MonoBehaviour {

	public void loadGame1()
    {
       // saver mySaver = this.gameObject.AddComponent<saver>();
        saver.loadGame();
    }
}
