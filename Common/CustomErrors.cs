namespace dotnet_simple_bank.Common
{
    public class CustomErrors
    {
        public int Code { get; set; }
        public string Error { get; set; } = string.Empty;

        public static CustomErrors BadRequest(string message)
        {
            return new CustomErrors
            {
                Code = 400,
                Error = message
            };
        }
        public static CustomErrors Unauthorized(string message)
        {
            return new CustomErrors
            {
                Code = 401,
                Error = message
            };
        }

        public static CustomErrors NotFound(string message)
        {
            return new CustomErrors
            {
                Code = 404,
                Error = message
            };
        }

        public static CustomErrors InternalServerError(string message)
        {
            return new CustomErrors
            {
                Code = 500,
                Error = message
            };
        }
    }
}
