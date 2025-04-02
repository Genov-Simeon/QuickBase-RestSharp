using QuickBaseApi.Client.Models;

namespace QuickBaseApi.Client.Utils
{
    public static class HelperMethods
    {
        public static List<long> GetAllFieldIds(Dictionary<string, FieldValueModel> randomRecord)
        {
            var fieldsToReturn = new List<long>();

            foreach (var id in randomRecord.Keys)
            {
                fieldsToReturn.Add(long.Parse(id));
            }

            return fieldsToReturn;
        }

        public static string RandomString(int length = 10)
        {
            var random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyz123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
