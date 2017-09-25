using System;

namespace Svelto.ECS.Example.Flock
{
    class SettingsImplementor:ISettingsComponent
    {
        Main.BoidSettingsEx sts;

        public SettingsImplementor(Main.BoidSettingsEx sts)
        {
            this.sts = sts;
        }

        public Main.BoidSettingsEx settings
        {
            get
            {
                return sts;
            }
        }
    }
}