using QuickBaseApi.Client.Enums;
using QuickBaseApi.Client.Models;

namespace QuickBaseApi.Client.Factories
{
    public static class DeleteRecordFactory
    {
        public static DeleteRecordModel DeleteRecord(string tableId, object fieldId, QueryOperator op, object value)
        {
            string? formattedfieldId = fieldId is string ? $"'{fieldId}'" : fieldId.ToString();
            string? formattedValue = value is string ? $"'{value}'" : value.ToString();

            string whereClause = $"{{{formattedfieldId}.{op}.{formattedValue}}}";

            return new DeleteRecordModel
            {
                From = tableId,
                Where = whereClause
            };
        }
    }
}
