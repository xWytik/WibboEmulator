namespace WibboEmulator.Games.Chat.Commands.User.RP;
using WibboEmulator.Games.GameClients;
using WibboEmulator.Games.Rooms;

internal class GiveMoney : IChatCommand
{
    public void Execute(GameClient session, Room room, RoomUser userRoom, string[] parameters)
    {
        if (parameters.Length != 3)
        {
            return;
        }

        if (!room.IsRoleplay)
        {
            return;
        }

        var rp = userRoom.Roleplayer;
        if (rp == null)
        {
            return;
        }

        if (rp.Dead || rp.SendPrison)
        {
            return;
        }

        var targetRoomUser = room.GetRoomUserManager().GetRoomUserByName(parameters[1].ToString());

        if (targetRoomUser == null || targetRoomUser.GetClient() == null || targetRoomUser.GetClient().GetUser() == null)
        {
            return;
        }

        if (!int.TryParse(parameters[2].ToString(), out var numberMoney))
        {
            return;
        }

        if (numberMoney <= 0)
        {
            return;
        }

        var rpTwo = targetRoomUser.Roleplayer;
        if (rpTwo == null)
        {
            return;
        }

        if (targetRoomUser.GetClient().GetUser().Id == session.GetUser().Id)
        {
            return;
        }

        if (rpTwo.Dead || rpTwo.SendPrison)
        {
            return;
        }

        if (rp.Money < numberMoney)
        {
            return;
        }

        if (!(Math.Abs(targetRoomUser.X - userRoom.X) >= 2 || Math.Abs(targetRoomUser.Y - userRoom.Y) >= 2))
        {
            rp.Money -= numberMoney;
            rpTwo.Money += numberMoney;

            rp.SendUpdate();
            rpTwo.SendUpdate();

            targetRoomUser.SendWhisperChat(string.Format(WibboEnvironment.GetLanguageManager().TryGetValue("rp.givemoney.receive", targetRoomUser.GetClient().Langue), numberMoney, userRoom.GetUsername()));

            session.SendWhisper(string.Format(WibboEnvironment.GetLanguageManager().TryGetValue("rp.givemoney.send", session.Langue), numberMoney, targetRoomUser.GetUsername()));
            userRoom.OnChat(string.Format(WibboEnvironment.GetLanguageManager().TryGetValue("rp.givemoney.send.chat", session.Langue), targetRoomUser.GetUsername()), 0, true);
        }
    }
}
