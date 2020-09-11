using System;
using UnityEngine;

public class SightLogic : MonoBehaviour
{
    [SerializeField] private float rotatingSpeed;
    [SerializeField] private ParticleSystem destroyEffect;

    [SerializeField] private float deltaScaling;
    [SerializeField] private Vector3 maxScale;
    private Vector3 minScale;

    private float angle = 0;

    private GameObject target;
    private Transform targetTransform;

    private new Transform transform;

    private void Awake()
    {
        transform = GetComponent<Transform>();
        minScale = transform.localScale;
        deltaScaling *= Time.fixedDeltaTime;
        destroyEffect.transform.SetParent(null, true);
    }

    private void FixedUpdate()
    {
        if (targetTransform)
        {
            transform.position = targetTransform.position;
            transform.localScale = transform.localScale.Delta(deltaScaling, deltaScaling, deltaScaling);

            if (transform.localScale.x > maxScale.y || transform.localScale.x < minScale.x)
            {
                deltaScaling = -deltaScaling;
            }

            angle += rotatingSpeed;
            if (angle > 360)
            {
                angle %= 360;
            }

            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void SetAim(GameObject aim)
    {
        target = aim;
        targetTransform = aim.transform;
        transform.position = targetTransform.position;
    }

    public void DestroyAim()
    {
        target.GetComponent<IDisposable>().Dispose();

        destroyEffect.transform.position = transform.position;
        destroyEffect.Play();

        target.SetActive(false);
        gameObject.SetActive(false);

        target = null;
        targetTransform = null;
    }
}
