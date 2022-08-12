using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.K))
			Restart();
	}
	public void Restart()
	{
		SceneManager.LoadScene(0);
	}
}