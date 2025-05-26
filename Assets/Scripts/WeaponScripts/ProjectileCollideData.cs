using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

namespace Projectiles
{
    public enum CollisionMode
    {
        NONE,
        PIERCE,
        NORMAL,
        BOUNCE
    }

    public struct HitData
    {
        public Damageable damageable;
        public Vector3 hitPos;
        public HitData(Damageable _d, Vector3 _p)
        {
            damageable = _d;
            hitPos = _p;
        }
    }


    public struct CollisionData
    {
        public CollisionMode type;
        public Vector3 hitNormal;
        public float hitDistance;
        public List<HitData> hitObjects;
        public CollisionData(CollisionMode _m, Vector3 _n, float _d, List<HitData> _o)
        {
            type = _m;
            hitNormal = _n;
            hitDistance = _d;
            hitObjects = _o;
        }
    }
}