using Wibbo.Communication.Packets.Outgoing.Groups;
using Wibbo.Communication.Packets.Outgoing.Rooms.Permissions;
using Wibbo.Game.Clients;using Wibbo.Game.Groups;using Wibbo.Game.Rooms;
using Wibbo.Game.Users;namespace Wibbo.Communication.Packets.Incoming.Structure{    internal class TakeAdminRightsEvent : IPacketEvent    {
        public double Delay => 100;

        public void Parse(Client Session, ClientPacket Packet)        {            int GroupId = Packet.PopInt();
            int UserId = Packet.PopInt();

            if (!WibboEnvironment.GetGame().GetGroupManager().TryGetGroup(GroupId, out Group Group))
            {
                return;
            }

            if (Session.GetUser().Id != Group.CreatorId || !Group.IsMember(UserId))
            {
                return;
            }

            User user = WibboEnvironment.GetUserById(UserId);
            if (user == null)
            {
                return;
            }

            Group.TakeAdmin(UserId);

            if (WibboEnvironment.GetGame().GetRoomManager().TryGetRoom(Group.RoomId, out Room Room))
            {
                RoomUser User = Room.GetRoomUserManager().GetRoomUserByUserId(UserId);
                if (User != null)
                {
                    if (User.Statusses.ContainsKey("flatctrl 3"))
                    {
                        User.RemoveStatus("flatctrl 3");
                    }

                    User.UpdateNeeded = true;
                    if (User.GetClient() != null)
                    {
                        User.GetClient().SendPacket(new YouAreControllerComposer(0));
                    }
                }
            }

            Session.SendPacket(new GroupMemberUpdatedComposer(GroupId, user, 2));        }    }}