using WibboEmulator.Game.Clients;
using WibboEmulator.Game.Rooms;

namespace WibboEmulator.Communication.Packets.Incoming.Structure
{
    internal class MuteUserEvent : IPacketEvent
    {
        public double Delay => 250;

        public void Parse(Client Session, ClientPacket Packet)
        {
            if (Session.GetUser() == null)
            {
                return;
            }

            Room room = WibboEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetUser().CurrentRoomId);
            if (room == null || (room.RoomData.MuteFuse != 1 || !room.CheckRights(Session)) && !room.CheckRights(Session, true))
            {
                return;
            }

            int Id = Packet.PopInt();
            int num = Packet.PopInt();

            RoomUser roomUserByUserId = room.GetRoomUserManager().GetRoomUserByUserId(Id);

            int Time = Packet.PopInt() * 60;

            if (roomUserByUserId == null || roomUserByUserId.IsBot || (room.CheckRights(roomUserByUserId.GetClient(), true) || roomUserByUserId.GetClient().GetUser().HasPermission("perm_mod") || roomUserByUserId.GetClient().GetUser().HasPermission("fuse_no_mute")))
            {
                return;
            }

            room.AddMute(Id, Time);
        }
    }
}
