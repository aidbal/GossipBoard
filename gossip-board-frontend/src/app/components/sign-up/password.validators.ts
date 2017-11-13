import { AbstractControl } from '@angular/forms';

export class PasswordValidators {
  static passwordsMatch(control: AbstractControl) {
    const password = control.get('password');
    const passwordConfirm = control.get('confirm');

    if (password.value !== passwordConfirm.value) {
      return{ passwordsMatch: true};
    }
    return false;
  }
}
