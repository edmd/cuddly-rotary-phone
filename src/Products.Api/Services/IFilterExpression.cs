namespace Products.Api.Services
{
    internal interface IFilterExpression
    {
        IReadOnlyCollection<IFilter> ToFilters();
    }
}
