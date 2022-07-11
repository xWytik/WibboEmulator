using WibboEmulator.Communication.Packets.Outgoing.Help;
using WibboEmulator.Game.Clients;
using WibboEmulator.Game.Rooms;

namespace WibboEmulator.Communication.Packets.Incoming.Guide
{
    internal class GuideInviteToRoomEvent : IPacketEvent
    {
        public double Delay => 0;

        public void Parse(Client Session, ClientPacket Packet)
        {
            Client requester = WibboEnvironment.GetGame().GetClientManager().GetClientByUserID(Session.GetUser().GuideOtherUserId);
            if (requester == null)
            {
                return;
            }

            Room room = Session.GetUser().CurrentRoom;

            if (room == null)
            {
                requester.SendPacket(new OnGuideSessionInvitedToGuideRoomComposer(0, ""));
                Session.SendPacket(new OnGuideSessionInvitedToGuideRoomComposer(0, ""));
            }
            else
            {
                requester.SendPacket(new OnGuideSessionInvitedToGuideRoomComposer(room.Id, room.RoomData.Name));
                Session.SendPacket(new OnGuideSessionInvitedToGuideRoomComposer(room.Id, room.RoomData.Name));
            }
        }
    }
}