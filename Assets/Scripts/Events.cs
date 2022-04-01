
using UnityEngine.SceneManagement;
using UnityEngine;

public class Events : MonoBehaviour {

	public void ReplayGame()
	{
		SceneManager.LoadScene ("Main");
	}

	public void QuitGame()
	{
		Application.Quit ();
	}

}
