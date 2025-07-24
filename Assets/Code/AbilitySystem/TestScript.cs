using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

// ==================== ScriptableObjects ====================
[CreateAssetMenu(menuName = "Game/AbilityUpgrade")]
public class AbilityUpgrade : ScriptableObject
{
    public string Name;
    [TextArea] public string Description;
    public Sprite Icon;
    public int MaxLevel = 5;
    public List<float> UpgradeValues;
    public AbilityType1 AbilityType;
}

public enum AbilityType1
{
    SwordStrike,
    Fireball,
    Dash
}

[CreateAssetMenu(menuName = "Game/AbilityConfig")]
public class AbilityConfig1 : ScriptableObject
{
    public AbilityType1 Type;
    public float BaseDamage;
    public float BaseRadius;
    public float BaseCooldown;
    public List<AbilityUpgrade> PossibleUpgrades;

    [HideInInspector] public int CurrentUpgradeLevel;
    [HideInInspector] public float CurrentDamage;
    [HideInInspector] public float CurrentRadius;
    [HideInInspector] public float CurrentCooldown;

    public void Initialize()
    {
        CurrentUpgradeLevel = 0;
        CurrentDamage = BaseDamage;
        CurrentRadius = BaseRadius;
        CurrentCooldown = BaseCooldown;
    }

    public void ApplyUpgrade(AbilityUpgrade upgrade)
    {
        CurrentUpgradeLevel++;

        switch (upgrade.AbilityType)
        {
            case AbilityType1.SwordStrike:
                CurrentDamage += upgrade.UpgradeValues[0];
                CurrentRadius += upgrade.UpgradeValues[1];
                break;
                // Добавь другие типы способностей по аналогии
        }
    }
}

// ==================== Systems ====================
public class LevelSystem : MonoBehaviour
{
    [SerializeField] private int _currentLevel = 1;
    [SerializeField] private int _currentExp;
    [SerializeField] private int _expToNextLevel = 100;

    private AbilityManager _abilityManager;
    private PauseManager _pauseManager;
    private UpgradePanel1 _upgradePanel;

    public event Action<int> OnLevelUp;

    public void Initialize(AbilityManager abilityManager, PauseManager pauseManager, UpgradePanel1 upgradePanel)
    {
        _abilityManager = abilityManager;
        _pauseManager = pauseManager;
        _upgradePanel = upgradePanel;
    }

    public void AddExperience(int amount)
    {
        _currentExp += amount;
        if (_currentExp >= _expToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        _currentLevel++;
        _currentExp = 0;
        _expToNextLevel = CalculateExpForNextLevel(_currentLevel);

        _pauseManager.PauseGame();
        var upgrades = _abilityManager.GetRandomUpgrades(3);
        _upgradePanel.Show(upgrades);

        OnLevelUp?.Invoke(_currentLevel);
    }

    private int CalculateExpForNextLevel(int level)
    {
        return 100 + (level * 50);
    }
}

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private List<AbilityConfig1> _allAbilities;

    public void Initialize()
    {
        foreach (var ability in _allAbilities)
        {
            ability.Initialize();
        }
    }

    public List<AbilityUpgrade> GetRandomUpgrades(int count)
    {
        var availableUpgrades = new List<AbilityUpgrade>();

        foreach (var ability in _allAbilities)
        {
            if (ability.CurrentUpgradeLevel < ability.PossibleUpgrades.Count)
            {
                availableUpgrades.AddRange(ability.PossibleUpgrades
                    .Where(u => u.MaxLevel > ability.CurrentUpgradeLevel));
            }
        }

        return availableUpgrades
            .OrderBy(x => Random.value)
            .Take(count)
            .ToList();
    }

    public void ApplyUpgrade(AbilityUpgrade upgrade)
    {
        AbilityConfig1 ability = _allAbilities.First(a => a.Type == upgrade.AbilityType);
        ability.ApplyUpgrade(upgrade);

        // Обновляем все активные экземпляры этой способности
        var abilityInstances = FindObjectsOfType<MonoBehaviour>().OfType<IAbility>();
        foreach (var instance in abilityInstances.Where(a => a.GetAbilityType() == upgrade.AbilityType))
        {
            instance.UpdateStats(ability);
        }
    }
}

public interface IAbility
{
    AbilityType1 GetAbilityType();
    void UpdateStats(AbilityConfig1 config);
}

// ==================== UI ====================
public class UpgradePanel1 : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button[] _upgradeButtons;
    [SerializeField] private Image[] _upgradeIcons;
    [SerializeField] private Text[] _upgradeNames;
    [SerializeField] private Text[] _upgradeDescriptions;

    private AbilityManager _abilityManager;
    private PauseManager _pauseManager;
    private List<AbilityUpgrade> _currentUpgrades;

    public void Initialize(AbilityManager abilityManager, PauseManager pauseManager)
    {
        _abilityManager = abilityManager;
        _pauseManager = pauseManager;

        for (int i = 0; i < _upgradeButtons.Length; i++)
        {
            int index = i;
            _upgradeButtons[i].onClick.AddListener(() => OnUpgradeSelected(index));
        }
    }

    public void Show(List<AbilityUpgrade> upgrades)
    {
        _currentUpgrades = upgrades;

        for (int i = 0; i < upgrades.Count; i++)
        {
            _upgradeIcons[i].sprite = upgrades[i].Icon;
            _upgradeNames[i].text = upgrades[i].Name;
            _upgradeDescriptions[i].text = upgrades[i].Description;
            _upgradeButtons[i].gameObject.SetActive(true);
        }

        _panel.SetActive(true);
    }

    private void OnUpgradeSelected(int index)
    {
        _abilityManager.ApplyUpgrade(_currentUpgrades[index]);
        _pauseManager.ResumeGame();
        _panel.SetActive(false);
    }
}

// ==================== Utility ====================
public class PauseManager : MonoBehaviour
{
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
}

// ==================== Ability Implementation ====================
public class SwordStrike : MonoBehaviour, IAbility
{
    [SerializeField] private AbilityConfig1 _config;
    private float _cooldownTimer;

    public AbilityType1 GetAbilityType() => AbilityType1.SwordStrike;

    private void Start()
    {
        _cooldownTimer = _config.CurrentCooldown;
    }

    private void Update()
    {
        _cooldownTimer -= Time.deltaTime;
        if (_cooldownTimer <= 0)
        {
            Attack();
            _cooldownTimer = _config.CurrentCooldown;
        }
    }

    private void Attack()
    {
        // Реализация атаки с текущими параметрами
        Collider[] hits = Physics.OverlapSphere(transform.position, _config.CurrentRadius);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<Health>(out var health))
            {
                health.TakeDamage(_config.CurrentDamage);
            }
        }
    }

    public void UpdateStats(AbilityConfig1 config)
    {
        _config = config;
    }
}

// ==================== Setup in GameController ====================
public class GameController : MonoBehaviour
{
    [SerializeField] private AbilityManager _abilityManager;
    [SerializeField] private PauseManager _pauseManager;
    [SerializeField] private UpgradePanel1 _upgradePanel;
    [SerializeField] private LevelSystem _levelSystem;

    private void Start()
    {
        _abilityManager.Initialize();
        _upgradePanel.Initialize(_abilityManager, _pauseManager);
        _levelSystem.Initialize(_abilityManager, _pauseManager, _upgradePanel);
    }
}