namespace Svelto.ECS.Example.Survive.Characters
{
    struct DamageablePlayerEntityStruct:IEntityStruct
    {
        public DamageInfo damageInfo;
        public bool damaged;
        public EGID ID { get; set; }
    }
    
    struct DamageableEnemyEntityStruct:IEntityStruct
    {
        public DamageInfo damageInfo;
        public bool       damaged;
        public EGID       ID { get; set; }
    }
}