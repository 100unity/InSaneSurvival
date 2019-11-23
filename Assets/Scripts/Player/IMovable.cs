using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{
    void Move(Vector3 position);

    bool FaceTarget(GameObject target, bool shouldTurn, out float difference);

    bool FaceTarget(GameObject target, bool shouldTurn);

    void StopMoving();
}
