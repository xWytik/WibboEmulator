namespace WibboEmulator.Communication.Packets.Incoming.Rooms.Furni;
using WibboEmulator.Games.GameClients;
using WibboEmulator.Games.Items;

internal sealed class SetMannequinFigureEvent : IPacketEvent
{
    public double Delay => 250;

    public void Parse(GameClient session, ClientPacket packet)
    {
        var itemId = packet.PopInt();

        if (!WibboEnvironment.GetGame().GetRoomManager().TryGetRoom(session.User.CurrentRoomId, out var room))
        {
            return;
        }

        if (!room.CheckRights(session, true))
        {
            return;
        }

        var roomItem = room.RoomItemHandling.GetItem(itemId);
        if (roomItem == null || roomItem.GetBaseItem().InteractionType != InteractionType.MANNEQUIN)
        {
            return;
        }

        var allowedParts = new List<string> { "ha", "he", "ea", "ch", "fa", "cp", "lg", "cc", "ca", "sh", "wa" };
        var look = string.Join(".", session.User.Look.Split('.').Where(part => allowedParts.Contains(part.Split('-')[0])));
        var stuff = roomItem.ExtraData.Split(new char[1] { ';' });
        var name = "";

        if (stuff.Length >= 3)
        {
            name = stuff[2];
        }

        roomItem.ExtraData = session.User.Gender + ";" + look + ";" + name;
        roomItem.UpdateState();
    }
}
