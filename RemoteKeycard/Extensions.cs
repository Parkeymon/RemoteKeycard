using Exiled.API.Enums;

namespace RemoteKeycard;

using System.Linq;
using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.Items;
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
        if (Plugin.Instance != null && Plugin.Instance.Config.AmnesiaMatters && player.IsEffectActive<AmnesiaVision>())
            return false;

        return player.Items.Any(item => item is Keycard keycard && item.Base is IDoorPermissionProvider keycardProvider && permissions.PermissionsPolicy.CheckPermissions(keycardProvider.GetPermissions(permissions)));
    }
}