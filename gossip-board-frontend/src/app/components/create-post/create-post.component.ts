import { Component } from '@angular/core';
import {MdDialog, MdDialogRef} from '@angular/material';
import {PostService} from '../../services/post.service';
import {Post} from '../../models/post';
import {UserService} from '../../services/user.service';

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.css']
})
export class CreatePostComponent  {
  errors;
  enabled = true;
  validationError = false;
  constructor(private postService: PostService, private userService: UserService, private dialogRef: MdDialogRef<CreatePostComponent>) {
    this.errors = null;
  }

  submitPost(value: Post) {
    this.enabled = false;
    this.errors = null;
    this.postService.createPost(value).subscribe(
      (response) => {
            this.dialogRef.close();
            this.enabled = true;
      },
      (error) => {
          const response = error.text();
          this.errors = JSON.parse(response);
          this.enabled = true;
      },
      () => {
        this.enabled = true;
      }
      );
  }

}
