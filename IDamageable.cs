namespace DungeonExplorer
{
    
    // Every class that uses the IDamageable interface must take damage throughout the game:
    public interface IDamageable
    {

        // Every class that accepts the IDamageable interface must also implement the following methods:
        void TakeDamage(int damage);
        bool IsAlive();
    }
}