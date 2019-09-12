using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_Alpha : MonoBehaviour {

	public void PlayGame()
    {
        //Go to tutorial if no save
        SaveData data = Loader.GetJSONSaveFile();
        if (data == null) data = Loader.GetEncryptedSaveFile();
        if (data == null)
        {
            SceneManager.LoadScene("Tutorial");
            return;
        }

        string sceneName = "";
        data.Load(GameMetaInfo._STATE_DATA[(int)StateData.Scene], ref sceneName);
        if (sceneName == "")
        {
            SceneManager.LoadScene("Tutorial");
            return;
        }
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
