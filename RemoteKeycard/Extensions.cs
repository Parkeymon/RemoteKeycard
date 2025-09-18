namespace RemoteKeycard;

using System.Linq;
using CustomPlayerEffects;
#if EXILED
using Exiled.API.Features;
using Exiled.API.Features.Items;
#else
using LabApi.Features.Wrappers;
using Keycard = LabApi.Features.Wrappers.KeycardItem;
#endif
using Interactables.Interobjects.DoorUtils;

public static class Extensions
{
    /// <summary>
    ///     Checks whether the player has a keycard of a specific permission.
    /// </summary>
    /// <param name="player"><see cref="Player" /> trying to interact.</param>
    /// <param name="permissions">The permission that's gonna be searched for.</param>
    /// <returns>Whether the player has the required keycard.</returns>
    public static bool HasKeycardPermission(this Player player, IDoorPermissionRequester permissions)
    {
        if (Plugin.Instance.Config.AmnesiaMatters
#if EXILED
            && player.IsEffectActive<AmnesiaVision>())
#else
            && player.HasEffect<AmnesiaVision>())
#endif
            return false;

        return player.Items.Any(item => item is Keycard keycard && item.Base is IDoorPermissionProvider keycardProvider && permissions.PermissionsPolicy.CheckPermissions(keycardProvider.GetPermissions(permissions)));
    }
}