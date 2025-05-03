using AngleSharp.Dom;
using Microsoft.Playwright;
using Newtonsoft.Json;
using Scraper;
using System.Diagnostics;
using System.Net;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

var addresses = new List<string>
{
    @"https://www.kolichka.bg/c1-hlyab-i-testeni",
    @"https://www.kolichka.bg/c5736-plodove-i-zelencuci",
    @"https://www.kolichka.bg/c511-meso-i-riba",
    @"https://www.kolichka.bg/c6831-kolbasi-i-delikatesi",
    @"https://www.kolichka.bg/c6096-mlecni-i-yajca",
    @"https://www.kolichka.bg/c4971-zamrazeni-hrani",
    @"https://www.kolichka.bg/c2971-paketirani-hrani",
    @"https://www.kolichka.bg/c201-napitki",
    @"https://www.kolichka.bg/c6871-bio-i-specializirani",
    @"https://www.kolichka.bg/c2241-kozmetika",
    @"https://www.kolichka.bg/c6841-za-doma",
    @"https://www.kolichka.bg/c1201-za-bebeto",
    @"https://www.kolichka.bg/c4816-domasni-lyubimci",
};

using var playwright = await Playwright.CreateAsync();
var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true });
var page = await browser.NewPageAsync();


for (int i = 8; i < addresses.Count; i++)
{
    string fileName = addresses[i].Split('/').Last();
    await ScrapeCategoriesAsync(addresses[i], fileName);
}
    
async Task ScrapeCategoriesAsync(string address,  string fileName)
{
    var productsInCategory = new List<ProductDto>();

    await page.GotoAsync(address);
    await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

    int maxScrolls = 15;
    int scrollCount = 0;

    List<string> productUrls = new List<string>();

    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();

    while (scrollCount < maxScrolls)
    {
        var productElements = await page.QuerySelectorAllAsync("a[data-tid='product-box__name']");
        scrollCount++;

        Console.WriteLine($"Scroll {scrollCount} — loaded {productElements.Count} products...");

        await page.EvaluateAsync(@"window.scrollBy(0, window.innerHeight);");
        await Task.Delay(2000); // Wait longer to ensure content loads
    }

    var finalElements = await page.QuerySelectorAllAsync("a[data-tid='product-box__name']");
    foreach (var product in finalElements)
    {
        string? href = await product.GetAttributeAsync("href");
        if (!string.IsNullOrWhiteSpace(href))
            productUrls.Add($"https://www.kolichka.bg{href}#productDescription");
    }

    Console.WriteLine(string.Join("\n", productUrls));
    Console.WriteLine(productUrls.Count);

    // For control by scraping
    int count = 0;

    foreach (var url in productUrls)
    {
        var product = await ScrapeProductDetailsAsync(url);
        if (product != null)
        {
            productsInCategory.Add(product);
            Console.WriteLine($"Scraped [{count++}]: {product.Name}");
        }
    }

    stopwatch.Stop();
    var timeInfo = $"{stopwatch.Elapsed.ToString()} --> {productUrls.Count} Products";


    string jsonFile = JsonConvert.SerializeObject(productsInCategory, Formatting.Indented);
    
    string path = $@"..\..\..\{fileName}.json";
    string pathTime = $@"..\..\..\{fileName}_time.json";
    
    File.WriteAllText(path, jsonFile);
    File.WriteAllText(pathTime, timeInfo);
}

async Task<ProductDto?> ScrapeProductDetailsAsync(string url)
{
    using var playwright = await Playwright.CreateAsync();
    await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true });
    var page = await browser.NewPageAsync();

    try
    {
        await page.GotoAsync(url);
        await page.WaitForSelectorAsync("h1");

        string GetText(string selector) =>
            page.QuerySelectorAsync(selector).Result?.InnerTextAsync().Result ?? string.Empty;

        string? GetAttribute(string selector, string attribute) =>
            page.QuerySelectorAsync(selector).Result?.GetAttributeAsync(attribute).Result;

        var name = GetText("h1");

        var imageSelector = @"#kosik-app2 > div:nth-child(2) > div > article > 
                        div.d-flex.mt-4.flex-wrap > aside > div:nth-child(1) > div > picture > img";
        var imageSrcset = GetAttribute(imageSelector, "srcset") ?? "";
        var imageUrl = imageSrcset.Split(", ").FirstOrDefault() ?? string.Empty;

        var description = GetText(@"#kosik-app2 > div:nth-child(2) > div > article > div.mt-2 >" +
                                  " div:nth-child(2) > div > div:nth-child(1) > div:nth-child(1) > dl > dd > span > p");

        var priceText = GetText(@"#kosik-app2 > div:nth-child(2) > div > article > div.d-flex.mt-4.flex-wrap " +
                                "> div > div.product-header-box.row.no-gutters.my-2.product-header-box--radius.border-neutrals" +
                                "-10.border.p-3 > div:nth-child(1) > strong");
        string numeric = new string(priceText.TakeWhile(c => char.IsDigit(c) || c == ',' || c == '.').ToArray());
        numeric = numeric.Replace(',', '.');
        decimal? price = decimal.TryParse(numeric, System.Globalization.NumberStyles.Any,
                         System.Globalization.CultureInfo.InvariantCulture, out var parsed) ? parsed : null;

        var brand = GetText("#kosik-app2 > div:nth-child(2) > div > article > div.d-flex.mt-4.flex-wrap > div > div:nth-child(5) > p > a");
        var origin = GetText(@"#kosik-app2 dl > dd > span");
        var packaging = GetText(@"#kosik-app2 > div:nth-child(2) > div > article > div.d-flex.mt-4.flex-wrap > div 
                                > div.product-tag-row.d-flex.justify-content-between > span");

        string[] categorySelectors =
        {
            @"#kosik-app2 nav > span:nth-child(3) > a > span",
            @"#kosik-app2 nav > span:nth-child(5) > a > span",
            @"#kosik-app2 nav > span:nth-child(7) > a > span"
        };

        var categories = categorySelectors
            .Select(sel => GetText(sel))
            .Where(text => !string.IsNullOrWhiteSpace(text))
            .ToList();

        return new ProductDto
        {
            Name = name,
            ImageUrl = imageUrl,
            Description = description,
            Price = price,
            Brand = brand,
            Origin = origin,
            Packaging = packaging,
            Categories = categories,
            OriginalUrl = url
        };
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to scrape: {url} \nError: {ex.Message}");
        return null;
    }
}