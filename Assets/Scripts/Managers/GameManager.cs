using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStatus
{
    GamePaused,
    GameRunning
}

public enum TurretSelected
{
    NoSelect,
    NormalTurret,
    MachineGunTurret,
    AreaTurret
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameStatus status;
    public TurretSelected turretSelected;

    //Event Actions for running functions, primarily in UIManager
    public static event Action OnTurretSelected;
    public static event Action<int> OnMoneyChanged;
    public static event Action<int> OnKillCountChanged;
    public static event Action<int> OnTurretCountChanged;
    public static event Action<float> OnDamageRecieved;
    public static event Action OnGamePaused;
    public static event Action OnGameOver;

    //Money and player health variables (+ amount of turrets on level)
    public int startingMoney;
    [HideInInspector] public int currentMoney;
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [HideInInspector] public int killCount;
    private int turretsPlaced;

    //Runs said function when invoked from other classes
    private void OnEnable()
    {
        Turret.OnStart += UpdateMoney;
        Turret.OnTurretPlacement += UpdateTurretCount;
        Turret.onUpgradeTurret += UpdateMoney;
        Enemy.OnEnemyKilled += UpdateMoney;
        Enemy.OnKillAdded += UpdateKillCount;
        Enemy.OnBaseReached += DamageToBase;
        InputManager.OnGamePaused += PauseGame;
    }

    private void OnDisable()
    {
        Turret.OnStart -= UpdateMoney;
        Turret.OnTurretPlacement -= UpdateTurretCount;
        Turret.onUpgradeTurret -= UpdateMoney;
        Enemy.OnEnemyKilled -= UpdateMoney;
        Enemy.OnKillAdded -= UpdateKillCount;
        Enemy.OnBaseReached -= DamageToBase;
        InputManager.OnGamePaused -= PauseGame;
    }

    //Singleton initialization
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        //Sets enums
        status = GameStatus.GameRunning;
        turretSelected = TurretSelected.NoSelect;

        //Sets money and player health
        currentHealth = maxHealth;
        currentMoney = startingMoney;
    }

    private void Update()
    {
        //Based on GameStatus enum, will run or freeze the game
        if (status == GameStatus.GameRunning)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
    }

    //Functions when selecting turret option from UI Buttons
    #region
    public void NormalSelect()
    {
        turretSelected = TurretSelected.NormalTurret;
        OnTurretSelected?.Invoke();
    }

    public void MachineGunSelect()
    {
        turretSelected = TurretSelected.MachineGunTurret;
        OnTurretSelected?.Invoke();
    }

    public void AreaSelect()
    {
        turretSelected = TurretSelected.AreaTurret;
        OnTurretSelected?.Invoke();
    }

    public void Unselect()
    {
        turretSelected = TurretSelected.NoSelect;
        OnTurretSelected?.Invoke();
    }
    #endregion

    //Changes value of variable and calls function in UIManager to change text
    #region
    //Changes current money
    public void UpdateMoney(int enemyMoney)
    {
        currentMoney += enemyMoney;
        OnMoneyChanged?.Invoke(currentMoney);
    }

    //Changes kill count
    public void UpdateKillCount()
    {
        killCount++;
        OnKillCountChanged?.Invoke(killCount);
    }

    //Changes turrets placed
    public void UpdateTurretCount()
    {
        turretsPlaced++;
        OnTurretCountChanged?.Invoke(turretsPlaced);
    }
    #endregion

    //Deducts health when enemy reaches player base
    private void DamageToBase(float enemyDamage)
    {
        currentHealth -= enemyDamage;

        //When health reaches or goes under 0
        if (currentHealth <= 0)
        {
            //Runs game over function and paused the game
            OnGameOver?.Invoke();
            status = GameStatus.GamePaused;
            return;
        }

        //Calculates fillValue of health bar before running function
        float fillValue = currentHealth / maxHealth;
        //Calls function in UIManager to change health bar and passes fillValue as a reference
        OnDamageRecieved?.Invoke(fillValue);
    }

    public void PauseGame()
    {
        //Pause or Resume game (based on current enum value) and call function for pause menu UIManager
        if (status == GameStatus.GameRunning)
        {
            status = GameStatus.GamePaused;
            OnGamePaused?.Invoke();
        }
        else if (status == GameStatus.GamePaused)
        {
            status = GameStatus.GameRunning;
            OnGamePaused?.Invoke();
        }
    }

    public void ReloadScene()
    {
        //Reloads current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        //Loads MainMenu scene
        SceneManager.LoadScene(0);
    }
}
