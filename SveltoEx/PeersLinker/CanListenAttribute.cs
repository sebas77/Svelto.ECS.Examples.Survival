using System;

namespace Svelto.PeersLinker
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CanListenAttribute : Attribute
    {
        public Type notificationType { get; private set; }

        public CanListenAttribute(Type notificationClass)
        {
            notificationType = notificationClass;
        }
    }
}
