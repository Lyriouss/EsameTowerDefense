using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Transform turretSpawns;
    private int turretsUntilDisable;

    [Header("UI Panels")]
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOver;

    [Header("In Game UI Elements")]
    [SerializeField] private Image healthFill;
    [SerializeField] private TMP_Text moneyTxt;
    [SerializeField] private TMP_Text timerTxt;
    [SerializeField] private TMP_Text killTallyTxt;
    [SerializeField] private Button normalTurretBtn;
    [SerializeField] private Button machineGunTurretBtn;
    [SerializeField] private Button areaTurretBtn;
    [SerializeField] private Button sniperTurretBtn;
    [SerializeField] private TMP_Text selectedTurretTxt;

    [Header("Game Over Elements")]
    [SerializeField] private TMP_Text timeStamp;
    [SerializeField] private TMP_Text killTally;

    private float timer;
    private float minutes;
    private float seconds;

    private bool maxTurrets;

    //Runs said function when invoked, primarily from GameManager
    private void OnEnable()
    {
        GameManager.OnTurretSelected += UpdateTurretTxt;
        GameManager.OnMoneyChanged += UpdateMoney;
        GameManager.OnKillCountChanged += UpdateKillTally;
        GameManager.OnTurretCountChanged += CheckTurretCount;
        GameManager.OnDamageRecieved += UpdateHealth;
        GameManager.OnGameOver += GameOver;
        GameManager.OnGamePaused += PauseGame;
    }

    private void OnDisable()
    {
        GameManager.OnTurretSelected -= UpdateTurretTxt;
        GameManager.OnMoneyChanged -= UpdateMoney;
        GameManager.OnKillCountChanged -= UpdateKillTally;
        GameManager.OnTurretCountChanged -= CheckTurretCount;
        GameManager.OnDamageRecieved -= UpdateHealth;
        GameManager.OnGameOver -= GameOver;
        GameManager.OnGamePaused -= PauseGame;
    }

    private void Start()
    {
        //Gets the amount of transforms in TurretSpawn children
        foreach (Transform child in turretSpawns)
        {
            turretsUntilDisable++;
        }

        //Sets the timer to 0
        timer = 0f;
        minutes = 0f;
        seconds = 0f;

        //Updates the UI of money and health
        UpdateMoney(GameManager.Instance.startingMoney);
        UpdateHealth(1f);

        //When scene is loaded, only inGameUI will be active
        inGameUI.SetActive(true);
        pauseMenu.SetActive(false);
        gameOver.SetActive(false);

        //No text in selected turret text
        selectedTurretTxt.text = null;
    }

    //Only timer will run in Update
    private void Update()
    {
        Timer();
    }

    private void Timer()
    {
        //+1f every second
        timer += Time.deltaTime;

        //Convert seconds into minutes (every 60 in a minute rounded by defect
        minutes = Mathf.Floor(timer / 60);

        //Returns the rest of the timer / 60 rounded to the nearest integer
        seconds = Mathf.FloorToInt(timer % 60);

        //Get a model string and its values withing the brackets are placeholders for elements inserted later
        timerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void UpdateTurretTxt()
    {
        //Updates selectedTurretTxt relative to TurretSelected enum in GameManager when interacted with in UI
        switch (GameManager.Instance.turretSelected)
        {
            case TurretSelected.NormalTurret:
                selectedTurretTxt.text = "Selected: Normal Turret";
                break;
            case TurretSelected.MachineGunTurret:
                selectedTurretTxt.text = "Selected: Machine Gun Turret";
                break;
            case TurretSelected.AreaTurret:
                selectedTurretTxt.text = "Selected: Area Turret";
                break;
            case TurretSelected.SniperTurret:
                selectedTurretTxt.text = "Selected: Sniper Turret";
                break;
            case TurretSelected.DeleteTurret:
                selectedTurretTxt.text = "Selected: Delete Turret";
                break;
            case TurretSelected.NoSelect:
                selectedTurretTxt.text = null;
                break;
        }
    }

    private void UpdateMoney(int currentMoney)
    {
        //Updates money in UI and checks whether a Turret can be placed or not
        moneyTxt.text = "€ " + currentMoney.ToString();
        TurretPlacementCheck();
    }

    private void CheckTurretCount(int turretsPlaced)
    {
        //When turrets placed reaches the total turret spawn positions
        if (turretsPlaced >= turretsUntilDisable)
        {
            //Deactivate all turret button interacts
            normalTurretBtn.interactable = false;
            machineGunTurretBtn.interactable = false;
            areaTurretBtn.interactable = false;
            sniperTurretBtn.interactable = false;
            //And sets turret select to no selection
            GameManager.Instance.turretSelected = TurretSelected.NoSelect;
            UpdateTurretTxt();

            maxTurrets = true;
        }
        //Else doesn't deactivate buttons and checks what can be placed
        else
        {
            maxTurrets = false;
            TurretPlacementCheck();
        }
    }

    //If a turret cannot be placed because there is not enough money or there aren't any placement spaces
    //deactivates turret button interact
    private void TurretPlacementCheck()
    {
        if (GameManager.Instance.currentMoney < 5 || maxTurrets)
            normalTurretBtn.interactable = false;
        else
            normalTurretBtn.interactable = true;

        if (GameManager.Instance.currentMoney < 10 || maxTurrets)
            machineGunTurretBtn.interactable = false;
        else
            machineGunTurretBtn.interactable = true;

        if (GameManager.Instance.currentMoney < 15 || maxTurrets)
            areaTurretBtn.interactable = false;
        else
            areaTurretBtn.interactable = true;

        if (GameManager.Instance.currentMoney < 20 || maxTurrets)
            sniperTurretBtn.interactable = false;
        else
            sniperTurretBtn.interactable = true;
    }

    private void UpdateKillTally(int killCount)
    {
        //Updates killTally in UI
        killTallyTxt.text = "Kills: " + killCount.ToString();
    }

    private void UpdateHealth(float fillValue)
    {
        //Updates health bar amount
        healthFill.fillAmount = fillValue;
    }

    private void PauseGame()
    {
        //Activates pause menu when paused
        if (GameManager.Instance.status == GameStatus.GamePaused)
        {
            pauseMenu.SetActive(true);
        }
        //Deactivates pause menu when running
        else if (GameManager.Instance.status == GameStatus.GameRunning)
        {
            pauseMenu.SetActive(false);
        }
    }

    private void GameOver()
    {
        //Deactivates inGameUI and activates game over menu
        inGameUI.SetActive(false);
        gameOver.SetActive(true);

        //Shows how long game lasted and how many enemies were killed
        timeStamp.text = timerTxt.text;
        killTally.text = "You Kept out: " + GameManager.Instance.killCount.ToString() + " Foggiani";
    }
}