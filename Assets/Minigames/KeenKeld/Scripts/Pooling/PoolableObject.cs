using UnityEngine;
using System.Collections;

namespace KeenKeld
{
    public abstract class PoolableObject : MonoBehaviour
    {
        public abstract void Activate();
        public abstract void Deactivate();
        public abstract bool IsActive();
    }
}