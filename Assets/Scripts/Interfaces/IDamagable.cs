using System;

namespace Spirinse.Interfaces
{
    public interface IDamagable
    {
        //event EventHandler<int> TakeDamage;
        int TakeDamage(int damage);
    }
}