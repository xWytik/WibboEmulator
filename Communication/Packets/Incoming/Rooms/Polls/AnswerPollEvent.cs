namespace WibboEmulator.Communication.Packets.Incoming.Rooms.Polls;
using WibboEmulator.Communication.Packets.Outgoing.Rooms.Polls;
using WibboEmulator.Games.GameClients;

internal class AnswerPollEvent : IPacketEvent
{
    public double Delay => 250;

    public void Parse(GameClient session, ClientPacket packet)
    {
        if (!WibboEnvironment.GetGame().GetRoomManager().TryGetRoom(session.GetUser().CurrentRoomId, out var room))
        {
            return;
        }

        var user = room.GetRoomUserManager().GetRoomUserByUserId(session.GetUser().Id);
        if (user == null)
        {
            return;
        }

        var id = packet.PopInt();
        var questionId = packet.PopInt();

        var count = packet.PopInt();//Count

        var value = "0";
        for (var i = 0; i < count; i++)
        {
            value = packet.PopString();
        }

        value = value is not "0" and not "1" ? "0" : value;

        if (value == "0")
        {
            room.VotedNoCount++;
        }
        else
        {
            room.VotedYesCount++;
        }

        room.SendPacket(new QuestionAnsweredComposer(session.GetUser().Id, value, room.VotedNoCount, room.VotedYesCount));

        var wiredCode = value == "0" ? "QUESTION_NO" : "QUESTION_YES";
        if (room.AllowsShous(user, wiredCode))
        {
            user.SendWhisperChat(wiredCode, false);
        }
    }
}
