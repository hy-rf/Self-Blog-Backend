namespace BBS.Common
{
    public class JsonBody
    {
        public bool Success { get; set; }
        public object? Payload { get; set; }
        public string? Message { get; set; }
        public static JsonBody CreateResponse(bool success, string? message)
        {
            return new JsonBody
            {
                Success = success,
                Message = message
            };
        }
        public static JsonBody CreateResponse(bool success, object? payload, string? message)
        {
            return new JsonBody
            {
                Success = success,
                Payload = payload,
                Message = message
            };
        }

    }
}
