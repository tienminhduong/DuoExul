using TriInspector;
using UnityEngine;

[RequireComponent(typeof(IEntity))]
public class AttackCommandInvoker : MonoBehaviour
{
    [SerializeField] private CommandData comboAttackData;
    [SerializeField] private float PauseMaxDurationInSeconds = 2f;
    [SerializeField] private AttackHitbox attackHitbox;
    private IEntity entity;

    [ReadOnly][SerializeField] private bool isExecuting;
    [ReadOnly][SerializeField] private bool isPaused;
    [ReadOnly][SerializeField] private float pauseTimer;

    [ReadOnly][SerializeField] private int currentExecutingCommandIndex;

    void Awake()
    {
        entity = GetComponent<IEntity>();
    }

    public void PauseExecution()
    {
        if (!isExecuting || isPaused)
            return;
        isPaused = true;
    }

    void OnEnable()
    {
        if (attackHitbox != null)
            attackHitbox.OnHit += ApplyAttackEffects;
        else
            Debug.LogWarning("AttackHitbox reference is missing in AttackCommandInvoker.");
    }

    void OnDisable()
    {
        if (attackHitbox != null)
            attackHitbox.OnHit -= ApplyAttackEffects;
        else
            Debug.LogWarning("AttackHitbox reference is missing in AttackCommandInvoker.");
    }

    public void ResumeExecution()
    {
        if (!isExecuting || !isPaused)
            return;
        isPaused = false;
    }

    public async Awaitable ExecuteCommandsAsync()
    {
        if (isExecuting)
        {
            if (isPaused)
                ResumeExecution();
            else
                Debug.LogWarning("Already executing commands. Call PauseExecution() before starting again.");
        }
        else
            await StartCommandsAsync();
    }

    public async Awaitable StartCommandsAsync()
    {
        isExecuting = true;
        while (isExecuting)
        {
            if (isPaused)
            {
                pauseTimer += Time.deltaTime;
                if (pauseTimer >= PauseMaxDurationInSeconds)
                {
                    StopExecution();
                    break;
                }
                await Awaitable.NextFrameAsync();
                continue;
            }

            if (currentExecutingCommandIndex >= comboAttackData.commands.Count)
            {
                if (comboAttackData.isLooping)
                    currentExecutingCommandIndex = 0;
                else
                {
                    StopExecution();
                    break;
                }
            }

            var command = comboAttackData.commands[currentExecutingCommandIndex];
            pauseTimer = 0f;
            Debug.Log($"Executing command {currentExecutingCommandIndex} of {comboAttackData.commands.Count}");
            await command.Execute(entity);
            currentExecutingCommandIndex++;
        }
    }

    public void StopExecution()
    {
        isExecuting = false;
        currentExecutingCommandIndex = 0;
        pauseTimer = 0f;
        isPaused = false;
    }

    private void ApplyAttackEffects(IEntity target)
    {
        if (currentExecutingCommandIndex < 0 || currentExecutingCommandIndex >= comboAttackData.commands.Count)
            return;
        if (comboAttackData.commands[currentExecutingCommandIndex] is PlayerAttackCommand attackData)
            AbilityInvoker.ApplyEffect(attackData.attackData.uniqueAbilities, entity, target);
    }

    public bool SetComboAttackData(CommandData newData)
    {
        if (comboAttackData.label == newData.label)
            return true;
        if (isExecuting && !isPaused)
        {
            Debug.LogWarning("Cannot change combo attack data while executing. Please pause execution first.");
            return false;
        }

        StopExecution();

        comboAttackData = newData;
        return true;
    }
}