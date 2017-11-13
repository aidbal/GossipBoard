import {Component, Input, OnInit} from '@angular/core';
import {User} from '../../models/user';
import {AuthService} from '../../services/auth.service';
import {UserService} from '../../services/user.service';
import {MdSidenav} from '@angular/material';

@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.css']
})
export class UserDetailsComponent implements OnInit {
  @Input() sidenav: MdSidenav;
  user: User;
  private userService; // exposing the service to template
  constructor(userService: UserService) {
    this.userService = userService;
  }
  ngOnInit() {
  }
  onLogOutClick() {
    this.sidenav.close();
    this.userService.logout();
  }

}
