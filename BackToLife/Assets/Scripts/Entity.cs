﻿using System;
using UnityEngine;

namespace BackToLife
{
    [Serializable]
    public abstract class Entity : MonoBehaviour
    {
        public Vector2 gridPosition;
        public GameGrid.Cell cell;
        public EntityType type;
        public int weight;

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}