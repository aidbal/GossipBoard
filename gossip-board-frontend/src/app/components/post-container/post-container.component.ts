import {Component, Input, OnInit} from '@angular/core';
import {PostService} from '../../services/post.service';
import {Post} from '../../models/post';
import {MdDialog} from '@angular/material';
import { CreatePostComponent } from '../create-post/create-post.component';

@Component({
  selector: 'app-post-container',
  templateUrl: './post-container.component.html',
  styleUrls: ['./post-container.component.css']
})
export class PostContainerComponent implements OnInit {
posts: Post[] = [];
postCount = 0;
  constructor(private postService: PostService, private dialog: MdDialog) { }


  ngOnInit() {
    this.postService.getPosts().subscribe(posts => {
        this.posts = posts;
        this.postCount = posts.length;
      });
    this.postService.createPostObservable.subscribe((newPost: Post) => {
      this.posts.unshift(newPost);
      this.postCount++;
    });
    this.postService.searchFieldObservable.subscribe((search: string) => {
      // on new search string
      this.postService.searchPosts(search).subscribe(posts => {
        this.posts = posts;
        this.postCount = posts.length;
      });
    });
  }
}
