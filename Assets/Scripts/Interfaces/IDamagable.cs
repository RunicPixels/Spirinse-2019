using System;

namespace Spirinse.Interfaces
{
    public interface IDamagable
    {
        //event EventHandler<int> TakeDamage;
        void TakeDamage(int damage);
    }
}