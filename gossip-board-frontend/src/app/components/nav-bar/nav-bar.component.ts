import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormControl, Validators} from '@angular/forms';
import {MdDialog} from '@angular/material';
import { CreatePostComponent } from '../create-post/create-post.component';
import {UserService} from '../../services/user.service';
import {PostService} from '../../services/post.service';


@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {
  @Input() searchField: string;
  // user service used in template for create post dialogs
  constructor(private dialog: MdDialog, private userService: UserService, private postService: PostService) {
  }
  ngOnInit() {
  }
  onSearchFieldChange() {
    // on losing focus or enter
    this.postService._searchFieldSource.next(this.searchField);
  }
  openDialog() {
    const dialogRef = this.dialog.open(CreatePostComponent);
  }
}
