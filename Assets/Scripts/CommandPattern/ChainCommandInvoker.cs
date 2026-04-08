using TriInspector;
using UnityEngine;

[RequireComponent(typeof(IEntity))]
public class ChainCommandInvoker : MonoBehaviour
{
    [SerializeField] private CommandData commandData;
    [SerializeField] private float PauseMaxDurationInSeconds = 2f;
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

            if (currentExecutingCommandIndex >= commandData.commands.Count)
            {
                // StopExecution();
                // break;
                currentExecutingCommandIndex = 0;
            }

            var command = commandData.commands[currentExecutingCommandIndex];
            pauseTimer = 0f;
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
}