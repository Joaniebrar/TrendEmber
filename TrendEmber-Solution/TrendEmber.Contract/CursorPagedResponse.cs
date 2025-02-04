
namespace TrendEmber.Contract
{
    public class CursorPagedResponse<T>
    {
        public IEnumerable<T> Data { get; }
        public string? NextCursor { get; }

        public CursorPagedResponse(IEnumerable<T> data, string? nextCursor)
        {
            Data = data;
            NextCursor = nextCursor;
        }
    }
}
