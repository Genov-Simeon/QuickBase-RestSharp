using QuickBaseApi.Client.Enums;
using QuickBaseApi.Client.Models;
using QuickBaseApi.Client.Utils;

namespace QuickBaseApi.Client.Factories
{
    public static class DeleteRecordFactory
    {
        public static DeleteRecordModel DeleteByFieldId(string tableId, object id, string fieldId = "3", QueryOperator op = QueryOperator.EX)
        {
            var whereClause = BuildWhereClause(fieldId, id, op);

            return new DeleteRecordModel
            {
                From = tableId,
                Where = whereClause
            };
        }

        public static DeleteRecordModel DeleteByCondition(string tableId, List<QuickBaseFieldModel> fields, Func<QuickBaseFieldModel, bool> matchField, object value, QueryOperator op = QueryOperator.EX)
        {
            var fieldId = FieldHelper.GetFieldId(fields, matchField);

            if (fieldId == null)
                throw new ArgumentException("No matching field found for the given condition.");

            var where = BuildWhereClause(fieldId, value, op);

            return new DeleteRecordModel
            {
                From = tableId,
                Where = where
            };
        }

        public static string BuildWhereClause(string fieldId, object value, QueryOperator op = QueryOperator.EX)
        {
            string? formattedValue = value is string ? $"'{value}'" : value.ToString();

            return $"{{{fieldId}.{op}.{formattedValue}}}";
        }
    }
}
