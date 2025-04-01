//namespace QuickBaseApi.Client.Factories
//{
//    public static class QuickbaseFieldFactory
//    {
//        private static readonly Random _rand = new();

//        public static Dictionary<string, FieldValue> CreateAllRandomFields()
//        {
//            return new Dictionary<string, FieldValue>
//            {
//                ["text"] = Text(RandomString()),
//                ["richText"] = RichText(RandomString()),
//                ["multiLine"] = MultiLineText(RandomString(50)),
//                ["multiChoice"] = MultipleChoice("Choice A"),
//                ["multiSelect"] = MultiSelect(new[] { "Option 1", "Option 2" }),
//                ["email"] = Email("example@quickbase.com"),
//                ["url"] = Url("https://quickbase.com"),
//                ["numeric"] = Numeric(_rand.Next(100)),
//                ["percent"] = Percent(0.75),
//                ["rating"] = Rating(4),
//                ["currency"] = Currency(99.99m),
//                ["duration"] = Duration(TimeSpan.FromDays(15)),
//                ["date"] = Date(DateTime.Today),
//                ["dateTime"] = DateTimeField(DateTime.UtcNow),
//                ["timeOfDay"] = TimeOfDay(DateTime.Now.TimeOfDay),
//                ["checkbox"] = Checkbox(_rand.Next(2) == 1),
//                ["phone"] = Phone("123-456-7890 x123"),
//                ["user"] = User("123456.ab1s"),
//                ["multiUser"] = MultiUser(new[] { "123456.ab1s", "789654.vc2s" }),
//                ["file"] = FileAttachment("test.txt", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Hello file")))
//            };
//        }

//        public static FieldValue Text(string value) => new() { Value = value };
//        public static FieldValue RichText(string value) => new() { Value = value };
//        public static FieldValue MultiLineText(string value) => new() { Value = value };
//        public static FieldValue MultipleChoice(string value) => new() { Value = value };
//        public static FieldValue MultiSelect(IEnumerable<string> values) => new() { Value = values };
//        public static FieldValue Email(string value) => new() { Value = value };
//        public static FieldValue Url(string value) => new() { Value = value };
//        public static FieldValue Numeric(int value) => new() { Value = value };
//        public static FieldValue Percent(double value) => new() { Value = value };
//        public static FieldValue Rating(int value) => new() { Value = value };
//        public static FieldValue Currency(decimal value) => new() { Value = value };
//        public static FieldValue Duration(TimeSpan duration) => new() { Value = (long)duration.TotalMilliseconds };
//        public static FieldValue Date(DateTime date) => new() { Value = date.ToString("yyyy-MM-dd") };
//        public static FieldValue DateTimeField(DateTime dateTime) => new() { Value = dateTime.ToString("yyyy-MM-ddTHH:mm:ss") };
//        public static FieldValue TimeOfDay(TimeSpan time) => new() { Value = time.ToString("hh\\:mm\\:ss") };
//        public static FieldValue Checkbox(bool value) => new() { Value = value };
//        public static FieldValue Phone(string number) => new() { Value = number };
//        public static FieldValue User(string userId) => new() { Value = new { id = userId } };
//        public static FieldValue MultiUser(IEnumerable<string> ids) => new() { Value = ids.Select(id => new { id }) };
//        public static FieldValue FileAttachment(string fileName, string base64) => new() { Value = new { fileName, data = base64 } };

//        private static string RandomString(int length = 10)
//        {
//            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
//            return new string(Enumerable.Repeat(chars, length)
//                .Select(s => s[_rand.Next(s.Length)]).ToArray());
//        }
//    }
//}
