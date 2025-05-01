namespace Scraper;

public class ProductDto
{
    public ProductDto()
    {
        this.Categories = new HashSet<string>();
    }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal? Weight { get; set; }

    public string? Price { get; set; }

    public string? ProductCode { get; set; }

    public string? Brand { get; set; }

    public string? Origin { get; set; }

    public string ImageUrl { get; set; } = string.Empty ;

    public IEnumerable<string> Categories { get; set; }
}


//var addresses = new List<string>
//{
//    $"https://www.kolichka.bg/c1-hlyab-i-testeni",
//    //$"https://www.kolichka.bg/c5736-plodove-i-zelencuci",
//    //$"https://www.kolichka.bg/c511-meso-i-riba",
//    //$"https://www.kolichka.bg/c6831-kolbasi-i-delikatesi",
//    //$"https://www.kolichka.bg/c6096-mlecni-i-yajca",
//    //$"https://www.kolichka.bg/c4971-zamrazeni-hrani",
//    //$"https://www.kolichka.bg/c2971-paketirani-hrani",
//    //$"https://www.kolichka.bg/c201-napitki",
//    //$"https://www.kolichka.bg/c6871-bio-i-specializirani",
//    //$"https://www.kolichka.bg/c2241-kozmetika",
//    //$"https://www.kolichka.bg/c6841-za-doma",
//    //$"https://www.kolichka.bg/c1201-za-bebeto",
//    //$"https://www.kolichka.bg/c4816-domasni-lyubimci",
//};