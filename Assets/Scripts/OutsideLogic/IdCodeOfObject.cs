using UnityEngine;

public class IdCodeOfObject : MonoBehaviour, IHave<IdCode>
{
    public IdCode Item => idCode;
    [SerializeField] private IdCode idCode;
}