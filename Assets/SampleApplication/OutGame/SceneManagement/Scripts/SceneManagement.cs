using System;
using System.Threading.Tasks;
using Doinject;
using Mew.Core.Assets;
using Mew.Core.TaskHelpers;
using Mew.Core.Tasks;
using UnityEngine;

public class SceneManagement : MonoBehaviour, IInjectableComponent
{
    [SerializeField] private UnifiedScene stageSelectScene = default;
    [SerializeField] private UnifiedScene titleScene = default;
    [SerializeField] private UnifiedScene inGameScene = default;

    private IContext Context { get; set; }
    private BlackoutCurtain BlackoutCurtain { get; set; }

    [Inject]
    // ReSharper disable once UnusedMember.Global
    public void Construct(
        IContext context,
        [Optional] BlackoutCurtain blackoutCurtain)
    {
        TaskQueue.DisposeWith(destroyCancellationToken);
        Context = context;
        BlackoutCurtain = blackoutCurtain;
    }

    [OnInjected]
    // ReSharper disable once UnusedMember.Global
    public async Task OnInjected()
    {
        if (Context.IsReverseLoaded)
        {
            if (BlackoutCurtain) await BlackoutCurtain.FadeIn();
            return;
        }

        // Load initial scene
        await LoadTitle();
    }

    public async Task LoadTitle()
        => await Transit(titleScene);

    public async Task LoadStageSelect()
        => await Transit(stageSelectScene);
    public async Task LoadInGame(InGameArg inGameArg)
        => await Transit(inGameScene, inGameArg);

    private TaskQueue TaskQueue { get; } = new(TaskQueueLimitType.SwapLast, maxSize: 2);

    // Load main scene by using SceneContextLoader.
    // scene will be created as a child context of Application context.
    private async Task Transit(UnifiedScene to, IContextArg contextArg = null)
    {
        await TaskQueue.EnqueueAsync(async ct =>
        {
            try
            {
                // start loading
                BlackoutCurtain.SetProgression(0f);
                if (BlackoutCurtain) await BlackoutCurtain.FadeOut();

                // show progress bar
                BlackoutCurtain.ShowProgression(true);

                // unload scenes
                await Context.SceneContextLoader.UnloadAllScenesAsync();
                BlackoutCurtain.SetProgression(0.1f);

                // start loading new scene
                var contextLoader = Context.SceneContextLoader;
                var loadTask = contextLoader.LoadAsync(to, active: true, contextArg);

                // update loading progress bar
                while (!destroyCancellationToken.IsCancellationRequested &&
                       !loadTask.IsCompleted)
                {
                    BlackoutCurtain.SetProgression(0.1f + 0.9f * contextLoader.Progression);
                    await TaskHelper.NextFrame(destroyCancellationToken);
                }

                // loading finished
                BlackoutCurtain.SetProgression(1f);
                if (BlackoutCurtain) await BlackoutCurtain.FadeIn();

                // handle cancellation while loading
                destroyCancellationToken.ThrowIfCancellationRequested();
            }
            catch (OperationCanceledException e)
            {
                await Context.SceneContextLoader.UnloadAllScenesAsync();
                throw;
            }
        });
    }
}