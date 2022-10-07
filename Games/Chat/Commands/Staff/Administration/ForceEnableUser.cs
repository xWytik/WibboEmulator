namespace WibboEmulator.Games.Chat.Commands.Staff.Administration;
using WibboEmulator.Games.GameClients;
using WibboEmulator.Games.Rooms;

internal class ForceEnableUser : IChatCommand
{
    public void Execute(GameClient session, Room room, RoomUser userRoom, string[] parameters)
    {
        if (parameters.Length != 3)
        {
            return;
        }

        var username = parameters[1];

        var roomUserByUserId = session.GetUser().CurrentRoom.GetRoomUserManager().GetRoomUserByName(username);
        if (roomUserByUserId == null || roomUserByUserId.GetClient() == null)
        {
            return;
        }

        if (session.Langue != roomUserByUserId.GetClient().Langue)
        {
            session.SendWhisper(string.Format(WibboEnvironment.GetLanguageManager().TryGetValue("cmd.authorized.langue.user", session.Langue), roomUserByUserId.GetClient().Langue));
            return;
        }

        if (!int.TryParse(parameters[2], out var effectId))
        {
            return;
        }

        if (!WibboEnvironment.GetGame().GetEffectManager().HaveEffect(effectId, session.GetUser().HasPermission("perm_god")))
        {
            return;
        }

        roomUserByUserId.ApplyEffect(effectId);
    }
}
