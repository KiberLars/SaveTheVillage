using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TimerScript HarvestTimer;
    public TimerScript EatingTimer;
    public Image RaidTimer;
    public Image WarriorTimer;
    public Image PeasantTimer;
    public Button PeasantButton;
    public Button WarriorButton;
    public TextMeshProUGUI WorkerText;
    public TextMeshProUGUI KillersText;
    public TextMeshProUGUI HarvestText;
    public TextMeshProUGUI SaveWaveText;
    public TextMeshProUGUI WavesText;
    public TextMeshProUGUI allHarvestText;
    public TextMeshProUGUI KillersNextWaveText;

    // count
    public int peasantCount;
    public int warriorCount;
    public int harvestCount;

    // per circle
    public int harvestPerCircle;
    public int harvestToWarriors;

    // price
    public int peasantCost;
    public int warriorCost;

    // raid
    public int raidMaxTimer;
    public int raidIncrease;
    public int nextRaidCount;
    public int waveToIncreaseWarriors;
    public int saveWaves;

    private float peasantTimer = -2;
    private float warriorTimer = -2;
    private float raidTimer = -2;

    private float peasantCreateTime = 2;
    private float warriorCreateTime = 3;

    // statistic
    private int allWaves = 0;
    private int allHarvest = 0;
    private int _waveToIncreaseWarriors;

    // finish panels
    public GameObject PausePanel;
    public GameObject ResultPanel;
    public GameObject WinPanel;
    public GameObject ResultPanelBackGround;
    public TextMeshProUGUI ResultPanelHarvest;
    public TextMeshProUGUI ResultPanelWaves;
    public int HarvestToWin;
    public int WavesToWin;
    public TextMeshProUGUI HarvestToWinText;
    public TextMeshProUGUI WavesToWinText;

    public AudioSource audio;
    public AudioClip clip;

    //GameAudioSource.PlayOneShot(clickClip);

    void Start()
    {
        Time.timeScale = 1;
        WavesToWinText.text = "- Пережить " + WavesToWin + " волн";
        HarvestToWinText.text = "- Собрать " + HarvestToWin + " урожая";
        UpdateText();
        UpdateStatText();
        raidTimer = raidMaxTimer;
        _waveToIncreaseWarriors = waveToIncreaseWarriors;
    }

    void Update()
    {
        TimerHandler();
    }

    private void TimerHandler()
    {
        raidTimer -= Time.deltaTime;
        RaidTimer.fillAmount = raidTimer / raidMaxTimer;
        if (raidTimer <= 0)
        {
            raidTimer = raidMaxTimer;
            if (saveWaves > 0)
                saveWaves--;
            else
            {
                audio.PlayOneShot(clip);
                warriorCount -= nextRaidCount;
            }

            if (_waveToIncreaseWarriors > 0)
                _waveToIncreaseWarriors--;
            else
            {
                _waveToIncreaseWarriors = waveToIncreaseWarriors;
                nextRaidCount += raidIncrease;
            }
            allWaves++;
            UpdateStatText();
        }

        if (HarvestTimer.Tick)
        {
            harvestCount += peasantCount * harvestPerCircle;
            allHarvest += peasantCount * harvestPerCircle;
        }

        if (EatingTimer.Tick)
        {
            harvestCount -= warriorCount * warriorCost;
        }

        if (peasantTimer > 0)
        {
            peasantTimer -= Time.deltaTime;
            PeasantTimer.fillAmount = peasantTimer / peasantCreateTime;
        }
        else if (peasantTimer <= 0)
        {
            PeasantButton.interactable = true;
            PeasantTimer.fillAmount = 1;
        }

        if (warriorTimer > 0)
        {
            warriorTimer -= Time.deltaTime;
            WarriorTimer.fillAmount = warriorTimer / warriorCreateTime;
        }
        else if (warriorTimer <= 0)
        {
            WarriorButton.interactable = true;
            WarriorTimer.fillAmount = 1;
        }

        UpdateText();
        CheckFinishGame();
    }

    private void CheckFinishGame()
    {
        if (harvestCount < 0 || warriorCount < 0)
        {
            Time.timeScale = 0; 
            ResultPanelHarvest.text = "Собрано урожая: " + allHarvest;
            ResultPanelWaves.text = "Пройдено волн: " + allWaves;
            ResultPanel.SetActive(true);
            ResultPanelBackGround.SetActive(true);
        }
        if (allWaves > WavesToWin || allHarvest > HarvestToWin)
        {
            Time.timeScale = 0;
            WinPanel.SetActive(true);
            ResultPanelBackGround.SetActive(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ContinueGame()
    {
        ResultPanelBackGround.SetActive(false);
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        ResultPanelBackGround.SetActive(true);
        PausePanel.SetActive(true);
    }

    public void CreatePeasant()
    {
        if (harvestCount - peasantCost >= 0 && peasantTimer <= 0)
        {
            peasantCount += 1;
            harvestCount -= peasantCost;
            peasantTimer = peasantCreateTime;
            PeasantButton.interactable = false;
            UpdateText();
        }
    }

    public void CreateWarrior()
    {
        if (harvestCount - warriorCost >= 0)
        {
            warriorCount += 1;
            harvestCount -= warriorCost;
            warriorTimer = warriorCreateTime;
            WarriorButton.interactable = false;

            UpdateText();
        }
    }

    public void UpdateText()
    {
        WorkerText.text = "Крестьян: " + peasantCount;
        KillersText.text = "Воинов: " + warriorCount;
        int harvestStat = warriorCount * warriorCost;
        HarvestText.text = "Пшеницы: " + harvestCount + "(-" + harvestStat + ")";
        allHarvestText.text = "Собрано урожая: " + allHarvest;
        if (harvestCount >= peasantCost && peasantTimer <= 0)
            PeasantButton.interactable = true;
        else
            PeasantButton.interactable = false;

        if (harvestCount >= warriorCost && warriorTimer <= 0)
            WarriorButton.interactable = true;
        else
            WarriorButton.interactable = false;
    }

    public void UpdateStatText()
    {
        SaveWaveText.text = "Волн перемирия: " + saveWaves;
        WavesText.text = "Волн:" + allWaves;
        KillersNextWaveText.text = "Врагов: " + nextRaidCount;
    }
}
