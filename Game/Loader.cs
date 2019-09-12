using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public static bool LoadGame(bool fileEncrypted)
    {
        string file = GameManager.Instance.GameSaveFile;

        if (File.Exists(file))
        {
            //Get data from file
            Debug.Log("Deserialising file");
            SaveData savedData = new SaveData();

            if (fileEncrypted)
            {
                using (var fs = File.OpenRead(file))
                {
                    using (var reader = new BsonReader(fs))
                    {
                        var serializer = new JsonSerializer();
                        savedData = serializer.Deserialize<SaveData>(reader);
                    }
                }
            }
            else
            {
                string dataAsJSON = File.ReadAllText(file);
                savedData = JsonConvert.DeserializeObject<SaveData>(dataAsJSON);
            }

            if (savedData == null) return false;

            LoadData(savedData);
            return true;
        }
        else
        {
            return false;
        }
    }

    private static bool LoadData(SaveData data)
    {
        //Ensure correct scene loaded
        string sceneName = "";
        data.Load(GameMetaInfo._STATE_DATA[(int)StateData.Scene], ref sceneName);
        if (!sceneName.Equals(""))
        {
            if (SceneManager.GetActiveScene().name != sceneName)
            {
                SceneManager.LoadScene(sceneName);
            }
        }

        LoadAbilityKeybinds(data);
        LoadScriptableObjects(data);

        int difficulty = 0;
        data.Load(GameMetaInfo._STATE_DATA[(int)StateData.GameDifficulty], ref difficulty);
        GameMetaInfo._GAME_DIFFICULTY = (Difficulty)difficulty;

        Vector3 newPlayerPos = new Vector3();
        data.Load(GameMetaInfo._STATE_DATA[(int)StateData.PlayerPosition], ref newPlayerPos);
        if(newPlayerPos != Vector3.zero) PlayerManager.Instance.player.transform.position = newPlayerPos;

        int playerHP = 0;
        data.Load(GameMetaInfo._STATE_DATA[(int)StateData.PlayerHP], ref playerHP);
        if(playerHP != 0) PlayerManager.Instance.player.GetComponent<AgentStats>().CurrentHealth = playerHP;

        return true;
    }

    public static SaveData GetEncryptedSaveFile()
    {
        //Set difficulty in json file
        if (File.Exists(GameMetaInfo._SAVE_FILE_ENCRYPTED))
        {
            //Get data from file
            SaveData savedData = new SaveData();
            using (var fs = File.OpenRead(GameMetaInfo._SAVE_FILE_ENCRYPTED))
            {
                using (var reader = new BsonReader(fs))
                {
                    var serializer = new JsonSerializer();
                    savedData = serializer.Deserialize<SaveData>(reader);
                }
            }
            return savedData;
        }
        return null;
    }

    public static SaveData GetJSONSaveFile()
    {
        if (File.Exists(GameMetaInfo._SAVE_FILE_JSON))
        {
            //Get save game file
            SaveData savedData = new SaveData();
            string dataAsJSON = File.ReadAllText(GameMetaInfo._SAVE_FILE_JSON);
            savedData = JsonConvert.DeserializeObject<SaveData>(dataAsJSON);
            return savedData;
        }
        return null;
    }
    
}
