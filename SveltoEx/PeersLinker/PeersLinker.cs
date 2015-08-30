using Svelto.Command;
using Svelto.Command.Dispatcher;
using System;
using System.Collections.Generic;

namespace Svelto.PeersLinker
{
    public class PeersLinker
    {
        private Dictionary<IPeer, PeerWirer> _peers;

        public PeersLinker()
        {
            _peers = new Dictionary<IPeer, PeerWirer>();
        }

        public void Introduce(IPeerDispatcher colleague)
        {
            Introduce((IPeer)colleague);
        }

        public void Introduce(IPeerListener colleague)
        {
            Introduce((IPeer)colleague);
        }

        void Introduce(IPeer colleague)
        {
            if (_peers.ContainsKey(colleague) == false)
            {
                PeerWirer newWire = new PeerWirer(colleague);

                for (var peer = _peers.Values.GetEnumerator(); peer.MoveNext();)
                {
                    PeerWirer wirer = peer.Current;

                    Link(wirer, newWire);
                }

                _peers.Add(colleague, newWire);
            }
        }

        void Link(PeerWirer peerA, PeerWirer peerB)
        {
            peerA.Notifies(peerB);
            peerB.Notifies(peerA);
        }
    }

    sealed class PeerWirer
    {
        EventDispatcher             _dispatcher;
        Dictionary<Type, ICommand>  _answers;
        List<Type>                  _dispatches;
        IPeer                       _peer;

        internal PeerWirer(IPeer peer)
        {
            _dispatcher = new EventDispatcher();
            _answers = new Dictionary<Type, ICommand>();
            _dispatches = new List<Type>();

            _peer = peer;

            if (peer is IPeerDispatcher)
                (peer as IPeerDispatcher).notify += Dispatch;

            GatherAttributes();
        }

        internal void Notifies(PeerWirer subscriber)
        {
            foreach (Type notification in _dispatches)
            {
                if (subscriber.CanAnswer(notification) == true)
                    _dispatcher.Add(notification, subscriber._answers[notification]);
                else
                	throw new Exception("subscriber " + subscriber._peer.ToString() + " cannot answer to " + notification.ToString());
            }
        }

        void GatherAttributes()
        {
            Attribute[] attrs = Attribute.GetCustomAttributes(_peer.GetType());

            bool attributeFound = false;

            if (_peer is IPeerListener)
            {
                for (int i = attrs.Length - 1; i >= 0; --i)
                {
                    var attr = attrs[i];

                    if ((attr is CanListenAttribute))
                    {
                        Type notification = (attr as CanListenAttribute).notificationType;

                        _answers.Add(notification, (_peer as IPeerListener).CanExecuteCommand(notification));

                        attributeFound = true;
                    }

                    if (attributeFound == false)
                        throw new Exception("Listener " + _peer.ToString() + " doesn't have any compatible attribute found");
                }
            }

            attributeFound = false;

            if (_peer is IPeerDispatcher)
            {
                for (int i = attrs.Length - 1; i >= 0; --i)
                {
                    var attr = attrs[i];

                    if (attr is CanDispatchAttribute)
                    {
                        _dispatches.Add((attr as CanDispatchAttribute).notificationType);

                        attributeFound = true;
                    }
                }

                if (attributeFound == false)
                    throw new Exception("Dispatcher " + _peer.ToString() + " doesn't have any compatible attribute found");
            }
        }

        bool CanDispatch(Type notification)
        {
            return _dispatches.Contains(notification);
        }

        bool CanAnswer(Type notification)
        {
            return _answers.ContainsKey(notification);
        }

        void Dispatch(object notification)
        {
            if (CanDispatch(notification.GetType()))
                _dispatcher.Dispatch(notification.GetType(), notification);
            else
                throw new Exception("dispatcher " + _peer.ToString() + " cannot dispatch notification type " + notification.ToString());
        }
    }
}
