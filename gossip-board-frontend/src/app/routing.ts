/**
 * Test routing
 */
import {RouterModule, Routes} from '@angular/router';
import {PostContainerComponent} from './components/post-container/post-container.component';
import {SignUpComponent} from './components/sign-up/sign-up.component';
import {LoginFormComponent} from './components/login-form/login-form.component';
import {AuthGuard} from './guards/auth.guard';

const appRoutes: Routes = [
  {path: 'main', component: PostContainerComponent, canActivate: [AuthGuard]},
  {path: 'register', component: SignUpComponent},
  {path: 'login', component: LoginFormComponent},
  {path: '', redirectTo: 'main', pathMatch: 'full'},
  {path: '**', redirectTo: 'main'}
];
export const routing = RouterModule.forRoot(appRoutes);
