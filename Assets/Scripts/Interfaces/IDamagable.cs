using System;

namespace Spirinse.Interfaces
{
    public interface IDamagable
    {
        //event EventHandler<int> TakeDamage;
        bool TakeDamage(int damage);
    }
}