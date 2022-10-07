namespace WibboEmulator.Communication.Packets.Incoming.Rooms.AI.Pets;
using WibboEmulator.Communication.Packets.Outgoing.Rooms.AI.Pets;
using WibboEmulator.Games.GameClients;

internal class GetPetInformationEvent : IPacketEvent
{
    public double Delay => 0;

    public void Parse(GameClient session, ClientPacket packet)
    {
        if (session.GetUser() == null || session.GetUser().CurrentRoom == null)
        {
            return;
        }

        var petId = packet.PopInt();

        if (!session.GetUser().CurrentRoom.GetRoomUserManager().TryGetPet(petId, out var pet))
        {
            var user = session.GetUser().CurrentRoom.GetRoomUserManager().GetRoomUserByUserId(petId);
            if (user == null)
            {
                return;
            }

            if (user.GetClient() == null || user.GetClient().GetUser() == null)
            {
                return;
            }

            session.SendPacket(new PetInformationComposer(user.GetClient().GetUser()));
            return;
        }

        if (pet.RoomId != session.GetUser().CurrentRoomId || pet.PetData == null)
        {
            return;
        }

        session.SendPacket(new PetInformationComposer(pet.PetData, pet.RidingHorse));
    }
}
