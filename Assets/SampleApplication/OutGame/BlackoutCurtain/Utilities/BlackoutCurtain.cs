using System.Threading.Tasks;
using Doinject;
using Mew.Core.TaskHelpers;
using Mew.Core.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class BlackoutCurtain : MonoBehaviour
{
    [SerializeField] private CanvasGroup target;
    [SerializeField] private bool isOn;
    [SerializeField] private float speed = 1f;
    [SerializeField] private Slider progression;

    private readonly TaskQueue taskQueue = new();

    private void Awake()
    {
        taskQueue.DisposeWith(destroyCancellationToken);
    }

    [OnInjected]
    // ReSharper disable once UnusedMember.Global
    public void Construct()
    {
        SetAlpha(1f);
        isOn = true;
        target.interactable = true;
        target.blocksRaycasts = true;
    }

    private void Update()
    {
        var delta = Time.unscaledDeltaTime * speed;
        var a = Mathf.Clamp01(target.alpha + (isOn ? delta : -delta));
        SetAlpha(a);
    }

    public async Task FadeOut()
    {
        await taskQueue.EnqueueAsync(async ct =>
        {
            isOn = true;
            target.interactable = true;
            target.blocksRaycasts = true;
            while (isOn && target.alpha < 1f)
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
            while (!isOn && target.alpha > 0f)
            {
                destroyCancellationToken.ThrowIfCancellationRequested();
                await TaskHelper.NextFrame(ct);
            }
            target.interactable = false;
            target.blocksRaycasts = false;
            ShowProgression(false);
        });
    }

    public void SetProgression(float value)
    {
        progression.value = value;
    }

    public void ShowProgression(bool value)
    {
        progression.gameObject.SetActive(value);
    }

    private void SetAlpha(float alpha)
    {
        target.alpha = alpha;
    }
}