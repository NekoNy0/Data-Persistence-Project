using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public string playerNameText;

    public int highScore;
    public string highScoreName;

    public static GameManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LaunchGame()
    {
        LoadScore();
        SceneManager.LoadScene("main");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }

    [System.Serializable]
    class SaveData
    {
        public int highScoreSaved;
        public string highScoreNameSaved;
    }

    public void SaveScore()
    {
        SaveData saveFile = new SaveData();
        saveFile.highScoreNameSaved = highScoreName;
        saveFile.highScoreSaved = highScore;

        string json = JsonUtility.ToJson(saveFile);

        File.WriteAllText(Application.persistentDataPath + "/scoreData.json", json);
    }

    public void LoadScore()
    {
        string scoreDataPath = Application.persistentDataPath + "/scoreData.json";

        if (File.Exists(scoreDataPath))
        {
            string json = File.ReadAllText(scoreDataPath);
            SaveData loadFile = JsonUtility.FromJson<SaveData>(json);

            highScoreName = loadFile.highScoreNameSaved;
            highScore = loadFile.highScoreSaved;
        }
    }
}
