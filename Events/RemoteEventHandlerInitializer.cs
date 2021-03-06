﻿using Foundation.SitecoreFileExchange.Models;
using Sitecore.Pipelines;
using System;

namespace Foundation.SitecoreFileExchange.Events
{
    public class RemoteEventHandlerInitializer
    {
        public virtual void InitializeFromPipeline(PipelineArgs args)
        {
            var action = new Action<RemoteFileExchangeServiceArgs>(RaiseRemoteEvent);
            Sitecore.Eventing.EventManager.Subscribe(action);
        }
        private void RaiseRemoteEvent(RemoteFileExchangeServiceArgs args)
        {
            Sitecore.Events.Event.RaiseEvent(Constants.Events.FileUploadedRemoteEvent, args);
        }
    }
}