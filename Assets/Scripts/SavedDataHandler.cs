using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SavedDataHandler : MonoBehaviour
{
    public static SavedDataHandler Instance;

    public int level;
    public int currency;
    public int stackUpgrade;

    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code
        LoadScore();
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    [System.Serializable]
    class SaveData
    {
        public int level;
        public int currency;
        public int stackUpgrade;
    }
    public void ResetScore()
    {
        level = 1;
        currency = 0;
        stackUpgrade = 0;
        SaveScore();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void SaveScore()
    {
        SaveData data = new SaveData();
        data.level = level;
        data.currency = currency;
        data.stackUpgrade = stackUpgrade;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            level = data.level;
            currency = data.currency;
            stackUpgrade = data.stackUpgrade;
        }
    }
}
