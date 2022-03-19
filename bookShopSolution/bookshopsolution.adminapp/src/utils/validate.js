export const ValidateEmail = (email) => {
  return email.match(
    /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
  );
};

export const ValidatePassword = (password) => {
  return password && password.length > 6;
};

export const ValidateConfirmPassword = (confirmPassword, password) => {
  return confirmPassword && confirmPassword === password && password.length > 6;
};
