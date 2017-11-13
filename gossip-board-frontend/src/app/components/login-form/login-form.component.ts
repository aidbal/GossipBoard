import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Params, Router} from '@angular/router';
import {UserService} from '../../services/user.service';
import {isUndefined} from 'util';
import {AuthService} from '../../services/auth.service';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/map';

interface Credentials {
  username: string;
  password: string;
}

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.css']
})
export class LoginFormComponent implements OnInit {
  authenticationFlag = true;
  authFlagForAnimation = true; // a copy of the above flag for the animation
  headsUpMessage: string;

  constructor(
    private router: Router,
    private userService: UserService,
    private activatedRoute: ActivatedRoute,
    private auth: AuthService
  ) { }

  ngOnInit() {
    // // this.userService.logOut();
    //
    // // subscribe to router event
    //     this.activatedRoute.queryParams.subscribe((params: Params) => {
    //       const returnUrl = params['returnUrl'];
    //       if (!isUndefined(returnUrl)) {
    //         this.headsUpMessage = 'You must log in first!';
    //       } else {
    //         this.headsUpMessage = '';
    //   }
    // });
  }

  signIn(credentials) { // credentials - from values
    this.authFlagForAnimation = true;
    this.userService.login(credentials)
      .subscribe(
        result => {},
        error => {
        this.authenticationFlag = false;
        this.authFlagForAnimation = false;
        }
      );
  }

  logout() {
    this.userService.logout();
  }

  redirectToRegister() {
    this.router.navigate(['/register']);
  }
}
