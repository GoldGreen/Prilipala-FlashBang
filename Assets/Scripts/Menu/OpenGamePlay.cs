using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class OpenGamePlay : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private float minLenToOpenGameplay = 3;
    [SerializeField] private float multiplySwiping = 0.01f;

    [SerializeField] private GameObject target;
    [SerializeField] private OpenSceneScript openingScene;

    private Transform targetTranform;

    private Vector3 startSwipe;
    private Vector3 endSwipe;

    private void Awake()
    {
        targetTranform = target.GetComponent<Transform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startSwipe = targetTranform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var pos = targetTranform.position;
        pos.y += eventData.delta.y * multiplySwiping;
        targetTranform.position = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        endSwipe = targetTranform.position;

        if (endSwipe.y - startSwipe.y > minLenToOpenGameplay)
            openingScene.OpenScene();
        else
            targetTranform.position = startSwipe;
    }
}
