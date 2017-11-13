export class Constants {
  readonly backendUrl = 'http://localhost:29270';
  readonly webApiUrl = `${this.backendUrl}/api`;
  readonly postsApiUrl = `${this.webApiUrl}/posts`
  readonly postsSearchApiUrl = `${this.postsApiUrl}/search`;
  readonly accountApiUrl = `${this.webApiUrl}/account`;
  readonly accountSignInUrl = `${this.accountApiUrl}/sign-in`;
  readonly accountChangePasswordUrl = `${this.accountApiUrl}/change-password`;
  readonly currentUserCookieName = 'currentUserInfoCookie';
}
