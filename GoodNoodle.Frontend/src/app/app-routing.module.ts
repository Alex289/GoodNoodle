import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './pages/main/main.component';
import { AdminComponent } from './pages/admin/admin.component';

import {
  NbLoginComponent,
  NbRegisterComponent,
  NbLogoutComponent
} from '@nebular/auth';
import { SearchComponent } from './pages/search/search.component';
import { GroupComponent } from './pages/group/group.component';
import { AuthGuard } from './services/auth/auth-guard.service';
import { DeclinedComponent } from './pages/declined/declined.component';
import { DeclinedGuard } from './services/auth/declined-guard.service';
import { JoinComponent } from './pages/join/join.component';

export const routes: Routes = [
  {
    path: 'login',
    canActivate: [DeclinedGuard],
    component: NbLoginComponent
  },
  {
    path: 'register',
    canActivate: [DeclinedGuard],
    component: NbRegisterComponent
  },
  {
    path: 'logout',
    component: NbLogoutComponent
  },
  {
    path: '',
    canActivate: [DeclinedGuard],
    component: MainComponent
  },
  {
    path: 'admin',
    canActivate: [AuthGuard, DeclinedGuard],
    component: AdminComponent
  },
  {
    path: 'group/:id',
    pathMatch: 'full',
    canActivate: [DeclinedGuard],
    component: GroupComponent
  },
  {
    path: 'search/:name',
    pathMatch: 'full',
    canActivate: [DeclinedGuard],
    component: SearchComponent
  },
  {
    path: 'declined',
    component: DeclinedComponent
  },
  {
    path: 'join/:id',
    canActivate: [DeclinedGuard],
    component: JoinComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
