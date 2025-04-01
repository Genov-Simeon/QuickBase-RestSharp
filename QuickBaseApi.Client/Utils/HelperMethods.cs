using QuickBaseApi.Client.Models;

namespace QuickBaseApi.Client.Utils
{
    public static class HelperMethods
    {
        public static List<long> GetAllFieldIds(List<Dictionary<string, FieldValueModel>> randomRecord)
        {
            var fieldsToReturn = new List<long>();

            foreach (var id in randomRecord[0].Keys)
            {
                fieldsToReturn.Add(long.Parse(id));
            }

            return fieldsToReturn;
        }
    }
}
