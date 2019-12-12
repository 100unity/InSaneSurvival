﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interfaces
{
    public interface IMovable
    {
        void Move(Vector3 position);

        bool FaceTarget(GameObject target, bool shouldTurn, out float difference);

        void StopMoving();
    }
}