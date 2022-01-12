using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyNumber, upgradeButton, endTitle;
    [SerializeField] private Image stackFill;
    [SerializeField] private GameObject gold, diamond, platform, obstacle, finish;
    [SerializeField] private GameObject titleScreen, inGameScreen, endScreen;
    [SerializeField] private Transform levelHierarchy;
    [SerializeField] private GameObject wonButton, lostButton;
    private int collectedDiamond;
    private GameObject instancedFinish;
    private void Start()
    {
        currencyNumber.text = SavedDataHandler.Instance.currency.ToString();
        collectedDiamond = 0;
        AtTitle();
        upgradeButton.text = string.Format("Currency {0} | Upgrade {1}", SavedDataHandler.Instance.currency, SavedDataHandler.Instance.stackUpgrade);
        GenerateLevel(SavedDataHandler.Instance.level);
    }

    public void AtTitle()
    {
        titleScreen.SetActive(true);
        inGameScreen.SetActive(false);
        endScreen.SetActive(false);
    }

    public void GameStart()
    {
        titleScreen.SetActive(false);
        inGameScreen.SetActive(true);
        endScreen.SetActive(false);
        FindObjectOfType<CharacterBehaviour>().StartRun();
    }
    public void Collect()
    {
        collectedDiamond++;
    }
    public void GameOver()
    {
        FindObjectOfType<CharacterBehaviour>().StopRun();
        titleScreen.SetActive(false);
        inGameScreen.SetActive(false);
        endScreen.SetActive(true);
        instancedFinish.SetActive(false);

        if (collectedDiamond >= (SavedDataHandler.Instance.level * 10))
        {
            FindObjectOfType<CharacterBehaviour>().Dance();
            wonButton.SetActive(true);
            lostButton.SetActive(false);
            endTitle.text = "WON WITH " + collectedDiamond + " DIAMONDS";
        }
        else
        {
            endTitle.text = collectedDiamond + " collected but " + SavedDataHandler.Instance.level * 10 + " needed";
            wonButton.SetActive(false);
            lostButton.SetActive(true);
        }
    }
    public void Upgrade()
    {
        if (SavedDataHandler.Instance.currency < 1) return;
        SavedDataHandler.Instance.currency--;
        SavedDataHandler.Instance.stackUpgrade++;
        upgradeButton.text = string.Format("Currency {0} | Upgrade {1}", SavedDataHandler.Instance.currency, SavedDataHandler.Instance.stackUpgrade);
        SavedDataHandler.Instance.SaveScore();
    }
    public void ResetProgress()
    {
        SavedDataHandler.Instance.ResetScore();
    }
    public void Replay()
    {
        SavedDataHandler.Instance.SaveScore();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void NextLevel()
    {
        SavedDataHandler.Instance.level++;
        SavedDataHandler.Instance.SaveScore();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void GenerateLevel(int level)
    {
        //if (level < 5) return;


        //generate platforms
        for (int i = 0; i < level * 12; i++)
        {
            Instantiate(platform, new Vector3(0, 0, i * 10), Quaternion.identity, levelHierarchy);
        }

        int levelSegments = level * 12;

        instancedFinish = Instantiate(finish, new Vector3(0, 2, levelSegments * 10), Quaternion.identity, levelHierarchy);

        levelSegments--;

        //generate diamonds
        int diamonds = 10 + (20 * level);
        for (int i = 1; i < diamonds + 1; i++)
        {
            Instantiate(diamond, new Vector3(Random.Range(-2f, 2f), 1,(i / ((float)diamonds / (float)levelSegments)) * 10 + Random.Range(-5f, 5f)), Quaternion.identity, levelHierarchy);
        }

        //generate golds
        int golds = 10;
        for (int i = 1; i < golds + 1; i++)
        {
            Instantiate(gold, new Vector3(Random.Range(-2f, 2f), 1, (i / ((float)golds / (float)levelSegments)) * 10 + Random.Range(-3f, 3f)), Quaternion.identity, levelHierarchy);
        }

        //generate obstacles
        int obstacles = level * 6;
        for (int i = 1; i < obstacles + 1; i++)
        {
            Instantiate(obstacle, new Vector3(Random.Range(-2f, 2f), 1, (i / ((float)obstacles / (float)levelSegments)) * 10 + Random.Range(-4f, 4f)), Quaternion.identity, levelHierarchy);
        }
    }
    public void CollectCurrency(Vector3 position)
    {
        SavedDataHandler.Instance.currency++;
        currencyNumber.text = SavedDataHandler.Instance.currency.ToString();
        SavedDataHandler.Instance.SaveScore();
    }
    public void StackAdded(float value)
    {
        stackFill.fillAmount = value;
    }
}
