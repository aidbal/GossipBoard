import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { PasswordValidators } from './password.validators';
import {UserService} from '../../services/user.service';
import { Router} from '@angular/router';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent {
  form: FormGroup;
  shakeValidError = false;
  constructor(public formBuilder: FormBuilder,
              private userService: UserService,
              private router: Router) {
    this.form = this.formBuilder.group({
        email: ['', [Validators.required, Validators.email]],
        name: ['', Validators.required],
        lastname: ['', Validators.required],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirm: ['', Validators.required]},
        {validator: PasswordValidators.passwordsMatch }
      );
  }

   get email(){ return this.form.get('email'); }
   get password(){ return this.form.get('password'); }
   get confirm(){ return this.form.get('confirm'); }

   onFormSubmit() {
     this.shakeValidError = false;
     if (this.form.valid) {
        this.userService.register({
          Email: String( this.form.get('email').value),
          Password: String( this.form.get('password').value),
          Firstname: String( this.form.get('name').value),
          Lastname: String( this.form.get('lastname').value)
        });
      }else {
        this.shakeValidError = true;
      }
   }

  goBackToLogin() {
    this.router.navigate(['/login']);
  }
}
