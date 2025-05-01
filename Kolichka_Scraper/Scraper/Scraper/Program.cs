using AngleSharp.Dom;
using Microsoft.Playwright;
using Newtonsoft.Json;
using Scraper;
using System.Diagnostics;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

var allProducts = new List<ProductDto>();

using var playwright = await Playwright.CreateAsync();
var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true });
var page = await browser.NewPageAsync();


var productsByCategoryUrl = @"https://www.kolichka.bg/c1-hlyab-i-testeni";
string fileName = productsByCategoryUrl.Split('/').Last();

await page.GotoAsync(productsByCategoryUrl);
await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

int maxScrolls = 15;
int scrollCount = 0;

List<string> productUrls = new List<string>();

Stopwatch stopwatch = new Stopwatch();
stopwatch.Start();

while (scrollCount < maxScrolls)
{
    var productElements = await page.QuerySelectorAllAsync("a[data-tid='product-box__name']");
    foreach (var product in productElements)
    {
        string? href = await product.GetAttributeAsync("href");
        if (href != null)
        {
            productUrls.Add($"https://www.kolichka.bg{href}#productDescription");
        }
    }

    scrollCount++;

    Console.WriteLine($"Scroll {scrollCount} — loaded {productElements.Count} products...");

    await page.EvaluateAsync(@"window.scrollBy(0, window.innerHeight);");
    await Task.Delay(2000); // Wait longer to ensure content loads
}

Console.WriteLine(string.Join("\n", productUrls));

foreach (var productUrl in productUrls)
{
    var productDetails = await ScrapeProductDetailsAsync(productUrl);
    if (productDetails != null)
    {
        allProducts.Add(productDetails);
    }
}

stopwatch.Stop();
TimeSpan time = stopwatch.Elapsed;
var timeInfo = $"{time.ToString()} --> {productUrls.Count} Products";


string jsonFile = JsonConvert.SerializeObject(allProducts, Formatting.Indented);
string path = $@"..\..\..\{fileName}.json";
string pathTime = $@"..\..\..\{fileName}time.json";
File.WriteAllText(path, jsonFile);
File.WriteAllText(pathTime, timeInfo);

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
    var imageSelector = @"#kosik-app2 > div:nth-child(2) > div > article > 
                        div.d-flex.mt-4.flex-wrap > aside > div:nth-child(1) > div > picture > img";

    var imageElement = await page.QuerySelectorAsync(imageSelector);
    var imageUrlRow = imageElement != null ? await imageElement.GetAttributeAsync("srcset") : string.Empty;
    var imageUrl = imageUrlRow != null ? imageUrlRow!.Split(", ").FirstOrDefault() : string.Empty;


    // Extract description
    var descriptionSelector = @"#kosik-app2 > div:nth-child(2) > div > article > div.mt-2 >" +
                             " div:nth-child(2) > div > div:nth-child(1) > div:nth-child(1) > dl > dd > span > p";

    var descriptionElement = await page.QuerySelectorAsync(descriptionSelector);
    var description = descriptionElement != null ? await descriptionElement.InnerTextAsync() : string.Empty;

    // Extract price
    var priceSelector = @"#kosik-app2 > div:nth-child(2) > div > article > div.d-flex.mt-4.flex-wrap " +
                        "> div > div.product-header-box.row.no-gutters.my-2.product-header-box--radius.border-neutrals" +
                        "-10.border.p-3 > div:nth-child(1) > strong";

    var priceElement = await page.QuerySelectorAsync(priceSelector);
    var priceText = priceElement != null ? await priceElement.InnerTextAsync() : string.Empty;
    string numeric = new string(priceText.TakeWhile(c => char.IsDigit(c) || c == ',' || c == '.').ToArray());
    numeric = numeric.Replace(',', '.');
    decimal? price = decimal.TryParse(numeric, System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out var result) ? result : null;

    // Extract brand
    var brandSelector = "#kosik-app2 > div:nth-child(2) > div > article > div.d-flex.mt-4.flex-wrap > div > div:nth-child(5) > p > a";
    var brandElement = await page.QuerySelectorAsync(brandSelector);
    var brand = brandElement != null ? await brandElement.InnerTextAsync() : string.Empty;

    // Extract origin
    var originSelector = @"#kosik-app2 > div:nth-child(2) > div > article > div.d-flex.mt-4.flex-wrap 
                        > div > div:nth-child(5) > dl > dd > span";

    var originElement = await page.QuerySelectorAsync(originSelector);
    var origin = originElement != null ? await originElement.InnerTextAsync() : string.Empty;


    // Extract main category
    var mainCategorySelector = @"#kosik-app2 > div:nth-child(2) > div > article > nav > span:nth-child(3) > a > span";
    var mainCategoryElement = await page.QuerySelectorAsync(mainCategorySelector);
    var mainCategory = mainCategoryElement != null ? await mainCategoryElement.InnerTextAsync() : string.Empty;

    // Extract subcategory
    var subCategorySelector = @"#kosik-app2 > div:nth-child(2) > div > article > nav > span:nth-child(5) > a > span";
    var subCategoryElement = await page.QuerySelectorAsync(subCategorySelector);
    var subCategory = subCategoryElement != null ? await subCategoryElement.InnerTextAsync() : string.Empty;

    // Extract subSubCategory
    var subSubCategorySelector = @"#kosik-app2 > div:nth-child(2) > div > article > nav > span:nth-child(7) > a > span";
    var subSubCategoryElement = await page.QuerySelectorAsync(subSubCategorySelector);
    var subSubCategory = subSubCategoryElement != null ? await subSubCategoryElement.InnerTextAsync() : string.Empty;

    List<string> categories = new List<string>() { mainCategory, subCategory, subSubCategory }
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .ToList()!;

    return new ProductDto
    {
        Name = name,
        ImageUrl = imageUrl,
        Description = description,
        Price = price,
        Brand = brand,
        Origin = origin,
        Categories = categories,
    };
}