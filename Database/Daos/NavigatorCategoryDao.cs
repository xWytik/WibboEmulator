using System.Data;
using Butterfly.Database;
using Butterfly.Database.Interfaces;

namespace Butterfly.Database.Daos
{
    class NavigatorCategoryDao
    {
        internal static DataTable GetAll(IQueryAdapter dbClient)
        {
            dbClient.SetQuery("SELECT * FROM navigator_categories ORDER BY id ASC");
            return dbClient.GetTable();
        }
    }
}