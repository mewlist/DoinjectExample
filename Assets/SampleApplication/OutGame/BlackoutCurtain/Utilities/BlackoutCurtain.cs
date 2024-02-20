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
        taskQueue.DisposeWith(destroyCancellationToken);
    }

    private void Start()
    {
        isOn = true;
        target.raycastTarget = true;
        SetAlpha(1f);
    }

    private void Update()
    {
        var delta = Time.unscaledDeltaTime * speed;
        var a = Mathf.Clamp01(target.color.a + (isOn ? delta : -delta));
        SetAlpha(a);
    }

    public async Task FadeOut()
    {
        await taskQueue.EnqueueAsync(async ct =>
        {
            isOn = true;
            target.raycastTarget = true;
            while (target.color.a < 1f)
            {
                destroyCancellationToken.ThrowIfCancellationRequested();
                await TaskHelper.NextFrame(ct);
            }
        });
    }

    public async Task FadeIn()
    {
        await taskQueue.EnqueueAsync(async ct =>
        {
            isOn = false;
            while (target.color.a > 0f)
            {
                destroyCancellationToken.ThrowIfCancellationRequested();
                await TaskHelper.NextFrame(ct);
            }
            target.raycastTarget = false;
        });
    }

    private void SetAlpha(float alpha)
    {
        var color = target.color;
        target.color = new Color(color.r, color.g, color.b, alpha);
    }
}