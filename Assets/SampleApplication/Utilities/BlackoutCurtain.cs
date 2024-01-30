using System.Threading.Tasks;
using Mew.Core.TaskHelpers;
using Mew.Core.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class BlackoutCurtain : MonoBehaviour
{
    [SerializeField] private Graphic target;
    [SerializeField] private bool isOn;
    [SerializeField] private float speed = 1f;

    private readonly TaskQueue taskQueue = new TaskQueue();

    private void Awake()
    {
        taskQueue.Start(destroyCancellationToken);
    }

    private void Update()
    {
        var color = target.color;
        var delta = Time.unscaledDeltaTime * speed;
        var a = Mathf.Clamp01(color.a + (isOn ? delta : -delta));
        target.color = new Color(color.r, color.g, color.b, a);
    }

    public async Task FadeOut()
    {
        await taskQueue.EnqueueAsync(async _ =>
        {
            isOn = true;
            target.raycastTarget = true;
            while (target.color.a < 1f)
                await TaskHelper.NextFrame();
        });
    }

    public async Task FadeIn()
    {
        await taskQueue.EnqueueAsync(async _ =>
        {
            isOn = false;
            while (target.color.a > 0f)
                await TaskHelper.NextFrame();
            target.raycastTarget = false;
        });
    }
}