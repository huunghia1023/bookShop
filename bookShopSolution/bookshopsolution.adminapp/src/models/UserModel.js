export default class UserModel {
    constructor(id, username, firstName, lastName, email, emailVerified, birthDay, roles, phoneNumber) {
      this.id = id;
      this.username = username;
      this.firstName = firstName;
      this.lastName = lastName;
      this.email = email;
      this.emailVerified = emailVerified;
      this.birthDay = birthDay;
      this.roles = roles;
      this.phoneNumber = phoneNumber;
    }
};