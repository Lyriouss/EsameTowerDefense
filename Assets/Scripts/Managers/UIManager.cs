using TMPro;
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
    [SerializeField] private TMP_Text selectedTurretTxt;

    [Header("Game Over Elements")]
    [SerializeField] private TMP_Text timeStamp;
    [SerializeField] private TMP_Text killTally;

    private float timer;
    private float minutes;
    private float seconds;

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
        foreach (Transform child in turretSpawns)
        {
            turretsUntilDisable++;
        }

        timer = 0f;
        minutes = 0f;
        seconds = 0f;

        UpdateMoney(GameManager.Instance.startingMoney);
        UpdateHealth(1f);

        inGameUI.SetActive(true);
        pauseMenu.SetActive(false);
        gameOver.SetActive(false);

        selectedTurretTxt.text = null;
    }

    private void Update()
    {
        Timer();
    }

    private void Timer()
    {
        timer += Time.deltaTime;

        //convert seconds into minutes (every 60 in a minute rounded by defect
        minutes = Mathf.Floor(timer / 60);

        //returns the rest of the timer / 60 rounded to the nearest integer
        seconds = Mathf.FloorToInt(timer % 60);

        //get a model string and its values withing the brackets are placeholders for elements inserted later
        timerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void UpdateTurretTxt()
    {
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
            case TurretSelected.NoSelect:
                selectedTurretTxt.text = null;
                break;
        }
    }

    private void UpdateMoney(int currentMoney)
    {
        moneyTxt.text = "€ " + currentMoney.ToString();

        TurretPlacementCheck();
    }

    private void CheckTurretCount(int turretsPlaced)
    {
        if (turretsPlaced >= turretsUntilDisable)
        {
            normalTurretBtn.interactable = false;
            machineGunTurretBtn.interactable = false;
            areaTurretBtn.interactable = false;
            selectedTurretTxt.text = null;
        }
    }

    private void UpdateKillTally(int killCount)
    {
        killTallyTxt.text = "Kills: " + killCount.ToString();
    }

    private void UpdateHealth(float fillValue)
    {
        healthFill.fillAmount = fillValue;
    }

    private void TurretPlacementCheck()
    {
        if (GameManager.Instance.currentMoney < 5)
            normalTurretBtn.interactable = false;
        else
            normalTurretBtn.interactable = true;

        if (GameManager.Instance.currentMoney < 10)
            machineGunTurretBtn.interactable = false;
        else
            machineGunTurretBtn.interactable = true;

        if (GameManager.Instance.currentMoney < 15)
            areaTurretBtn.interactable = false;
        else
            areaTurretBtn.interactable = true;
    }

    private void PauseGame()
    {
        if (GameManager.Instance.status == GameStatus.GameRunning)
        {
            pauseMenu.SetActive(true);
        }
        else if (GameManager.Instance.status == GameStatus.GamePaused)
        {
            pauseMenu.SetActive(false);
        }
    }

    private void GameOver()
    {
        inGameUI.SetActive(false);
        gameOver.SetActive(true);

        timeStamp.text = timerTxt.text;
        killTally.text = "You Kept out: " + GameManager.Instance.killCount.ToString() + " Foggiani";
    }
}