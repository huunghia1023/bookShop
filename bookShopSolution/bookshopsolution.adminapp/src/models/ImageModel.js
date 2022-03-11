export default class ImageModel {
  constructor(
    ImageId,
    ImagePath,
    Caption,
    IsDefault,
    DateCreated,
    SortOrder,
    FileSize
  ) {
    this.ImageId = ImageId;
    this.ImagePath = ImagePath;
    this.Caption = Caption;
    this.IsDefault = IsDefault;
    this.DateCreated = DateCreated;
    this.SortOrder = SortOrder;
    this.FileSize = FileSize;
  }
}
