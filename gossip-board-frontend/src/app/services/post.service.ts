import { Injectable } from '@angular/core';
import {Observable} from 'rxjs/Observable';
import { Post} from '../models/post';
import { Http, Headers } from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import {Constants} from '../constants';
import {ReplaySubject} from 'rxjs/ReplaySubject';
import {UserService} from './user.service';






@Injectable()
export class PostService {
  // Observable search Field
  public _searchFieldSource = new ReplaySubject<string>();
  public searchFieldObservable = this._searchFieldSource.asObservable();


  private _createPostSource = new ReplaySubject<Post>();
  public createPostObservable  = this._createPostSource.asObservable();

  constructor(private http: Http, private constants: Constants, private userService: UserService) {
  }
  // private readonly backendUrl = 'http://localhost:29270';
  // private readonly webApiUrl = `${this.backendUrl}/api`;
  // private readonly postsApiUrl = `${this.webApiUrl}/posts`;
  getPosts(): Observable<Post[]> {
    return this.http.get(this.constants.postsApiUrl, this.getRequestOptions())
      .map(result => result.json())
      .catch(e => e.status === 401 ? Observable.throw('Unauthorized') : e.json());
  }

  createPost(body: Post) {
    let res =  this.http.post(this.constants.postsApiUrl, body, this.getRequestOptions());
    res.subscribe((response) => {
    const currentUser = this.userService.getCurrentUser();
    body.id = parseInt(response.text());
    body.applicationUserEmail = currentUser.Email;
    body.applicationUserFirstName = currentUser.Firstname;
    body.applicationUserLastName = currentUser.Lastname;
    body.likesCount = 0;
    this._createPostSource.next(body);
    });
    return res;
  }
  deletePost(postId) {
    return this.http.delete(`${this.constants.postsApiUrl}/${postId}`, this.getRequestOptions());
  }
  private getRequestOptions() {
    const headers = new Headers({
      'Content-Type': 'application/json',
    });
    return {headers: headers, withCredentials: true};
  }

  searchPosts(str: string): Observable<Post[]> {
    const offset = 0;
    const limit = 20;
    const searchUrl = `${this.constants.postsSearchApiUrl}?search=${encodeURIComponent(str)}
    &offset=${encodeURIComponent(String(offset))}&limit=${encodeURIComponent(String(limit))}`;
    return this.http.get(searchUrl, this.getRequestOptions())
      .map(result => result.json())
      .catch(e => e.status === 401 ? Observable.throw('Unauthorized') : e.json());
  }
  upVotePost(postId) {
    return this.http.get(`${this.constants.postsApiUrl}/upvote/${postId}`, this.getRequestOptions());
  }
}
