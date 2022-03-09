export default class UserRequestModel {
  constructor(
    LastName,
    UserName,
    PhoneNumber,
    FirstName,
    ConfirmPassword,
    Password,
    Email,
    BirthDay,
    Roles
  ) {
    this.LastName = LastName;
    this.UserName = UserName;
    this.PhoneNumber = PhoneNumber;
    this.FirstName = FirstName;
    this.ConfirmPassword = ConfirmPassword;
    this.Password = Password;
    this.Email = Email;
    this.BirthDay = BirthDay;
    this.Roles = Roles;
  }

  GetCreateUserFormData(
    LastName,
    UserName,
    PhoneNumber,
    FirstName,
    ConfirmPassword,
    Password,
    Email,
    BirthDay,
    Roles
  ) {
    var formData = new FormData();
    formData.append("LastName", LastName);
    formData.append("UserName", UserName);
    formData.append("PhoneNumber", PhoneNumber);
    formData.append("FirstName", FirstName);
    formData.append("ConfirmPassword", ConfirmPassword);
    formData.append("Password", Password);
    formData.append("Email", Email);
    formData.append("BirthDay", BirthDay);
    Roles.forEach((element) => {
      formData.append("Roles", element);
    });
    return formData;
  }

  GetUpdateUserFormData(
    FirstName,
    LastName,
    PhoneNumber,
    Email,
    BirthDay,
    Roles
  ) {
    var formData = new FormData();
    formData.append("FirstName", FirstName);
    formData.append("LastName", LastName);
    formData.append("PhoneNumber", PhoneNumber);
    formData.append("Email", Email);
    formData.append("BirthDay", BirthDay);
    Roles.forEach((element) => {
      formData.append("Roles", element);
    });
    return formData;
  }
}
