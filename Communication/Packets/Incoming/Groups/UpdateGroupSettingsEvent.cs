using WibboEmulator.Communication.Packets.Outgoing.Groups;
using WibboEmulator.Communication.Packets.Outgoing.Rooms.Permissions;
using WibboEmulator.Database.Daos;
using WibboEmulator.Database.Interfaces;
using WibboEmulator.Game.Clients;
using WibboEmulator.Game.Groups;
using WibboEmulator.Game.Rooms;

namespace WibboEmulator.Communication.Packets.Incoming.Structure
{
    internal class UpdateGroupSettingsEvent : IPacketEvent
    {
        public double Delay => 500;

        public void Parse(Client Session, ClientPacket Packet)
        {
            int GroupId = Packet.PopInt();

            if (!WibboEnvironment.GetGame().GetGroupManager().TryGetGroup(GroupId, out Group Group))
            {
                return;
            }

            if (Group.CreatorId != Session.GetUser().Id)
            {
                return;
            }

            int Type = Packet.PopInt();
            int FurniOptions = Packet.PopInt();

            switch (Type)
            {
                default:
                case 0:
                    Group.GroupType = GroupType.OPEN;
                    break;
                case 1:
                    Group.GroupType = GroupType.LOCKED;
                    break;
                case 2:
                    Group.GroupType = GroupType.PRIVATE;
                    break;
            }

            if (Group.GroupType != GroupType.LOCKED)
            {
                if (Group.GetRequests.Count > 0)
                {
                    foreach (int UserId in Group.GetRequests.ToList())
                    {
                        Group.HandleRequest(UserId, false);
                    }

                    Group.ClearRequests();
                }
            }

            using (IQueryAdapter dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                GuildDao.UpdateStateAndDeco(dbClient, Group.Id, (Group.GroupType == GroupType.OPEN ? 0 : Group.GroupType == GroupType.LOCKED ? 1 : 2), FurniOptions);
            }

            Group.AdminOnlyDeco = FurniOptions;

            if (!WibboEnvironment.GetGame().GetRoomManager().TryGetRoom(Group.RoomId, out Room Room))
            {
                return;
            }

            foreach (RoomUser User in Room.GetRoomUserManager().GetRoomUsers().ToList())
            {
                if (Room.RoomData.OwnerId == User.UserId || Group.IsAdmin(User.UserId) || !Group.IsMember(User.UserId))
                {
                    continue;
                }

                if (FurniOptions == 1)
                {
                    User.RemoveStatus("flatctrl");
                    User.UpdateNeeded = true;

                    User.GetClient().SendPacket(new YouAreControllerComposer(0));
                }
                else if (FurniOptions == 0 && !User.ContainStatus("flatctrl"))
                {
                    User.SetStatus("flatctrl", "1");
                    User.UpdateNeeded = true;

                    User.GetClient().SendPacket(new YouAreControllerComposer(1));
                }
            }

            Session.SendPacket(new GroupInfoComposer(Group, Session));
        }
    }
}
