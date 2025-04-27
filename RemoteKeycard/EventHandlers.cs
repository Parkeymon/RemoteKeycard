using Exiled.API.Enums;

namespace RemoteKeycard;

using System;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Players = Exiled.Events.Handlers.Player;

public class EventHandlers
{
    private readonly Config _config;

    /// <summary>
    ///     Initializes a new instance of the <see cref="EventHandlers" /> class.
    /// </summary>
    /// <param name="config">The <see cref="Config" /> settings that will be used.</param>
    public EventHandlers(Config config)
    {
        this._config = config;
    }

    /// <summary>
    ///     Registers all events used.
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
    ///     Unregisters all events used.
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
            if (!_config.AffectDoors)
                return;

            Log.Debug(
                $"Allowed: {ev.IsAllowed}, Permission?: {ev.Player.HasKeycardPermission(ev.Door.KeycardPermissions)}, Current Item: ${ev.Player.CurrentItem}");

            if (!ev.IsAllowed && ev.Player.HasKeycardPermission(ev.Door.KeycardPermissions) &&
                !ev.Door.IsLocked)
                ev.IsAllowed = true;
        }
        catch (Exception e)
        {
            if (_config.ShowExceptions)
                Log.Warn($"{nameof(OnDoorInteract)}: {e.Message}\n{e.StackTrace}");
        }
    }

    private void OnWarheadUnlock(ActivatingWarheadPanelEventArgs ev)
    {
        Log.Debug("Warhead Unlock Event");
        try
        {
            if (!_config.AffectWarheadPanel)
                return;

            Log.Debug(
                $"Allowed: {ev.IsAllowed}, Permission?: {ev.Player.HasKeycardPermission(KeycardPermissions.AlphaWarhead)}");

            if (!ev.IsAllowed && ev.Player.HasKeycardPermission(KeycardPermissions.AlphaWarhead))
                ev.IsAllowed = true;
        }
        catch (Exception e)
        {
            if (_config.ShowExceptions)
                Log.Warn($"{nameof(OnWarheadUnlock)}: {e.Message}\n{e.StackTrace}");
        }
    }

    private void OnGeneratorUnlock(UnlockingGeneratorEventArgs ev)
    {
        Log.Debug("Generator Unlock Event");
        try
        {
            if (!_config.AffectGenerators)
                return;

            Log.Debug(
                $"Allowed: {ev.IsAllowed}, Permission?: {ev.Player.HasKeycardPermission(ev.Generator.KeycardPermissions)}");

            if (!ev.IsAllowed && ev.Player.HasKeycardPermission(ev.Generator.KeycardPermissions))
                ev.IsAllowed = true;
        }
        catch (Exception e)
        {
            if (_config.ShowExceptions)
                Log.Warn($"{nameof(OnGeneratorUnlock)}: {e.Message}\n{e.StackTrace}");
        }
    }

    private void OnLockerInteract(InteractingLockerEventArgs ev)
    {
        Log.Debug("Locker Interact Event");
        try
        {
            if (!_config.AffectScpLockers)
                return;

            Log.Debug(
                $"Allowed: {ev.IsAllowed}, Permission?: {ev.Player.HasKeycardPermission(ev.InteractingChamber.RequiredPermissions, true)}");

            if (!ev.IsAllowed && ev.InteractingChamber != null &&
                ev.Player.HasKeycardPermission(ev.InteractingChamber.RequiredPermissions, true))
                ev.IsAllowed = true;
        }
        catch (Exception e)
        {
            if (_config.ShowExceptions)
                Log.Warn($"{nameof(OnLockerInteract)}: {e.Message}\n{e.StackTrace}");
        }
    }
}