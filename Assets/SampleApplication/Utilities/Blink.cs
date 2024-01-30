using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    [SerializeField] Graphic target;
    [SerializeField] float interval = 1f;

    private void Update()
    {
        var t = (1f + Mathf.Sin(Time.unscaledTime * Mathf.PI * 2f / interval)) / 2f;
        t = Mathf.Pow(t, 0.7f);

        var color = target.color;
        target.color = new Color(color.r, color.g, color.b, Mathf.Lerp(0.2f, 1f, t));
    }
}