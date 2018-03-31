namespace Svelto.ECS.Example.Survive.Enemies
{
    public class EnemyAttackImplementor:IEnemyAttackDataComponent, IImplementor
    {
        public EnemyAttackImplementor(float timeBetweenAttacks, int attackDamage)
        {
            attackInterval = timeBetweenAttacks;
            damage = attackDamage;
        }
        
        public int damage { get; }
        public float attackInterval { get; }
        public float timer { get; set; }
    }
}