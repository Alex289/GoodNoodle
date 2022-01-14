import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {
  NbThemeModule,
  NbSidebarModule,
  NbLayoutModule,
  NbButtonModule,
  NbUserModule,
  NbTabsetModule,
  NbCardModule,
  NbListModule,
  NbSelectModule,
  NbIconModule,
  NbWindowModule,
  NbInputModule,
  NbDialogModule,
  NbSearchModule,
  NbCheckboxModule,
  NbOptionModule,
  NbAccordionModule,
  NbBadgeModule
} from '@nebular/theme';
import {
  NbPasswordAuthStrategy,
  NbAuthModule,
  NbAuthJWTToken
} from '@nebular/auth';
import { NbEvaIconsModule } from '@nebular/eva-icons';
import { NavbarComponent } from './components/navbar/navbar.component';
import { AdminComponent } from './pages/admin/admin.component';
import { CreateGroupDialogComponent } from './components/dialogs/create-group-dialog/create-group-dialog.component';
import { FormsModule } from '@angular/forms';
import { GroupComponent } from './pages/group/group.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { ToastrModule } from 'ngx-toastr';
import { AdminDialogComponent } from './components/dialogs/admin-dialog/admin-dialog.component';
import { DeleteDialogComponent } from './components/dialogs/delete-dialog/delete-dialog.component';
import { SearchComponent } from './pages/search/search.component';
import { AddToGroupDialogComponent } from './components/dialogs/add-to-group-dialog/add-to-group-dialog.component';
import { AuthInterceptor } from './interceptor/auth.interceptor';
import { AuthGuard } from './services/auth/auth-guard.service';
import { DeclinedComponent } from './pages/declined/declined.component';
import { DeclinedGuard } from './services/auth/declined-guard.service';
import { CreateStarComponent } from './components/dialogs/create-star/create-star.component';
import { UserSettingsComponent } from './components/dialogs/user-settings/user-settings.component';
import { ChangePasswordComponent } from './components/dialogs/change-password/change-password.component';
import { JoinComponent } from './pages/join/join.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    AdminComponent,
    SidebarComponent,
    CreateGroupDialogComponent,
    GroupComponent,
    AdminDialogComponent,
    DeleteDialogComponent,
    DeclinedComponent,
    SearchComponent,
    AddToGroupDialogComponent,
    CreateStarComponent,
    UserSettingsComponent,
    ChangePasswordComponent,
    JoinComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    NbCheckboxModule,
    NbBadgeModule,
    NbInputModule,
    NbIconModule,
    FormsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    NbAuthModule.forRoot({
      strategies: [
        NbPasswordAuthStrategy.setup({
          name: 'email',
          baseEndpoint: 'https://localhost:5001/api',
          token: {
            class: NbAuthJWTToken,
            key: 'data'
          },
          login: {
            endpoint: '/user/login'
          },
          register: {
            endpoint: '/user/register'
          }
        })
      ],
      forms: {
        login: {
          rememberMe: false
        }
      }
    }),
    NbThemeModule.forRoot({ name: 'cosmic' }),
    NbSidebarModule.forRoot(),
    NbLayoutModule,
    NbTabsetModule,
    NbWindowModule.forRoot(),
    NbInputModule,
    NbUserModule,
    NbEvaIconsModule,
    NbCardModule,
    NbAccordionModule,
    NbListModule,
    NbSelectModule,
    NbOptionModule,
    NbButtonModule,
    NbEvaIconsModule,
    NbSearchModule,
    ToastrModule.forRoot(),
    NbDialogModule.forRoot()
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    AuthGuard,
    DeclinedGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
