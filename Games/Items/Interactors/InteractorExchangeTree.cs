namespace WibboEmulator.Games.Items.Interactors;

using WibboEmulator.Communication.Packets.Outgoing.Inventory.Purse;
using WibboEmulator.Database.Daos.Item;
using WibboEmulator.Database.Daos.User;
using WibboEmulator.Games.GameClients;

public class InteractorExchangeTree : FurniInteractor
{
    private bool _haveReward;

    public override void OnPlace(GameClient session, Item item) => item.ReqUpdate(60);

    public override void OnRemove(GameClient session, Item item)
    {
    }

    public override void OnTrigger(GameClient session, Item item, int request, bool userHasRights, bool reverse)
    {
        if (session == null || this._haveReward || !userHasRights)
        {
            return;
        }

        var room = item.GetRoom();

        if (room == null || !room.CheckRights(session, true))
        {
            return;
        }

        var roomUser = item.GetRoom().RoomUserManager.GetRoomUserByUserId(session.User.Id);

        if (roomUser == null)
        {
            return;
        }

        var days = 31;
        switch (item.Data.InteractionType)
        {
            case InteractionType.EXCHANGE_TREE:
                days = 3;
                break;
            case InteractionType.EXCHANGE_TREE_CLASSIC:
                days = 7;
                break;
            case InteractionType.EXCHANGE_TREE_EPIC:
                days = 14;
                break;
            case InteractionType.EXCHANGE_TREE_LEGEND:
                days = 31;
                break;
        }

        var expireSeconds = days * 24 * 60 * 60;

        _ = int.TryParse(item.ExtraData, out var activateTime);

        var timeLeft = DateTime.UnixEpoch.AddSeconds(activateTime + expireSeconds) - DateTimeOffset.UtcNow;

        if (timeLeft.TotalSeconds > 0)
        {
            roomUser.SendWhisperChat($"Il reste encore {timeLeft.Days} jours, {timeLeft.Hours} heures et {timeLeft.Minutes} minutes avant que votre plante ne soit prête pour la récolte.");
        }
        else if (timeLeft.TotalSeconds <= 0)
        {
            this._haveReward = true;

            var rewards = 0;
            switch (item.Data.InteractionType)
            {
                case InteractionType.EXCHANGE_TREE:
                    rewards = WibboEnvironment.GetRandomNumber(1000, 1050);
                    break;
                case InteractionType.EXCHANGE_TREE_CLASSIC:
                    rewards = WibboEnvironment.GetRandomNumber(2000, 2200);
                    break;
                case InteractionType.EXCHANGE_TREE_EPIC:
                    rewards = WibboEnvironment.GetRandomNumber(3000, 3450);
                    break;
                case InteractionType.EXCHANGE_TREE_LEGEND:
                    rewards = WibboEnvironment.GetRandomNumber(4000, 4800);
                    break;
            }

            using var dbClient = WibboEnvironment.GetDatabaseManager().GetQueryReactor();
            ItemDao.DeleteById(dbClient, item.Id);

            room.RoomItemHandling.RemoveFurniture(null, item.Id);

            session.User.WibboPoints += rewards;
            session.SendPacket(new ActivityPointNotificationComposer(session.User.WibboPoints, 0, 105));

            UserDao.UpdateAddPoints(dbClient, session.User.Id, rewards);

            roomUser.SendWhisperChat($"Félicitations ! Vous avez obtenu {rewards} WibboPoints pour votre récolte réussie.");
        }
    }

    public override void OnTick(Item item)
    {
        item.UpdateCounter = 60;
        item.UpdateState(false);
    }
}
