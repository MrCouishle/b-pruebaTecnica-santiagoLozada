namespace DTOs
{
    public class BaseResponse
    {
        public bool Success { get; set; } = true;
        public string? Message { get; set; }
        public string? Error { get; set; }
        public string Type { get; set; } = "info";
        public object? Data { get; set; }
        public object? ListData { get; set; }
    }

    public class Response<T> : BaseResponse
    {
        public new T? Data { get; set; }
    }
}
