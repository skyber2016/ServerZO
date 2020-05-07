import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeLayoutComponent } from './Layouts/home-layout/home-layout.component';
import { DashboardPageComponent } from './Pages/dashboard-page/dashboard-page.component';
import { HeaderComponent } from './Layouts/home-layout/Components/header/header.component';
import { LeftSideComponent } from './Layouts/home-layout/Components/left-side/left-side.component';
import { FooterComponent } from './Layouts/home-layout/Components/footer/footer.component';
import { BannerComponent } from './Pages/dashboard-page/Components/banner/banner.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NewsComponent } from './Pages/dashboard-page/Components/news/news.component';
import { RanksComponent } from './Layouts/home-layout/Components/ranks/ranks.component';
import { AuthComponent } from './Layouts/home-layout/Components/auth/auth.component';
import { LoginFormComponent } from './Layouts/home-layout/Components/login-form/login-form.component';
import { MatDialogModule } from '@angular/material/dialog';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { from } from 'rxjs';
import { HttpConfigInterceptor } from './Share/httpconfig.interceptor';
@NgModule({
  declarations: [
    AppComponent,
    HomeLayoutComponent,
    DashboardPageComponent,
    HeaderComponent,
    BannerComponent,
    LeftSideComponent,
    FooterComponent,
    NewsComponent,
    RanksComponent,
    AuthComponent,
    LoginFormComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatDialogModule,
    HttpClientModule
  ],
  providers: [{ provide: HTTP_INTERCEPTORS, useClass: HttpConfigInterceptor, multi: true }],
  bootstrap: [AppComponent],
  entryComponents: [LoginFormComponent]
})
export class AppModule { }
