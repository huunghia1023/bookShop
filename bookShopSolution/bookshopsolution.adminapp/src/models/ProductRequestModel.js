export default class ProductRequestModel {
  constructor(
    Description,
    ProductName,
    Stock,
    OriginalPrice,
    Price,
    Details,
    SeoDescription,
    SeoTitle,
    SeoAlias,
    LanguageId,
    Thumbnail,
    IsFeatured
  ) {
    this.Description = Description;
    this.ProductName = ProductName;
    this.Stock = Stock;
    this.OriginalPrice = OriginalPrice;
    this.Price = Price;
    this.Details = Details;
    this.SeoDescription = SeoDescription;
    this.SeoTitle = SeoTitle;
    this.SeoAlias = SeoAlias;
    this.LanguageId = LanguageId;
    this.Thumbnail = Thumbnail;
    this.IsFeatured = IsFeatured;
  }

  GetCreateProductFormData(
    Description,
    ProductName,
    Stock,
    OriginalPrice,
    Price,
    Details,
    SeoDescription,
    SeoTitle,
    SeoAlias,
    LanguageId,
    Thumbnail,
    IsFeatured
  ) {
    var formData = new FormData();
    formData.append("Description", Description);
    formData.append("ProductName", ProductName);
    formData.append("Stock", Stock);
    formData.append("OriginalPrice", OriginalPrice);
    formData.append("Price", Price);
    formData.append("Details", Details);
    formData.append("SeoDescription", SeoDescription);
    formData.append("SeoTitle", SeoTitle);
    formData.append("SeoAlias", SeoAlias);
    formData.append("LanguageId", LanguageId);
    formData.append("SeoAlias", SeoAlias);
    formData.append("ThumbnailImage", Thumbnail);
    formData.append("IsFeatured", IsFeatured);

    return formData;
  }

  GetAssignProductBody(categories) {
    return { categories: categories };
  }

  GetUpdateProductFormData(
    Description,
    ProductName,
    Details,
    SeoDescription,
    SeoTitle,
    SeoAlias,
    Price,
    Stock,
    LanguageId
  ) {
    var formData = new FormData();
    formData.append("Description", Description);
    formData.append("ProductName", ProductName);
    formData.append("Details", Details);
    formData.append("SeoDescription", SeoDescription);
    formData.append("SeoTitle", SeoTitle);
    formData.append("SeoAlias", SeoAlias);
    formData.append("LanguageId", LanguageId);
    formData.append("SeoAlias", SeoAlias);
    formData.append("Price", Price);
    formData.append("Stock", Stock);

    return formData;
  }
}
