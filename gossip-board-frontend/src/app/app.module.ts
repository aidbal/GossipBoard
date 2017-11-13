import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MdToolbarModule, MdButtonModule,
        MdInputModule, MdIconModule,
        MdSidenavModule, MdCardModule,
        MdDialogModule
        } from '@angular/material';
import { HttpModule } from '@angular/http';
import {RouterModule, Routes} from '@angular/router';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { SignUpComponent } from './components/sign-up/sign-up.component';
import { PostComponent } from './components/post/post.component';
import { LoginFormComponent } from './components/login-form/login-form.component';
import { CreatePostComponent } from './components/create-post/create-post.component';
import { PostService } from './services/post.service';
import { PostContainerComponent } from './components/post-container/post-container.component';
import {routing} from './routing';
import {UserService} from './services/user.service';
import {AuthGuard} from './guards/auth.guard';
import { UserDetailsComponent } from './components/user-details/user-details.component';
import {AuthService} from './services/auth.service';
import {Constants} from './constants';
import {CookieService} from 'angular2-cookie/core';

@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    SignUpComponent,
    PostComponent,
    LoginFormComponent,
    CreatePostComponent,
    PostContainerComponent,
    UserDetailsComponent
  ],
  imports: [
    routing,
    BrowserModule,
    BrowserAnimationsModule,
    MdInputModule,
    MdButtonModule,
    MdCardModule,
    MdDialogModule,
    FormsModule,
    ReactiveFormsModule,
    MdToolbarModule,
    RouterModule,
    MdIconModule,
    MdSidenavModule,
    HttpModule
    ],
    entryComponents: [
    CreatePostComponent
  ],
    providers: [
      PostService,
      UserService,
      AuthGuard,
      AuthService,
      Constants,
      CookieService
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
