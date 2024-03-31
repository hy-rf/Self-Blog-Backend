namespace BBS.Models
{
    public class JsonBody
    {
        public bool Success { get; set; }
        public Object? Payload { get; set; }
        public string? Message { get; set; }
        public static JsonBody CreateResponse(bool success, string? message)
        {
            return new JsonBody
            {
                Success = success,
                Message = message
            };
        }
        public static JsonBody CreateResponse(bool success, Object? payload, string? message)
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
