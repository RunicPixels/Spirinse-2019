using System;

namespace DefaultNamespace
{
    public interface IDamagable
    {
        //Action<int> TakeDamageAction();
        void TakeDamage(float damage);
    }
}