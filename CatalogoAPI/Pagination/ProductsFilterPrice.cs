namespace CatalogoAPI.Pagination;

public class ProductsFilterPrice : QueryStringParameters
{
    public decimal? Price { get; set; }
    public string? PriceFilter { get; set; }
}