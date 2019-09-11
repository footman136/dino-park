using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectCreation;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Worker.CInterop;
using Assets.Gamelogic.Core;
using UnityEngine;

namespace DinoPark
{
    public class UnityClientConnector : WorkerConnector
    {
        public const string WorkerType = "UnityClient";
        private long _accountId;

        private async void Start()
        {
            var connParams = CreateConnectionParameters(WorkerType);
            connParams.Network.ConnectionType = NetworkConnectionType.Kcp;

            var builder = new SpatialOSConnectionHandlerBuilder()
                .SetConnectionParameters(connParams);

            if (!Application.isEditor)
            {
                var initializer = new CommandLineConnectionFlowInitializer();
                switch (initializer.GetConnectionService())
                {
                    case ConnectionService.Receptionist:
                        builder.SetConnectionFlow(new ReceptionistFlow(CreateNewWorkerId(WorkerType), initializer));
                        break;
                    case ConnectionService.Locator:
                        builder.SetConnectionFlow(new LocatorFlow(initializer));
                        break;
                    case ConnectionService.AlphaLocator:
                        builder.SetConnectionFlow(new AlphaLocatorFlow(initializer));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                builder.SetConnectionFlow(new ReceptionistFlow(CreateNewWorkerId(WorkerType)));
            }

            await Connect(builder, new ForwardingDispatcher()).ConfigureAwait(false);
        }

        protected override void HandleWorkerConnectionEstablished()
        {
            PlayerLifecycleHelper.AddClientSystems(Worker.World);
            
            // 改变状态
            GameManager.Instance.StateMachine.TriggerTransition(ConnectionFSMStateEnum.StateEnum.CONNECTED);
            
            // 创建实体的预制件
            var fallbackCreator = new GameObjectCreatorFromMetadata(Worker.WorkerType, Worker.Origin, Worker.LogDispatcher);
            var customCreator = new EntityGameObjectCreator(fallbackCreator, Worker.World, Worker.WorkerType);
            Debug.Log("HandleWorkerConnectionEstablished!");
            
            GameObjectCreationHelper.EnableStandardGameObjectCreation(Worker.World, customCreator);
        }
        
        protected override void HandleWorkerConnectionFailure(string errorMessage)
        {
            // 改变状态
            UIManager.Instance.SystemTips(errorMessage, PanelSystemTips.MessageType.Error);
            GameManager.Instance.StateMachine.TriggerTransition(ConnectionFSMStateEnum.StateEnum.LOGIN);
        }

        public void SetAccountID(long accountId)
        {
            _accountId = accountId;
        }
    }
}
