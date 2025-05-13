using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Models;
using static ReGreenShop.Application.Common.GlobalConstants;

namespace ReGreenShop.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ZTestController : ControllerBase
{
    private readonly IImageDownloader downloader;
    private readonly IStorage storage;
    private readonly IEmailSender sender;
    private readonly IPdfGenerator pdfGenerator;
    private readonly IWebHostEnvironment web;
    private readonly IData data;

    public ZTestController(IImageDownloader downloader,
                    IStorage storage, IEmailSender sender,
                    IPdfGenerator pdfGenerator, IWebHostEnvironment web,
                    IData data)
    {
        this.downloader = downloader;
        this.storage = storage;
        this.sender = sender;
        this.pdfGenerator = pdfGenerator;
        this.web = web;
        this.data = data;
    }

    [HttpGet(nameof(DownloadImage))]
    public async Task DownloadImage()
    {
        string imageUrl = @"https://static-new.kolichka.bg/k3wCdnContainerk3w-static-ne-bg-prod/images/thumbs/mg/300x200x1_mg4t5udji0to.jpg";
        string name = "test";
        var bytesExtension = await this.downloader.DownloadImageAsync(imageUrl);
        await this.storage.SaveImageAsync(bytesExtension.ImageBytes, name, bytesExtension.Extension);
    }

    [HttpGet(nameof(SendEmail))]
    public async Task SendEmail(string email)
    {
        var html = new StringBuilder();
        html.AppendLine($"<h3>Thank you for contacting us!</h3>");
        html.AppendLine($"<p>Dear Irena,</p>");
        html.AppendLine($"<p>Thanks for getting in touch! We’ve received your message and " +
            $"will get back to you as soon as we can — usually within 3 days.</p>");

        var path = Path.Combine(this.web.WebRootPath, "invoices", "invoice.pdf");
        var fileBytes = await System.IO.File.ReadAllBytesAsync(path);
        var attachment = new EmailAttachment
        {
            FileName = "invoice.pdf",
            Content = fileBytes,
            MimeType = "application/pdf"
        };
        await this.sender.SendEmailAsync(SystemEmailSender, SystemEmailSenderName, email, "Test", html.ToString(), new List<EmailAttachment> { attachment });
    }


    [HttpGet(nameof(SendEmailTemplate))]
    public async Task SendEmailTemplate(string email)
    {
        string templateId = "d-5e226b6ae4c4434cadfee761ff05ea51";
        var dynamicDta = new Dictionary<string, object>
        {
            { "name1", "John Doe" },
            { "name2", "12345" },
        };
        await this.sender.SendTemplateEmailAsync(SystemEmailSender, SystemEmailSenderName, email, templateId, dynamicDta);
    }


    [HttpPost(nameof(MakePdf))]
    public async Task MakePdf(InvoiceItem model)
    {
        var bytes = this.pdfGenerator.GenerateReceiptPdfAsync(model);
        await this.storage.SaveInvoicesAsync(bytes, "Invoice");
    }

    [HttpGet("AllcategoriesName")]
    public async Task<List<string>> AllcategoriesName()
    {
        var categorynames = await this.data.Categories.Where(x => x.Id > 831)
            .Select(x => x.NameInBulgarian)
            .ToListAsync();
        return categorynames!;
    }


    [Authorize]
    [HttpGet("RenameCategories")]
    public async Task Rename()
    {
        var categories = this.data.Categories.Where(x => x.Id > 831).ToList();

        var categoriesName = new List<(string Bulgarian, string English)>
{
    ("Прясно изпечен", "Freshly Baked"),
    ("Хляб", "Bread"),
    ("Бял хляб", "White Bread"),
    ("Пълнозърнест и многозърнест хляб", "Whole Grain and Multigrain Bread"),
    ("Ръжен и типов хляб", "Rye and Type Bread"),
    ("Питки за бургер", "Burger Buns"),
    ("Без глутен", "Gluten-Free"),
    ("Безглутенов хляб", "Gluten-Free Bread"),
    ("Тортила и лаваш", "Tortilla and Lavash"),
    ("Торти и десерти", "Cakes and Desserts"),
    ("Торти", "Cakes"),
    ("Десерти и сладки", "Desserts and Sweets"),
    ("Хляб и печива", "Bread and Pastries"),
    ("Замразено тесто", "Frozen Dough"),
    ("Кори за баница и тесто", "Banitsa Sheets and Dough"),
    ("Безглутенови кори, брашно, подправки", "Gluten-Free Sheets, Flour, Spices"),
    ("Сладки печива", "Sweet Pastries"),
    ("Замразени кроасани", "Frozen Croissants"),
    ("Замразени хлебчета", "Frozen Bread Rolls"),
    ("Замразени баници", "Frozen Banitsa"),
    ("Замразени десерти", "Frozen Desserts"),
    ("Замразени пайове и торти", "Frozen Pies and Cakes"),
    ("Сухари и крутони", "Rusks and Croutons"),
    ("Грижа", "Care"),
    ("Мокри кърпички", "Wet Wipes"),
    ("Зърнени закуски, корнфлейкс и мюсли", "Cereals, Cornflakes, and Muesli"),
    ("Зърнени закуски", "Cereals"),
    ("Био продукти за деца", "Organic Products for Children"),
    ("Био храни", "Organic Foods"),
    ("Пелени", "Diapers"),
    ("Храни", "Foods"),
    ("Пюрета", "Purees"),
    ("Адаптирано мляко", "Infant Formula"),
    ("Сокове", "Juices"),
    ("Детски сокчета", "Children's Juices"),
    ("Плодов пауч", "Fruit Pouch"),
    ("Солети, крекери, чипс", "Pretzels, Crackers, Chips"),
    ("Солети ", "Pretzels"),
    ("Вода", "Water"),
    ("Плодове", "Fruits"),
    ("Фреш и смути", "Fresh Juice and Smoothies"),
    ("Закуски и сухари", "Snacks and Rusks"),
    ("Био", "Organic"),
    ("Био за деца", "Organic for Children"),
    ("Алкохолни напитки", "Alcoholic Beverages"),
    ("Ракия", "Rakia"),
    ("Кухненски принадлежности", "Kitchen Accessories"),
    ("Чаши", "Glasses"),
    ("Бира", "Beer"),
    ("Стъкло", "Glass"),
    ("Кафе", "Coffee"),
    ("Мляно кафе", "Ground Coffee"),
    ("Плодови напитки", "Fruit Beverages"),
    ("Разтворимо кафе микс", "Instant Coffee Mix"),
    ("Кен", "Can"),
    ("Прибори", "Utensils"),
    ("Безалкохолни напитки", "Non-Alcoholic Beverages"),
    ("Газирани напитки", "Carbonated Drinks"),
    ("Ликьори", "Liqueurs"),
    ("Джин", "Gin"),
    ("Изворна и трапезна вода", "Spring and Table Water"),
    ("Минерална вода", "Mineral Water"),
    ("Газирана вода", "Sparkling Water"),
    ("Кафе на зърна", "Coffee Beans"),
    ("Веган храни", "Vegan Foods"),
    ("Веган напитки и млека", "Vegan Drinks and Milks"),
    ("Прясно и UHT мляко", "Fresh and UHT Milk"),
    ("UHT Млека", "UHT Milks"),
    ("Голяма бутилка", "Large Bottle"),
    ("Вино", "Wine"),
    ("Плодово вино", "Fruit Wine"),
    ("Бяло вино", "White Wine"),
    ("Червено вино", "Red Wine"),
    ("Пенливо вино", "Sparkling Wine"),
    ("Вино в кутия", "Boxed Wine"),
    ("100% Натурален сок", "100% Natural Juice"),
    ("Плодови сокове", "Fruit Juices"),
    ("Уиски", "Whiskey"),
    ("Узо и мастика", "Ouzo and Mastika"),
    ("Водка", "Vodka"),
    ("Чай", "Tea"),
    ("Билков чай", "Herbal Tea"),
    ("Черен чай", "Black Tea"),
    ("Плодов чай", "Fruit Tea"),
    ("Зелен чай", "Green Tea"),
    ("Енергийни и спортни напитки", "Energy and Sports Drinks"),
    ("Спортни напитки", "Sports Drinks"),
    ("Енергийни напитки", "Energy Drinks"),
    ("Сиропи", "Syrups"),
    ("Коктейлни сиропи", "Cocktail Syrups"),
    ("Био сиропи", "Organic Syrups"),
    ("Разтворими напитки", "Instant Beverages"),
    ("Шоколад", "Chocolate"),
    ("Какао", "Cocoa"),
    ("Устна хигиена", "Oral Hygiene"),
    ("Четки за зъби", "Toothbrushes"),
    ("Козметика за тяло", "Body Cosmetics"),
    ("Душ гелове", "Shower Gels"),
    ("Козметика за лице", "Facial Cosmetics"),
    ("Кремове", "Creams"),
    ("Грижа за ръце", "Hand Care"),
    ("Течен сапун", "Liquid Soap"),
    ("Лосиони за тяло", "Body Lotions"),
    ("Сапун", "Soap"),
    ("Епилация и бръснене", "Hair Removal and Shaving"),
    ("Дамска епилация", "Women's Hair Removal"),
    ("Еко козметика и почистващи препарати", "Eco Cosmetics and Cleaning Products"),
    ("Био и еко козметика", "Organic and Eco Cosmetics"),
    ("Дезодоранти и парфюми", "Deodorants and Perfumes"),
    ("Паста за зъби", "Toothpaste"),
    ("Продукти от памук", "Cotton Products"),
    ("Тампони за грим", "Makeup Pads"),
    ("Течен сапун 5 л.", "Liquid Soap 5L"),
    ("Kозметика", "Cosmetics"),
    ("Интимна грижа", "Intimate Care"),
    ("Хигиенни продукти", "Hygiene Products"),
    ("Дамски превръзки", "Sanitary Pads"),
    ("Вода за уста", "Mouthwash"),
    ("Устна хигиена за деца", "Children's Oral Hygiene"),
    ("Клечки за уши", "Cotton Swabs"),
    ("Памук", "Cotton"),
    ("Козметика за коса", "Hair Cosmetics"),
    ("Шампоани", "Shampoos"),
    ("Балсами и маски", "Conditioners and Masks"),
    ("Почистване на лице", "Facial Cleansing"),
    ("Мъжка епилация", "Men's Hair Removal"),
    ("Козметика за обувки", "Shoe Care Products"),
    ("Грижа за обувки", "Shoe Care"),
    ("Консерви", "Canned Foods"),
    ("Маслини", "Olives"),
    ("Олио, оцет и растителни масла", "Oil, Vinegar, and Vegetable Oils"),
    ("Оцет", "Vinegar"),
    ("Зеленчукови", "Vegetable"),
    ("Други растителни масла", "Other Vegetable Oils"),
    ("Кроасани и десерти", "Croissants and Desserts"),
    ("Бисквити", "Biscuits"),
    ("Варива и ориз", "Legumes and Rice"),
    ("Ориз", "Rice"),
    ("Брашно и галета", "Flour and Breadcrumbs"),
    ("Брашно за сладкиши", "Cake Flour"),
    ("Паста и макарони", "Pasta and Macaroni"),
    ("Макарони", "Macaroni"),
    ("Мед, сладка и тахани", "Honey, Jams, and Tahini"),
    ("Течен шоколад", "Liquid Chocolate"),
    ("Зимнина", "Preserves"),
    ("Кисели краставички", "Pickles"),
    ("Плодове и компоти", "Fruits and Compotes"),
    ("Месни", "Meat Products"),
    ("Риба и морски дарове", "Fish and Seafood"),
    ("Захар, сол, подправки", "Sugar, Salt, Spices"),
    ("Сладкарски продукти", "Confectionery Products"),
    ("Захар", "Sugar"),
    ("Яйца", "Eggs"),
    ("Билки и подправки", "Herbs and Spices"),
    ("Кроасани", "Croissants"),
    ("Бисквити, крекери, барчета", "Biscuits, Crackers, Bars"),
    ("Спорт и фитнес", "Sports and Fitness"),
    ("Протеинови барчета", "Protein Bars"),
    ("Халва и локум", "Halva and Turkish Delight"),
      ("Зехтин", "Olive Oil"),
    ("Олио", "Sunflower Oil"),
    ("Снакс", "Snacks"),
    ("Оризовки", "Rice Cakes"),
    ("Чипс", "Chips"),
    ("Шоколад и бонбони", "Chocolate and Candies"),
    ("Шоколадови десерти", "Chocolate Desserts"),
    ("Шоколадови бонбони", "Chocolate Candies"),
    ("Сосове", "Sauces"),
    ("Песто", "Pesto"),
    ("Соев сос", "Soy Sauce"),
    ("Сосове за паста", "Pasta Sauces"),
    ("Майонеза", "Mayonnaise"),
    ("Кетчуп", "Ketchup"),
    ("Други варива", "Other Legumes"),
    ("Киноа", "Quinoa"),
    ("Леща", "Lentils"),
    ("Паста с яйца", "Egg Pasta"),
    ("Спагети", "Spaghetti"),
    ("Фиде и кускус", "Vermicelli and Couscous"),
    ("Ньоки", "Gnocchi"),
    ("Диетичен хляб и паста", "Diet Bread and Pasta"),
    ("Бяло брашно", "White Flour"),
    ("Галета и панировки", "Breadcrumbs and Coatings"),
    ("Каперси", "Capers"),
    ("Чушки", "Peppers"),
    ("Друга зимнина", "Other Preserves"),
    ("Веган млечни продукти", "Vegan Dairy Products"),
    ("Лютеница", "Lutenitsa"),
    ("Ядки", "Nuts"),
    ("Сурови ядки", "Raw Nuts"),
    ("Печени ядки", "Roasted Nuts"),
("Аксесоари", "Accessories"),
    ("Храна за кучета", "Dog Food"),
    ("Пауч", "Pouch Food"),
    ("Суха храна за кучета", "Dry Dog Food"),
    ("Храна за котки", "Cat Food"),
    ("Консерви и пастети", "Canned Food and Pâtés"),
    ("Лакомства за кучета", "Dog Treats"),
    ("Замразена риба и морски дарове", "Frozen Fish and Seafood"),
    ("Замразени морски деликатеси", "Frozen Seafood Delicacies"),
    ("Замразени зеленчуци", "Frozen Vegetables"),
    ("Зеленчукови миксове", "Vegetable Mixes"),
    ("Замразено месо", "Frozen Meat"),
    ("Птиче месо", "Poultry"),
    ("Морски дарове", "Seafood"),
    ("Лазаня, паста, пица", "Lasagna, Pasta, Pizza"),
    ("Пролетни рулца", "Spring Rolls"),
    ("Замразена пица", "Frozen Pizza"),
    ("Замразени плодове", "Frozen Fruits"),
    ("Горски плодове", "Forest Fruits"),
    ("Замразени картофи", "Frozen Potatoes"),
    ("Замразени моно зеленчуци", "Frozen Single Vegetables"),
    ("Замразени листни зеленчуци", "Frozen Leafy Greens"),
    ("Други замразени тестени изделия", "Other Frozen Pastries"),
    ("Друго месо", "Other Meat"),
    ("Агнешко месо", "Lamb"),
    ("Кайма и наденици", "Minced Meat and Sausages"),
    ("Панирани хапки", "Breaded Bites"),
    ("Говеждо и телешко месо", "Beef and Veal"),
    ("Бургери", "Burgers"),
("Тропически плодове", "Tropical Fruits"),
    ("Други плодове", "Other Fruits"),
    ("Сладолед", "Ice Cream"),
    ("Малки опаковки", "Small Packages"),
    ("Лед на кубчета", "Ice Cubes"),
    ("Фамилни опаковки", "Family Packages"),
    ("Замразени заготовки", "Frozen Preparations"),
    ("Замразени пелмени", "Frozen Dumplings"),
    ("Замразени риби", "Frozen Fish"),
    ("Риба", "Fish"),
    ("Прясна риба", "Fresh Fish"),
    ("Изчистена риба", "Cleaned Fish"),
    ("Прясно месо", "Fresh Meat"),
    ("Пилешко месо", "Chicken Meat"),
    ("Филе и котлет", "Fillet and Chop"),
    ("XXL Опаковки", "XXL Packages"),
    ("Свинско XXL", "Pork XXL"),
    ("Барбекю", "Barbecue"),
    ("Кюфтета и кебапчета", "Meatballs and Kebapche"),
    ("Свинско месо", "Pork"),
    ("Цяла риба", "Whole Fish"),
    ("Домакински принадлежности", "Household Accessories"),
    ("Фолио и хартия за печене", "Foil and Baking Paper"),
    ("Мляно месо", "Minced Meat"),
    ("Скариди", "Shrimp"),
    ("Октопод", "Octopus"),
    ("Миди", "Mussels"),
    ("Калмари", "Squid"),
    ("Стриди", "Oysters"),
("Наденици", "Sausages"),
    ("Месо и мариновани", "Meat and Marinated"),
    ("Готови и полуготови", "Ready and Semi-Ready"),
    ("Полуготови", "Semi-Ready"),
    ("Рибни деликатеси", "Fish Delicacies"),
    ("Пушена риба", "Smoked Fish"),
    ("Сурими", "Surimi"),
    ("Мариновани морски дарове", "Marinated Seafood"),
    ("Маринована риба", "Marinated Fish"),
    ("Тарама хайвер", "Tarama Caviar"),
    ("Готови", "Ready"),
    ("Агнешко XXL", "Lamb XXL"),
    ("Пилешко XXL", "Chicken XXL"),
    ("Телешко XXL", "Veal XXL"),
    ("Пликове", "Bags"),
    ("МЕТРО Месарница", "METRO Butchery"),
    ("Свежи салати и миксове", "Fresh Salads and Mixes"),
    ("Цели салати и репички", "Whole Salads and Radishes"),
    ("Зеленчуци", "Vegetables"),
    ("Чушки и люти чушки", "Peppers and Hot Peppers"),
    ("Тиквички и патладжан", "Zucchini and Eggplant"),
    ("Картофи", "Potatoes"),
    ("Краставици", "Cucumbers"),
    ("Цитрусови плодове", "Citrus Fruits"),
    ("Моркови и кореноплодни", "Carrots and Root Vegetables"),
    ("Миксове", "Mixes"),
    ("Лук, Чесън, Праз", "Onion, Garlic, Leek"),
    ("Авокадо", "Avocado"),
    ("Банани и екзотични плодове", "Bananas and Exotic Fruits"),
    ("Ябълки и круши", "Apples and Pears"),
    ("Домати", "Tomatoes"),
    ("Свежи подправки", "Fresh Herbs"),
    ("Гъби", "Mushrooms"),
    ("Пресни гъби", "Fresh Mushrooms"),
    ("Замразени гъби", "Frozen Mushrooms"),
    ("Сушени плодове и зеленчуци", "Dried Fruits and Vegetables"),
    ("Стафиди", "Raisins"),
    ("Фурми и смокини", "Dates and Figs"),
    ("Кайсии и сливи", "Apricots and Plums"),
    ("Сушени зеленчуци", "Dried Vegetables"),
    ("Сушени плодове", "Dried Fruits"),
    ("Семена и дресинги", "Seeds and Dressings"),
    ("Семена и кълнове", "Seeds and Sprouts"),
    ("Салатни дресинги", "Salad Dressings"),
    ("Сирене", "Cheese"),
    ("Прясно и фета сирене", "Fresh and Feta Cheese"),
    ("Масло и маргарин", "Butter and Margarine"),
    ("Масло", "Butter"),
    ("Кисело мляко", "Yogurt"),
    ("Краве кисело мляко", "Cow Yogurt"),
    ("Кашкавал", "Kashkaval"),
    ("Крави кашкавал", "Cow Kashkaval"),
    ("Извара, котидж, скир", "Cottage Cheese, Curd, Skyr"),
    ("Веган разядки", "Vegan Spreads"),
    ("Прясно мляко", "Fresh Milk"),
    ("Козе сирене", "Goat Cheese"),
    ("Топено и крема сирене", "Processed and Cream Cheese"),
    ("Топено сирене", "Processed Cheese"),
    ("Деликатесни сирена", "Gourmet Cheeses"),
    ("Маскарпоне и рикота", "Mascarpone and Ricotta"),
    ("Краве сирене", "Cow Cheese"),
    ("Други сирена", "Other Cheeses"),
    ("Моцарела и бурата", "Mozzarella and Burrata"),
    ("Козе кисело мляко", "Goat Yogurt"),
    ("Цедено кисело мляко", "Strained Yogurt"),
    ("Чедър, Гауда, Ементал", "Cheddar, Gouda, Emmental"),
    ("Бри и Камембер", "Brie and Camembert"),
    ("Крема сирене", "Cream Cheese"),
    ("Кози и овчи кашкавал", "Goat and Sheep Kashkaval"),
    ("Сметана", "Cream"),
    ("Сметана за готвене", "Cooking Cream"),
    ("Заквасена сметана", "Sour Cream"),
    ("Сладкарска сметана", "Confectionery Cream"),
    ("Протеинови млечни продукти", "Protein Dairy Products"),
    ("Плодови кисели млека и десерти", "Fruit Yogurts and Desserts"),
    ("Пудинг, Мус, Крем", "Pudding, Mousse, Cream"),
    ("Активиа", "Activia"),
    ("Плодови кисели млека", "Fruit Yogurts"),
    ("Майонеза сос", "Mayonnaise Sauce"),
    ("Сушени колбаси", "Cured Meats"),
    ("Луканкови салами", "Lukanka Sausages"),
    ("Сервиране", "Serving"),
    ("Колбаси и салами слайс", "Sliced Sausages and Salami"),
    ("Шунка и бекон слайс", "Sliced Ham and Bacon"),
    ("Суджуци", "Sujuk Sausages"),
    ("Луканки", "Lukanki Sausages"),
    ("Шунка, Бекон, Филе", "Ham, Bacon, Fillet"),
    ("Филе", "Fillet"),
    ("Италиански салами слайс", "Sliced Italian Salami"),
    ("Свинско филе и врат слайс", "Sliced Pork Fillet and Neck"),
    ("Испански салами слайс", "Sliced Spanish Salami"),
    ("Филе елена слайс", "Sliced Elena Fillet"),
    ("Бабек и Бански старец", "Babek and Banski Starets"),
    ("Шпекови салами слайс", "Sliced Speck Sausages"),
    ("Испански колбаси", "Spanish Sausages"),
    ("Кренвирши, Наденици, Вурстове", "Frankfurters, Sausages, Wursts"),
    ("Кренвирши", "Frankfurters"),
    ("Бекон", "Bacon"),
    ("Пастети", "Pâtés"),
    ("Терин", "Terrine"),
    ("Патешки пастет", "Duck Pâté"),
    ("Детски пастет", "Children's Pâté"),
    ("Свински пастет", "Pork Pâté"),
    ("Цели варено-пушени колбаси", "Whole Cooked-Smoked Sausages"),
    ("Телешки", "Beef"),
    ("Хамбургски", "Hamburg-style"),
    ("Камчия", "Kamchia"),
    ("Саздърма и желирани меса", "Sazdarma and Jellied Meats"),
    ("Хайвер", "Caviar"),
    ("Филе Елена", "Elena Fillet"),
    ("Чинии", "Plates"),
    ("Почистващи препарати и принадлежности/средства", "Cleaning Products and Supplies"),
    ("За баня и тоалетна", "For Bathroom and Toilet"),
    ("Перилни препарати", "Laundry Detergents"),
    ("Против петна", "Stain Removers"),
    ("Капсули за пране", "Laundry Capsules"),
    ("Кани, бутилки, термоси", "Jugs, Bottles, Thermoses"),
    ("Други кани", "Other Jugs"),
    ("Омекотители", "Softeners"),
    ("Домашни потреби", "Household Items"),
    ("Мопове и подочистачки", "Mops and Floor Cleaners"),
    ("Филтри", "Filters"),
    ("Стъклочистачки", "Glass Cleaners"),
    ("Дом и градина", "Home and Garden"),
    ("Разклонители", "Power Strips"),
    ("Готвене", "Cooking"),
    ("Хартии", "Papers"),
    ("Тоалетна хартия", "Toilet Paper"),
    ("Салфетки", "Napkins"),
    ("Кухненска хартия", "Kitchen Paper"),
    ("Ръкавици, кърпи, гъби", "Gloves, Towels, Sponges"),
    ("За кухнята", "For the Kitchen"),
    ("Гел за пране", "Laundry Gel"),
    ("Прах за пране", "Laundry Powder"),
    ("Други", "Others"),
    ("Кърпи", "Towels"),
    ("Кърпи за ръце", "Hand Towels"),
    ("Ръкавици", "Gloves"),
    ("Почистване на съдове", "Dish Cleaning"),
    ("За съдомиялна машина", "For Dishwasher"),
    ("Препарат за съдове", "Dish Detergent"),
    ("Био и еко почистващи препарати", "Bio and Eco Cleaning Products"),
    ("Чували за смет", "Trash Bags"),
    ("51-100 Литра", "51–100 Liters"),
    ("101 Литра +", "101 Liters +"),
    ("21-50 Литра", "21–50 Liters"),
    ("Форми за сладкиши", "Baking Molds"),
    ("Барбекю и парти артикули", "BBQ and Party Supplies"),
    ("Съдове за еднократна употреба", "Disposable Tableware"),
    ("Батерии", "Batteries"),
    ("Малки електро уреди", "Small Electric Appliances"),
    ("Филтърни бутилки", "Filtered Bottles"),
    ("Филтърни Кани", "Filtered Jugs"),
    ("Спрейове и аромати за дома", "Home Sprays and Scents"),
    ("Ароматизатори", "Air Fresheners"),
    ("Свещи", "Candles"),
    ("Свещи за рожден ден", "Birthday Candles"),
    ("Веган месо", "Vegan Meat"),
    ("Кленов сироп", "Maple Syrup"),
    ("Био основни храни", "Organic Staple Foods"),
    ("Био и еко перилни препарати", "Organic and Eco Laundry Detergents"),
    ("Безглутенова паста", "Gluten-Free Pasta"),
    ("Бира без глутен", "Gluten-Free Beer"),
    ("Био млечни и млечни алтернативи", "Organic Dairy and Alternatives"),
    ("Вафли", "Waffles"),
    ("Корнфлейкс", "Cornflakes"),
    ("Без лактоза", "Lactose-Free"),
    ("Веган млечни заместители", "Vegan Dairy Substitutes"),
    ("Сметана за кафе", "Coffee Creamer"),
    ("Млечни напитки", "Dairy Drinks"),
    ("Айрян, Кефир, Таратор", "Ayran, Kefir, Tarator")
};

        for (int i = 0; i < categories.Count; i++)
        {
            if (categories[i].NameInBulgarian == categoriesName[i].Bulgarian)
            {
                categories[i].NameInEnglish = categoriesName[i].English;
            }
        }

        await this.data.SaveChangesAsync();
    }
}
