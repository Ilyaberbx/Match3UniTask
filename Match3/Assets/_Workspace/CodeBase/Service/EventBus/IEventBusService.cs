﻿using System;
using _Workspace.CodeBase.Infrastructure.Service.EventBus.Handlers;

namespace _Workspace.CodeBase.Service.EventBus
{
    public interface IEventBusService
    {
        void CleanUp();
        void Subscribe(IGlobalSubscriber subscriber);
        void Unsubscribe(IGlobalSubscriber subscriber);

        void RaiseEvent<TSubscriber>(Action<TSubscriber> action)
            where TSubscriber : class, IGlobalSubscriber;
    }
}