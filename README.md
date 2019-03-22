## Why?
As noted in the Sitecore security hardening guide, using the Master database directly on CD instances is against the Sitecore best practices. Unfortunately, there are times when you need the master database because of saving a file to Media Library or importing data as Sitecore items.

One of the options to fulfill the requirements for a similar scenario is to use Sitecore’s remote events. Trigger a remote event, catch it on CM instance and process using your event handler processor. In order to avoid implementing transferring of uploaded file from CD instances to CM everytime, I have implemented a reusable foundation library. It does not depend on anything other than Sitecore Kernel and Logging packages.

Although the aim is to transfer files from CD to CM, with some configuration tweaks, you can move files between all instances.

The library has tested on Standalone and Azure XP scaled environments. So far it works as expected.

You can access to the source code on github https://github.com/b169/sc-fileexchange

## How to use it?
* Add a reference to Foundation.SitecoreFileExchange library. 
* When you want to transfer an uploaded file to CM instance, call RemoteFileExhangeService.Store function
* On CM instance implement an event handler and subscribe to remotefile:uploadfinished event. Make sure your event runs before built-in handler.
  <handler type=”Foundation.SitecoreFileExchange.Events.RemoteFileExchangeEvents” method=”HandleFileUploadFinished” patch:after=”processor[position()=last()]”></handler>

## Example

### Class
```
  public class MyController
  { 
      private readonly IRemoteFileExchangeService _fileService;
      private readonly ILog _log;

      public MyController(IRemoteFileExchangeService service)
      {
          _log = LoggerFactory.GetLogger(typeof(MyController));
          _fileService = service;
      }

      private void SaveToSitecore(HttpPostedFileBase file)
      {
          try
          {
              _fileService.Store( file.InputStream,
                                  file.FileName,
                                  new FileExchangeEventData
                                  {
                                      RaiseEvent = true,
                                      Data = "My Custom data",
                                      Key = "MyControllerKey"
                                  });
          }
          catch (Exception ex)
          {
                  _log.Error("Error while storing in file exchange service", ex);
          }       
      }
  }
```
### Events.config
```
  <?xml version="1.0" encoding="utf-8" ?>
  <configuration xmlns:patch="http://www.sitecore.net/xmlconfig/"
                 xmlns:role="http://www.sitecore.net/xmlconfig/role/">
    <sitecore>
      <events role:require="Standalone or ContentManagement">
        <event name="remotefile:uploadfinished">
          <handler type="Feature.MyController.Events.EventHandlers" method="HandleUploadFinished" patch:before="handler[@type='Foundation.SitecoreFileExchange.Events.RemoteFileExchangeEvents']"></handler>
        </event>
      </events>   
    </sitecore>
  </configuration>
```
### Event Handler
```
  namespace Feature.MyController.Events
  {
      public class EventHandlers
      {
          private readonly ILog _log;
          public EventHandlers()
          {
              _log = LoggerFactory.GetLogger(typeof(EventHandlers));
          }

          public void HandleUploadFinished(object sender, EventArgs args)
          {
              var eventArgs = Event.ExtractParameter(args, 0) as RemoteFileExchangeFinishedEventArgs;
              _log.Info($"Remote upload finished event. {eventArgs.FileName} Key: {eventArgs.Key} CustomData {eventArgs.CustomData} ");
          }
      }
  }
  ```
