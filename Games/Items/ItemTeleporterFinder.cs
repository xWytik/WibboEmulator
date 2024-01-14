namespace WibboEmulator.Games.Items;
using WibboEmulator.Database.Daos.Item;
using WibboEmulator.Games.Rooms;

public static class ItemTeleporterFinder
{
    public static int GetLinkedTele(int teleId)
    {
        using var dbClient = WibboEnvironment.GetDatabaseManager().Connection();
        var teleportId = ItemTeleportDao.GetOne(dbClient, teleId);

        return teleportId;
    }

    public static int GetTeleRoomId(int teleId, Room room)
    {
        if (room == null)
        {
            return 0;
        }

        if (room.RoomItemHandling == null)
        {
            return 0;
        }

        if (room.RoomItemHandling.GetItem(teleId) != null)
        {
            return room.Id;
        }

        using var dbClient = WibboEnvironment.GetDatabaseManager().Connection();
        var roomId = ItemDao.GetOneRoomId(dbClient, teleId);

        return roomId;
    }

    public static bool IsTeleLinked(int teleId, Room room)
    {
        var linkedTele = GetLinkedTele(teleId);
        if (linkedTele == 0)
        {
            return false;
        }

        var roomItem = room.RoomItemHandling.GetItem(linkedTele);
        return (roomItem != null && (roomItem.GetBaseItem().InteractionType == InteractionType.TELEPORT || roomItem.GetBaseItem().InteractionType == InteractionType.TELEPORT_ARROW)) || GetTeleRoomId(linkedTele, room) != 0;
    }
}
