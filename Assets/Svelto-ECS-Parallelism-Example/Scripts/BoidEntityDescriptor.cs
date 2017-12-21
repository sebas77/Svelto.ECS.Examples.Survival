namespace Svelto.ECS.Example.Parallelism
{
#if FOURTH_TIER_EXAMPLE
    class BoidEntityDescriptor : MixedEntityDescriptor<EntityStructWithBuilder<BoidEntityView>>
#else
    class BoidEntityDescriptor : MixedEntityDescriptor<EntityViewBuilder<BoidEntityView>>
#endif
    {}
}