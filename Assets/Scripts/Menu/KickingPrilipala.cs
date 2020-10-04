using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Menu
{
    public class KickingPrilipala : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private RectTransform target;

        [SerializeField] private Vector2 playPosition;
        [SerializeField] private Vector2 interactivePosition;
        [SerializeField] private Vector2 equipPosition;
        private Vector2 nextPosition;

        private bool isMove = false;

        private void Update()
        {
            if (isMove)
            {
                target.anchoredPosition = Vector2.MoveTowards(target.anchoredPosition, nextPosition, speed * Time.deltaTime);
            }
        }

        private void StartMove()
        {
            isMove = true;
        }

        public void MoveToPlay()
        {
            StartMove();
            nextPosition = playPosition;
        }

        public void MoveToInteractive()
        {
            StartMove();
            nextPosition = interactivePosition;
        }

        public void MoveToEquip()
        {
            StartMove();
            nextPosition = equipPosition;
        }
    }
}