using System.Threading.Tasks;
using Doinject;
using Mew.Core.Assets;
using Mew.Core.Tasks;
using UnityEngine;

public class SceneManagement : MonoBehaviour, IInjectableComponent
{
    [SerializeField] private UnifiedScene stageSelectScene = default;
    [SerializeField] private UnifiedScene titleScene = default;
    [SerializeField] private UnifiedScene inGameScene = default;

    public IContext Context { get; set; }
    private BlackoutCurtain BlackoutCurtain { get; set; }

    [Inject]
    public void Construct(
        IContext context,
        [Optional] BlackoutCurtain blackoutCurtain)
    {
        TaskQueue.DisposeWith(destroyCancellationToken);
        Context = context;
        BlackoutCurtain = blackoutCurtain;
    }

    [OnInjected]
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
            if (BlackoutCurtain) await BlackoutCurtain.FadeOut();
            await Context.SceneContextLoader.UnloadAllScenesAsync();
            await Context.SceneContextLoader.LoadAsync(to, active: true, contextArg);
            if (BlackoutCurtain) await BlackoutCurtain.FadeIn();
        });
    }
}