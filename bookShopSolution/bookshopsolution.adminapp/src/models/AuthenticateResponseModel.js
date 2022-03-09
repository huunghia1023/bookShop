export default class AuthenticateResponseModel {
    constructor(accessToken, email, expires, firsName, lastName, roles, scope, tokenType) {
      this.accessToken = accessToken;
      this.email = email;
      this.expires = expires;
      this.firsName = firsName;
      this.lastName = lastName;
      this.roles = roles;
      this.scope = scope;
      this.tokenType = tokenType;
    }
};