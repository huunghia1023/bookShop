export default class ProductModel {
  constructor(
    id,
    dateCreated,
    description,
    details,
    languageId,
    name,
    originalPrice,
    price,
    seoAlias,
    seoDescription,
    seoTitle,
    stock,
    viewCount
  ) {
    this.id = id;
    this.dateCreated = dateCreated;
    this.description = description;
    this.details = details;
    this.languageId = languageId;
    this.name = name;
    this.originalPrice = originalPrice;
    this.price = price;
    this.seoAlias = seoAlias;
    this.seoDescription = seoDescription;
    this.seoTitle = seoTitle;
    this.stock = stock;
    this.viewCount = viewCount;
  }
}
