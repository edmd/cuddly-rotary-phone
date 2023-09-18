namespace Products.Api.Services
{
    public interface IPaginationParameters
    {
        int? Count { get; set; }
        int? StartIndex { get; set; }
    }
}
