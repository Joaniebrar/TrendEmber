
namespace TrendEmber.Service
{
    public class Result<T>
    {
        public Result(bool success, string message, T data) {
            Success = success;
            Message = message;
            Data = data;
        }

        public bool Success { get; }
        public string Message { get; }
        public T Data { get; }
}
}
