import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { AuthService } from 'src/app/Services/Auth/auth.service';
import { core } from 'src/app/Share/core';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.scss']
})
export class LoginFormComponent implements OnInit {
  @ViewChild('username') username: ElementRef;
  @ViewChild('password') password: ElementRef;
  message = '';
  constructor(private authService: AuthService) { }

  ngOnInit(): void {
  }
  login() {
    const usrName = this.username.nativeElement.value;
    const pwd = this.password.nativeElement.value;
    if (usrName && pwd) {
      this.authService.login({ username: usrName, password: pwd }).subscribe(result => {
        window.localStorage.setItem('token', result.token);
        window.localStorage.setItem('user', result.user);
      }, err => {
        this.message = err.Message;
      });
    }
  }
}
