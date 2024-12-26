#nullable enable
namespace RemoteKeycard;

using System;
using Exiled.API.Features;

public class Plugin : Plugin<Config>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Plugin" /> class.
    ///     Instance initializer.
    /// </summary>
    public Plugin()
    {
        Instance = this;
    }

    /// <summary>
    ///     Gets a static instance of this class.
    /// </summary>
    public static Plugin? Instance { get; private set; }

    /// <inheritdoc />
    public override string Name => "RemoteKeycard";

    /// <inheritdoc />
    public override string Prefix => "remote_keycard";

    /// <inheritdoc />
    public override Version RequiredExiledVersion => new(8, 4, 2);

    /// <inheritdoc />
    public override string Author => "Beryl (Maintained by Parkeymon)";

    /// <inheritdoc />
    public override Version Version => new(3, 3, 2);

    /// <inheritdoc cref="EventHandlers" />
    private EventHandlers? Handler { get; set; }

    /// <inheritdoc />
    public override void OnEnabled()
    {
        Log.Debug("Initializing events...");
        Handler = new EventHandlers(Config);
        Handler.Start();
        Log.Debug("Events initialized successfully.");

        base.OnEnabled();
    }

    /// <inheritdoc />
    public override void OnDisabled()
    {
        Log.Debug("Stopping events...");
        Handler?.Stop();
        Handler = null;
        Log.Debug("Events stopped successfully.");

        base.OnDisabled();
    }
}