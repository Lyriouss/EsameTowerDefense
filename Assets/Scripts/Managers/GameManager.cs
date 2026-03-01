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

    //[SerializeField] GameObject turretSpawns;

    public static event Action OnTurretSelected;
    public static event Action<int> OnMoneyChanged;
    public static event Action<int> OnKillCountChanged;
    public static event Action<int> OnTurretCountChanged;
    public static event Action<float> OnDamageRecieved;
    public static event Action OnGamePaused;
    public static event Action OnGameOver;

    public int startingMoney;
    [HideInInspector] public int currentMoney;
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [HideInInspector] public int killCount;
    private int turretsPlaced;


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
        status = GameStatus.GameRunning;
        turretSelected = TurretSelected.NoSelect;

        currentHealth = maxHealth;
        currentMoney = startingMoney;
    }

    private void Update()
    {
        if (status == GameStatus.GameRunning)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
    }

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

    //public void DeleteSelect()
    //{
    //    turretSelected = TurretSelected.DeleteTurret;
        

    //}

    public void UpdateMoney(int enemyMoney)
    {
        currentMoney += enemyMoney;
        OnMoneyChanged?.Invoke(currentMoney);
    }

    public void UpdateTurretCount()
    {
        turretsPlaced++;
        OnTurretCountChanged?.Invoke(turretsPlaced);
    }

    public void UpdateKillCount()
    {
        killCount++;
        OnKillCountChanged?.Invoke(killCount);
    }

    private void DamageToBase(float enemyDamage)
    {
        currentHealth -= enemyDamage;

        if (currentHealth <= 0)
        {
            OnGameOver?.Invoke();
            status = GameStatus.GamePaused;
            return;
        }

        float fillValue = currentHealth / maxHealth;
        OnDamageRecieved?.Invoke(fillValue);
    }

    public void PauseGame()
    {
        if (status == GameStatus.GameRunning)
        {
            OnGamePaused?.Invoke();
            status = GameStatus.GamePaused;
        }
        else if (status == GameStatus.GamePaused)
        {
            OnGamePaused?.Invoke();
            status = GameStatus.GameRunning;
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
