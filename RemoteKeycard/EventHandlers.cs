namespace RemoteKeycard;

#if EXILED
using Exiled.Events.EventArgs.Player;
using PlayerEvents = Exiled.Events.Handlers.Player;
#else
using LabApi.Events.Handlers;
using LabApi.Events.Arguments.PlayerEvents;
using System.Linq;
#endif
using System;
using MapGeneration.Distributors;
using Log = LabApi.Features.Console.Logger;

/// <inheritdoc />
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
        PlayerEvents.InteractingDoor += OnDoorInteract;
        PlayerEvents.UnlockingGenerator += OnGeneratorUnlock;
        PlayerEvents.InteractingLocker += OnLockerInteract;
#if EXILED
        PlayerEvents.ActivatingWarheadPanel += OnWarheadUnlock;
#else
        PlayerEvents.UnlockingWarheadButton += OnWarheadUnlock;
#endif
    }

    /// <summary>
    ///     Unregisters all events used.
    /// </summary>
    public void Stop()
    {
        PlayerEvents.InteractingDoor -= OnDoorInteract;
        PlayerEvents.UnlockingGenerator -= OnGeneratorUnlock;
        PlayerEvents.InteractingLocker -= OnLockerInteract;
#if EXILED
        PlayerEvents.ActivatingWarheadPanel -= OnWarheadUnlock;
#else
        PlayerEvents.UnlockingWarheadButton -= OnWarheadUnlock;
#endif
    }

#if EXILED
    private void OnDoorInteract(InteractingDoorEventArgs ev)
#else
    private void OnDoorInteract(PlayerInteractingDoorEventArgs ev)
#endif
    {
        Log.Debug("Door Interact Event", Plugin.Instance.Config.Debug);
        try
        {
            if (!_config.AffectDoors)
                return;

            Log.Debug($"Allowed: {ev.IsAllowed}, Permission?: {ev.Player.HasKeycardPermission(ev.Door.Base)}, Current Item: ${ev.Player.CurrentItem}", Plugin.Instance.Config.Debug);
#if EXILED
            if (!ev.IsAllowed && ev.Player.HasKeycardPermission(ev.Door.Base) && !ev.Door.IsLocked)
                ev.IsAllowed = true;
#else
            if (!ev.CanOpen && ev.Player.HasKeycardPermission(ev.Door.Base) && !ev.Door.IsLocked)
                ev.CanOpen = true;
#endif
        }
        catch (Exception e)
        {
            if (_config.ShowExceptions)
                Log.Warn($"{nameof(OnDoorInteract)}: {e.Message}\n{e.StackTrace}");
        }
    }

#if EXILED
    private void OnGeneratorUnlock(UnlockingGeneratorEventArgs ev)
#else
    private void OnGeneratorUnlock(PlayerUnlockingGeneratorEventArgs ev)
#endif
    {
        Log.Debug("Generator Unlock Event", Plugin.Instance.Config.Debug);
        try
        {
            if (!_config.AffectGenerators)
                return;

            Log.Debug($"Allowed: {ev.IsAllowed}, Permission?: {ev.Player.HasKeycardPermission(ev.Generator.Base)}", Plugin.Instance.Config.Debug);
#if EXILED
            if (!ev.IsAllowed && ev.Player.HasKeycardPermission(ev.Generator.Base))
                ev.IsAllowed = true;
#else
            if (!ev.CanOpen && ev.Player.HasKeycardPermission(ev.Generator.Base))
                ev.CanOpen = true;
#endif
        }
        catch (Exception e)
        {
            if (_config.ShowExceptions)
                Log.Warn($"{nameof(OnGeneratorUnlock)}: {e.Message}\n{e.StackTrace}");
        }
    }

#if EXILED
    private void OnLockerInteract(InteractingLockerEventArgs ev)
    {
        LockerChamber locker = ev.InteractingChamber?.Base;
#else
    private void OnLockerInteract(PlayerInteractingLockerEventArgs ev)
    {
        LockerChamber locker = ev.Chamber?.Base;
#endif
        Log.Debug("Locker Interact Event", Plugin.Instance.Config.Debug);
        try
        {
            if (!_config.AffectScpLockers)
                return;

            Log.Debug($"Allowed: {ev.IsAllowed}, Permission?: {ev.Player.HasKeycardPermission(locker)}", Plugin.Instance.Config.Debug);
#if EXILED
            if (!ev.IsAllowed && locker != null && ev.Player.HasKeycardPermission(locker))
                ev.IsAllowed = true;
#else
            if (!ev.CanOpen && locker != null && ev.Player.HasKeycardPermission(locker))
                ev.CanOpen = true;
#endif
        }
        catch (Exception e)
        {
            if (_config.ShowExceptions)
                Log.Warn($"{nameof(OnLockerInteract)}: {e.Message}\n{e.StackTrace}");
        }
    }

#if EXILED
    private void OnWarheadUnlock(ActivatingWarheadPanelEventArgs ev)
#else
    private void OnWarheadUnlock(PlayerUnlockingWarheadButtonEventArgs ev)
#endif
    {
        Log.Debug("Warhead Unlock Event", Plugin.Instance.Config.Debug);
        try
        {
            if (!_config.AffectWarheadPanel)
                return;

            Log.Debug($"Allowed: {ev.IsAllowed}, Permission?: {ev.Player.HasKeycardPermission(AlphaWarheadActivationPanel.Instance)}", Plugin.Instance.Config.Debug);

            if (!ev.IsAllowed && ev.Player.HasKeycardPermission(AlphaWarheadActivationPanel.Instance))
                ev.IsAllowed = true;
        }
        catch (Exception e)
        {
            if (_config.ShowExceptions)
                Log.Warn($"{nameof(OnWarheadUnlock)}: {e.Message}\n{e.StackTrace}");
        }
    }
}