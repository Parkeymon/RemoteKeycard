namespace RemoteKeycard;

#if EXILED
using PlayerEvents = Exiled.Events.Handlers.Player;
using PlayerUnlockingGeneratorEventArgs = Exiled.Events.EventArgs.Player.UnlockingGeneratorEventArgs;
using PlayerInteractingLockerEventArgs = Exiled.Events.EventArgs.Player.InteractingLockerEventArgs;
using PlayerUnlockingWarheadButtonEventArgs = Exiled.Events.EventArgs.Player.ActivatingWarheadPanelEventArgs;
using PlayerInteractingDoorEventArgs = Exiled.Events.EventArgs.Player.InteractingDoorEventArgs;
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

    private void OnDoorInteract(PlayerInteractingDoorEventArgs ev)
    {
        Log.Debug("Door Interact Event", Plugin.Instance.Config.Debug);
        try
        {
            if (!_config.AffectDoors)
                return;

#if EXILED
            if (!ev.IsAllowed && ev.Player.HasKeycardPermission(ev.Door.Base) && !ev.Door.IsLocked)
                ev.IsAllowed = true;

            Log.Debug($"Allowed: {ev.IsAllowed}, Permission?: {ev.Player.HasKeycardPermission(ev.Door.Base)}, Current Item: ${ev.Player.CurrentItem}", Plugin.Instance.Config.Debug);
#else
            if (!ev.CanOpen && ev.Player.HasKeycardPermission(ev.Door.Base) && !ev.Door.IsLocked)
                ev.CanOpen = true;

            Log.Debug($"Allowed: {ev.CanOpen}, Permission?: {ev.Player.HasKeycardPermission(ev.Door.Base)}, Current Item: ${ev.Player.CurrentItem}", Plugin.Instance.Config.Debug);
#endif
        }
        catch (Exception e)
        {
            if (_config.ShowExceptions)
                Log.Warn($"{nameof(OnDoorInteract)}: {e.Message}\n{e.StackTrace}");
        }
    }

    private void OnGeneratorUnlock(PlayerUnlockingGeneratorEventArgs ev)
    {
        Log.Debug("Generator Unlock Event", Plugin.Instance.Config.Debug);
        try
        {
            if (!_config.AffectGenerators)
                return;
            
#if EXILED
            if (!ev.IsAllowed && ev.Player.HasKeycardPermission(ev.Generator.Base))
                ev.IsAllowed = true;

            Log.Debug($"Allowed: {ev.IsAllowed}, Permission?: {ev.Player.HasKeycardPermission(ev.Generator.Base)}", Plugin.Instance.Config.Debug);
#else
            if (!ev.CanOpen && ev.Player.HasKeycardPermission(ev.Generator.Base))
                ev.CanOpen = true;

            Log.Debug($"Allowed: {ev.CanOpen}, Permission?: {ev.Player.HasKeycardPermission(ev.Generator.Base)}", Plugin.Instance.Config.Debug);
#endif
        }
        catch (Exception e)
        {
            if (_config.ShowExceptions)
                Log.Warn($"{nameof(OnGeneratorUnlock)}: {e.Message}\n{e.StackTrace}");
        }
    }

    private void OnLockerInteract(PlayerInteractingLockerEventArgs ev)
    {
        Log.Debug("Locker Interact Event", Plugin.Instance.Config.Debug);
        try
        {
            if (!_config.AffectScpLockers)
                return;
#if EXILED
            LockerChamber locker = ev.InteractingChamber?.Base;

            if (!ev.IsAllowed && locker != null && ev.Player.HasKeycardPermission(locker))
                ev.IsAllowed = true;

            Log.Debug($"Allowed: {ev.IsAllowed}, Permission?: {ev.Player.HasKeycardPermission(locker)}", Plugin.Instance.Config.Debug);
#else
            LockerChamber locker = ev.Chamber?.Base;

            if (!ev.CanOpen && locker != null && ev.Player.HasKeycardPermission(locker))
                ev.CanOpen = true;

            Log.Debug($"Allowed: {ev.CanOpen}, Permission?: {ev.Player.HasKeycardPermission(locker)}", Plugin.Instance.Config.Debug);
#endif

        }
        catch (Exception e)
        {
            if (_config.ShowExceptions)
                Log.Warn($"{nameof(OnLockerInteract)}: {e.Message}\n{e.StackTrace}");
        }
    }

    private void OnWarheadUnlock(PlayerUnlockingWarheadButtonEventArgs ev)
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