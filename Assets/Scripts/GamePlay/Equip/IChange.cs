using UnityEngine;

public interface IChangePhysics
{
    void Change(Transform playerTransform, Rigidbody2D playerRigitBody, PlayerControl control);
}

public interface IChangeCharacter
{
    void Change(PlayerCharacterLogic character);
}

public interface IChange
{
    void Change();
}
