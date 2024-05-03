namespace WibboEmulator.Communication.RCON.Commands.User;

using WibboEmulator.Core.Language;
using WibboEmulator.Core.Settings;
using WibboEmulator.Database;
using WibboEmulator.Database.Daos.User;
using WibboEmulator.Games.GameClients;
using WibboEmulator.Games.Items;

internal sealed class AddPhotoCommand : IRCONCommand
{
    public bool TryExecute(string[] parameters)
    {
        if (parameters.Length != 3)
        {
            return false;
        }

        if (!int.TryParse(parameters[1], out var userId))
        {
            return false;
        }

        if (userId == 0)
        {
            return false;
        }

        var client = GameClientManager.GetClientByUserID(userId);
        if (client == null)
        {
            return true;
        }

        var photoId = parameters[2];

        var photoItemId = SettingsManager.GetData<int>("photo.item.id");
        if (!ItemManager.GetItem(photoItemId, out var itemData))
        {
            return true;
        }

        var time = WibboEnvironment.GetUnixTimestamp();
        var extraData = "{\"w\":\"" + "/photos/" + photoId + ".png" + "\", \"n\":\"" + client.User.Username + "\", \"s\":\"" + client.User.Id + "\", \"u\":\"" + "0" + "\", \"t\":\"" + time + "000" + "\"}";

        using var dbClient = DatabaseManager.Connection;

        var item = ItemFactory.CreateSingleItemNullable(dbClient, itemData, client.User, extraData);
        client.User.InventoryComponent.TryAddItem(item);

        UserPhotoDao.Insert(dbClient, client.User.Id, photoId, time);

        client.SendNotification(LanguageManager.TryGetValue("notif.buyphoto.valide", client.Language));

        return true;
    }
}
