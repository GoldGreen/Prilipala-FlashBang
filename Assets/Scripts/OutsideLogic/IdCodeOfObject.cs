using UnityEngine;

public class IdCodeOfObject : MonoBehaviour, IHaveIdCode
{
    public IdCode IdCode => idCode;
    [SerializeField] private IdCode idCode;
}