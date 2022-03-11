export default class CategoryRequestModel {
  constructor(
    Id,
    Name,
    SeoDescription,
    SeoTitle,
    SeoAlias,
    IsShowOnHome,
    LanguageId
  ) {
    this.Id = Id;
    this.Name = Name;
    this.SeoDescription = SeoDescription;
    this.SeoTitle = SeoTitle;
    this.SeoAlias = SeoAlias;
    this.IsShowOnHome = IsShowOnHome;
    this.languageId = LanguageId;
  }

  GetCreateCategoryBody(
    Name,
    SeoDescription,
    SeoTitle,
    SeoAlias,
    IsShowOnHome,
    LanguageId
  ) {
    var formData = new FormData();
    formData.append("Name", Name);
    formData.append("SeoDescription", SeoDescription);
    formData.append("SeoTitle", SeoTitle);
    formData.append("SeoAlias", SeoAlias);
    formData.append("IsShowOnHome", IsShowOnHome);
    formData.append("LanguageId", LanguageId);
    return formData;
  }

  GetUpdateCategoryFormData(
    Name,
    SeoDescription,
    SeoTitle,
    SeoAlias,
    IsShowOnHome,
    LanguageId
  ) {
    var formData = new FormData();
    formData.append("Name", Name);
    formData.append("SeoDescription", SeoDescription);
    formData.append("SeoTitle", SeoTitle);
    formData.append("SeoAlias", SeoAlias);
    formData.append("IsShowOnHome", IsShowOnHome);
    formData.append("LanguageId", LanguageId);
    return formData;
  }

  GetDeleteCategoryFormData(Ids) {
    var formData = new FormData();
    Ids.forEach((id) => {
      formData.append("ids", id);
    });
    return formData;
  }
}
