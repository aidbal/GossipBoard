import { PostService } from '../../services/post.service';
import {Component, Input, OnInit} from '@angular/core';
import {Post} from '../../models/post';
import {UserService} from '../../services/user.service';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss']
})
export class PostComponent implements OnInit {
  @Input() post: Post;
  @Input() posts: Post[];
  userEmail: String;
  arrowUp: Boolean;
  constructor(private postService: PostService, private userService: UserService) {

  }
  ngOnInit() {
    this.userEmail = this.userService.getCurrentUser().Email;
    this.arrowUp = true;
}
  removeCard(cardId) {
    const index = this.posts.findIndex(post => post.id === cardId);
    this.postService.deletePost(cardId).subscribe(() => {
  },
    error => {
  });
   this.posts.splice(index, 1);
  }
  upVote(cardId: number) {
    this.postService.upVotePost(cardId).subscribe(response => {
      this.arrowUp = !this.arrowUp;
      this.post.likesCount =  parseInt(response.text());
    });
  }
  downVote(cardId: number) {
   // this.postService.downVote(cardId);
  }


}
