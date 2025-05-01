using Microsoft.Playwright;
using Newtonsoft.Json;
using Scraper;
using File = System.IO.File;

var allProducts = new List<ProductDto>();

using var playwright = await Playwright.CreateAsync();
var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true });
var page = await browser.NewPageAsync();

//await page.GotoAsync("https://www.kolichka.bg/c1-hlyab-i-testeni");
//await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

//int maxScrolls = 15; 
//int scrollCount = 0;

//List<string> productUrls = new List<string>();

//while (scrollCount < maxScrolls)
//{
//    var productElements = await page.QuerySelectorAllAsync("a[data-tid='product-box__name']");
//    foreach (var product in productElements)
//    {
//        string? href = await product.GetAttributeAsync("href");
//        if (href  != null)
//        {
//            productUrls.Add($"https://www.kolichka.bg{href}#productDescription");
//        }
//    }

//    scrollCount++;

//    Console.WriteLine($"Scroll {scrollCount} — loaded {productElements.Count} products...");

//    await page.EvaluateAsync(@"window.scrollBy(0, window.innerHeight);");
//    await Task.Delay(2000); // Wait longer to ensure content loads
//}

//Console.WriteLine(string.Join("\n",productUrls));


string url = @"https://www.kolichka.bg/p172941-frenski-kroasan-s-maslo-2h52gr#productDescription";
var product = await ScrapeProductDetailsAsync(url);
//await page.GotoAsync("https://www.kolichka.bg/c1-hlyab-i-testeni");
//await page.WaitForLoadStateAsync(LoadState.NetworkIdle);



//var something = "dshdjhsjdjsk";
//string jsonFile = JsonConvert.SerializeObject(something, Formatting.Indented);
//string path = @"..\..\..\something.json";
//File.WriteAllText(path, jsonFile);

async Task<ProductDto?> ScrapeProductDetailsAsync(string url)
{
    using var playwright = await Playwright.CreateAsync();
    await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
    var page = await browser.NewPageAsync();

    await page.GotoAsync(url);

    await page.WaitForSelectorAsync("h1");

    // Extract product name
    var name = await page.InnerTextAsync("h1");

    // Extract image URL
    var imageElement = await page.QuerySelectorAsync("#kosik-app2 > div:nth-child(2) > div > article > div.d-flex.mt-4.flex-wrap > aside > div:nth-child(1) > div > picture > img");
    var imageUrl = imageElement != null ? await imageElement.GetAttributeAsync("srcset") : string.Empty;

    // Extract description
    var descriptionElement = await page.QuerySelectorAsync("#kosik-app2 > div:nth-child(2) > div > article > div.mt-2 > div:nth-child(2) > div > div:nth-child(1) > div:nth-child(1) > dl > dd > span > p");
    var description = descriptionElement != null ? await descriptionElement.InnerTextAsync() : string.Empty;

    // Extract price
    var priceElement = await page.QuerySelectorAsync("#kosik-app2 > div:nth-child(2) > div > article > div.d-flex.mt-4.flex-wrap > div > div.product-header-box.row.no-gutters.my-2.product-header-box--radius.border-neutrals-10.border.p-3 > div:nth-child(1) > strong");
    var price = priceElement != null ? await priceElement.InnerTextAsync() : string.Empty;

    return new ProductDto
    {
        Name = name,
        ImageUrl = imageUrl,
        Description = description,
        Price = price
    };
}