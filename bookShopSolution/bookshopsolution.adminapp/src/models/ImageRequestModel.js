export default class ImageRequestModel {
  constructor(Caption, IsDefault, SortOrder, ImageFile) {
    this.Caption = Caption;
    this.IsDefault = IsDefault;
    this.SortOrder = SortOrder;
    this.ImageFile = ImageFile;
  }

  GetCreateImageFormData(Caption, IsDefault, SortOrder, ImageFile) {
    var formData = new FormData();
    formData.append("Caption", Caption);
    formData.append("IsDefault", IsDefault);
    formData.append("SortOrder", SortOrder);
    if (ImageFile) {
      formData.append("ImageFile", ImageFile);
    }

    return formData;
  }

  GetDeleteImageFormData(Ids) {
    var formData = new FormData();
    Ids.forEach((id) => {
      formData.append("id", id);
    });
    return formData;
  }
}
