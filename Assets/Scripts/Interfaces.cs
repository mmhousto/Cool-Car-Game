using UnityEngine;

public interface IPushable
    {
        public void Push(Vector2 direction, float force);
    }

    public interface IDamagable
    {
        public void DamageRpc();
    }
