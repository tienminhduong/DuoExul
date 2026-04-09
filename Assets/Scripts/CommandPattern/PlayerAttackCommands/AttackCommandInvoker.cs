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

    private int currentExecutingCommandIndex;

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
                // StopExecution();
                // break;
                currentExecutingCommandIndex = 0;
            }

            var command = comboAttackData.commands[currentExecutingCommandIndex];
            pauseTimer = 0f;
            // if (command is PlayerAttackCommand playerAttackCommand)
            // {
            //     AbilityInvoker.ApplyEffect(playerAttackCommand.attackData.uniqueAbilities, entity, null);
            // }
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
        if (comboAttackData.commands[currentExecutingCommandIndex] is PlayerAttackCommand attackData)
            AbilityInvoker.ApplyEffect(attackData.attackData.uniqueAbilities, entity, target);
    }
}