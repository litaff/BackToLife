﻿using UnityEngine;

namespace BackToLife
{
    public class EndTile : Block
    {
        public override Vector2 Move(Vector2 dir)
        {
            return dir;
        }
    }
}