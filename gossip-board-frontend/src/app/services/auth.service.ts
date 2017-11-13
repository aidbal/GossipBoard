import { Injectable } from '@angular/core';
import {Http, RequestOptions, Response, Headers} from '@angular/http';
import 'rxjs/add/operator/map';
import {Constants} from '../constants';
import {Observable} from 'rxjs/Observable';
import {User} from '../models/user';
import {CookieOptions, CookieService} from 'angular2-cookie/core';
import {isNull, isUndefined} from 'util';

@Injectable()
export class AuthService {
  public token: string;
  public isAuthenticated = false;

  constructor(private http: Http, private constants: Constants, private cookieService: CookieService) {
    // set token if saved in local storage
    const currentUser = JSON.parse(localStorage.getItem('currentEmail'));
    this.token = currentUser && currentUser.token;
  }

  login(credentials): Observable<User> {
    this.getCookieOptions();
    return this.http.post( this.constants.accountSignInUrl, credentials, this.getRequestOptions())
      .map((response: Response) => {
        // login successful if there's a jwt token in the response
        const token = response.json().tokenObject ; // && response.json().token;
        if (token) {
          // set token property
          this.token = token;

          // store username and jwt token in local storage to keep user logged in between page refreshes
          localStorage.setItem( this.constants.currentUserCookieName, JSON.stringify({ email: credentials.email, token: token }));

          const respUser = response.json().userObject;
          const u = new User();
          u.Email = respUser.email;
          u.Firstname = respUser.firstname;
          u.Lastname = respUser.lastname;
          // returning a user means the user was logged in
          this.cookieService.putObject(this.constants.currentUserCookieName, u, this.getCookieOptions());
          return u;
        } else {
          // return null to indicate failed login
          return null;
        }
      });
  }

  private getCookieOptions() {
    const expDate = new Date(Date.now() + 1000 * 60 * 60); // expires in 1h
    // console.log(expDate.toTimeString());
    const opt = new CookieOptions();
    opt.expires = expDate;
    return opt;
  }

  register(credentials): Observable<User> {
    return this.http.post( this.constants.accountApiUrl, credentials, this.getRequestOptions())
      .map((response: Response) => {
        // login successful if there's a jwt token in the response
        const token = response.json().tokenObject ; // && response.json().token;
        if (token) {
          const respUser = response.json().userObject;
          const u = new User();
          u.Email = respUser.email;
          u.Firstname = respUser.firstname;
          u.Lastname = respUser.lastname;
          this.cookieService.putObject(this.constants.currentUserCookieName, u, this.getCookieOptions());
          return u;
        } else {
          // return false to indicate failed login
          return null;
        }
      });
  }

  getUserFromCookie(): User {
    const u = this.cookieService.getObject(this.constants.currentUserCookieName);
    return <User>u;
  }

  logout(): void {
    // console.log('removing user info cookie');
    this.cookieService.remove(this.constants.currentUserCookieName);
  }

  loggedIn(): boolean {
    const u = this.getUserFromCookie();
    return (!isNull(u) && !isUndefined(u));
  }

  getRequestOptions() {
    const headers = new Headers ({
      'Content-Type': 'application/json',
    });
    return new RequestOptions({ headers: headers, withCredentials: true });
  }

}
