using UnityEngine;
using System.Collections.Generic;

public class ColorPipeline
{
    private class ColorWithTime
    {
        public Color Color { get; }
        public float Time { get; set; }

        public ColorWithTime(Color color, float time)
        {
            Color = color;
            Time = time;
        }
    }

    private readonly IEnumerable<SpriteRenderer> spriteRenderers;
    private readonly Stack<ColorWithTime> colorWithTimes = new Stack<ColorWithTime>();

    public ColorPipeline(IEnumerable<SpriteRenderer> spriteRenderers)
    {
        this.spriteRenderers = spriteRenderers;
    }

    public void Add(Color color, float time)
    {
        colorWithTimes.Push(new ColorWithTime(color, time));
    }

    public void DisctinctTime(float time)
    {
        colorWithTimes.ForEach(x => x.Time -= time);
    }

    public void UpdateColor()
    {
        while (colorWithTimes.Count > 0 && colorWithTimes.Peek().Time <= 0)
            colorWithTimes.Pop();

        var color = colorWithTimes.Count > 0 ? colorWithTimes.Peek().Color : Color.white;
        spriteRenderers.ForEach(x => x.color = color);
    }
}