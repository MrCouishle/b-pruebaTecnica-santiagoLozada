namespace Services
{
    public abstract class ResponseService
    {
        protected static Response<T> SuccessResponse<T>(T data, string message = "")
        {
            return new Response<T>
            {
                Success = true,
                Data = data,
                Type = MessageConst.Success,
                Message = message,
            };
        }

        protected static Response<T> ErrorResponse<T>(string message)
        {
            return new Response<T>
            {
                Success = false,
                Type = MessageConst.Error,
                Message = message,
            };
        }

        protected static Response<T> InfoResponse<T>(string message)
        {
            return new Response<T>
            {
                Success = false,
                Type = MessageConst.Info,
                Message = message,
            };
        }
    }
}
