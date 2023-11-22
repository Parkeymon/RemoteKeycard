﻿namespace RemoteKeycard;

using System;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Interactables.Interobjects.DoorUtils;
using Players = Exiled.Events.Handlers.Player;

public class EventHandlers
{
    private readonly Config config;

    /// <summary>
    ///     Initializes a new instance of the <see cref="EventHandlers" /> class.
    /// </summary>
    /// <param name="config">The <see cref="Config" /> settings that will be used.</param>
    public EventHandlers(Config config)
    {
        this.config = config;
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
            if (!config.AffectDoors)
                return;

            Log.Debug(
                $"Allowed: {ev.IsAllowed}, Permission?: {ev.Player.HasKeycardPermission(ev.Door.RequiredPermissions.RequiredPermissions)}, Current Item: ${ev.Player.CurrentItem}");

            if (!ev.IsAllowed && ev.Player.HasKeycardPermission(ev.Door.RequiredPermissions.RequiredPermissions) &&
                !ev.Door.IsLocked)
                ev.IsAllowed = true;
        }
        catch (Exception e)
        {
            if (config.ShowExceptions)
                Log.Warn($"{nameof(OnDoorInteract)}: {e.Message}\n{e.StackTrace}");
        }
    }

    private void OnWarheadUnlock(ActivatingWarheadPanelEventArgs ev)
    {
        Log.Debug("Warhead Unlock Event");
        try
        {
            if (!config.AffectWarheadPanel)
                return;

            Log.Debug(
                $"Allowed: {ev.IsAllowed}, Permission?: {ev.Player.HasKeycardPermission(KeycardPermissions.AlphaWarhead)}");

            if (!ev.IsAllowed && ev.Player.HasKeycardPermission(KeycardPermissions.AlphaWarhead))
                ev.IsAllowed = true;
        }
        catch (Exception e)
        {
            if (config.ShowExceptions)
                Log.Warn($"{nameof(OnWarheadUnlock)}: {e.Message}\n{e.StackTrace}");
        }
    }

    private void OnGeneratorUnlock(UnlockingGeneratorEventArgs ev)
    {
        Log.Debug("Generator Unlock Event");
        try
        {
            if (!config.AffectGenerators)
                return;

            Log.Debug(
                $"Allowed: {ev.IsAllowed}, Permission?: {ev.Player.HasKeycardPermission(ev.Generator.Base._requiredPermission)}");

            if (!ev.IsAllowed && ev.Player.HasKeycardPermission(ev.Generator.Base._requiredPermission))
                ev.IsAllowed = true;
        }
        catch (Exception e)
        {
            if (config.ShowExceptions)
                Log.Warn($"{nameof(OnGeneratorUnlock)}: {e.Message}\n{e.StackTrace}");
        }
    }

    private void OnLockerInteract(InteractingLockerEventArgs ev)
    {
        Log.Debug("Locker Interact Event");
        try
        {
            if (!config.AffectScpLockers)
                return;

            Log.Debug(
                $"Allowed: {ev.IsAllowed}, Permission?: {ev.Player.HasKeycardPermission(ev.Chamber.RequiredPermissions, true)}");

            if (!ev.IsAllowed && ev.Chamber != null &&
                ev.Player.HasKeycardPermission(ev.Chamber.RequiredPermissions, true))
                ev.IsAllowed = true;
        }
        catch (Exception e)
        {
            if (config.ShowExceptions)
                Log.Warn($"{nameof(OnLockerInteract)}: {e.Message}\n{e.StackTrace}");
        }
    }
}