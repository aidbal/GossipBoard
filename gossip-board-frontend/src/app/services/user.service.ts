import { Injectable } from '@angular/core';
import {User} from '../models/user';
import {AuthService} from './auth.service';
import {Observable} from 'rxjs/Observable';
import {Router} from '@angular/router';
import {isNull, isUndefined} from 'util';
import {ReplaySubject} from 'rxjs/ReplaySubject';

@Injectable()
export class UserService {
  // for testing
  user: User;

  constructor(private authService: AuthService,
              private router: Router) {
    this.user = null;
  }

  getCurrentUser(): User {
    this.user = this.authService.getUserFromCookie();
    if (!isNull(this.user) && !isUndefined(this.user)) {
      return this.user;
    } else {
      return null;
    }
  }

  isLoggedIn(): boolean {
    this.user = this.getCurrentUser();
    return (!isNull(this.user) && !isUndefined(this.user))
            && this.authService.loggedIn();
  }

  logout() {
    this.authService.logout();
    this.user = null;
    this.router.navigate(['/login']);
  }
  login(credentials): Observable<User> {
    const res = this.authService.login(credentials);
    res.subscribe(
      (user: User) => {
        this.user = user;
        this.router.navigate(['/main']);
      }
    );
    return res;
  }

  register(credentials): Observable<User> {
    // console.log(credentials);
    const res = this.authService.register(credentials);
    res.subscribe(
      (user: User) => {
        this.user = user;
        this.router.navigate(['/main']);
      }
    );
    return res;
  }
}
