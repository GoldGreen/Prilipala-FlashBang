using UnityEngine;

public class Sniper : GunAbstraction
{
    public override IdCode IdCode => IdCode.Sniper;

    protected override string BulletName => "SniperBullet";

    private LineRenderer lineRenderer;
    private IDisposableCollection subscribers = new Disposables();

    protected override void Awake()
    {
        base.Awake();
        lineRenderer = GetComponent<LineRenderer>();

        OnPlayerOutRadius.Subscribe(() => lineRenderer.enabled = false)
        .AddTo(subscribers);

        OnPlayerInRadius.Subscribe
        (
            () =>
            {
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, position);
                lineRenderer.SetPosition(1, playerPosition);
            }
        ).AddTo(subscribers);
    }

    private void OnDestroy()
    {
        subscribers.Dispose();
    }
}
