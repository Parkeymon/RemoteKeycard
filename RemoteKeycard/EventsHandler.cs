using System;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using Interactables.Interobjects.DoorUtils;
using RemoteKeycard.Extensions;
using Players = Exiled.Events.Handlers.Player;

namespace RemoteKeycard
{
    /// <summary>
    /// Handles all the events this plugin needs to function.
    /// </summary>
    public class EventsHandler
    {
        private readonly Config _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventsHandler"/> class.
        /// </summary>
        /// <param name="config">The <see cref="Config"/> settings that will be used.</param>
        public EventsHandler(Config config) => _config = config;

        /// <summary>
        /// Registers all events used.
        /// </summary>
        public void Start()
        {
            Log.Debug("Registering Events");
            Players.InteractingDoor += OnDoorInteract;
            Players.UnlockingGenerator += OnGeneratorUnlock;
            Players.InteractingLocker += OnLockerInteract;
            Players.ActivatingWarheadPanel += OnWarheadUnlock;
        }

        /// <summary>
        /// Unregisters all events used.
        /// </summary>
        public void Stop()
        {
            Players.InteractingDoor -= OnDoorInteract;
            Players.UnlockingGenerator -= OnGeneratorUnlock;
            Players.InteractingLocker -= OnLockerInteract;
            Players.ActivatingWarheadPanel -= OnWarheadUnlock;
        }

        private void OnDoorInteract(InteractingDoorEventArgs ev)
        {
            Log.Debug("Door Interact Event");
            try
            {
                if(!_config.AffectDoors)
                    return;
                
                Log.Debug($"Allowed: {ev.IsAllowed}, Permission?: {ev.Player.HasKeycardPermission(ev.Door.RequiredPermissions.RequiredPermissions)}, Current Item: ${ev.Player.CurrentItem}");

                if (!ev.IsAllowed && ev.Player.HasKeycardPermission(ev.Door.RequiredPermissions.RequiredPermissions) && !ev.Door.IsLocked)
                    ev.IsAllowed = true;

            } catch(Exception e)
            {
                if (_config.ShowExceptions) Log.Warn($"{nameof(OnDoorInteract)}: {e.Message}\n{e.StackTrace}");
            }
        }

        private void OnWarheadUnlock(ActivatingWarheadPanelEventArgs ev)
        {
            Log.Debug("Warhead Unlock Event");
            try
            {
                if(!_config.AffectWarheadPanel)
                    return;

                Log.Debug($"Allowed: {ev.IsAllowed}, Permission?: {ev.Player.HasKeycardPermission(KeycardPermissions.AlphaWarhead)}");
                
                if(!ev.IsAllowed && ev.Player.HasKeycardPermission(KeycardPermissions.AlphaWarhead))
                    ev.IsAllowed = true;

            } catch(Exception e)
            {
                if (_config.ShowExceptions) Log.Warn($"{nameof(OnWarheadUnlock)}: {e.Message}\n{e.StackTrace}");
            }
        }

        private void OnGeneratorUnlock(UnlockingGeneratorEventArgs ev)
        {
            Log.Debug("Generator Unlock Event");
            try
            {
                if(!_config.AffectGenerators)
                    return;

                Log.Debug($"Allowed: {ev.IsAllowed}, Permission?: {ev.Player.HasKeycardPermission(ev.Generator.Base._requiredPermission)}");

                if(!ev.IsAllowed && ev.Player.HasKeycardPermission(ev.Generator.Base._requiredPermission))
                    ev.IsAllowed = true;
                
            } catch(Exception e)
            {
                if (_config.ShowExceptions) Log.Warn($"{nameof(OnGeneratorUnlock)}: {e.Message}\n{e.StackTrace}");
            }
        }

        private void OnLockerInteract(InteractingLockerEventArgs ev)
        {
            Log.Debug("Locker Interact Event");
            try
            {
                if(!_config.AffectScpLockers)
                    return;

                Log.Debug($"Allowed: {ev.IsAllowed}, Permission?: {ev.Player.HasKeycardPermission(ev.Chamber.RequiredPermissions, true)}");
                
                if(!ev.IsAllowed && ev.Chamber != null && ev.Player.HasKeycardPermission(ev.Chamber.RequiredPermissions, true))
                    ev.IsAllowed = true;
                
            } catch(Exception e)
            {
                if (_config.ShowExceptions) Log.Warn($"{nameof(OnLockerInteract)}: {e.Message}\n{e.StackTrace}");
            }
        }
    }
}