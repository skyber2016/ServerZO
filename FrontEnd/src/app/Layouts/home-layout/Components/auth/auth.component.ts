import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { LoginFormComponent } from '../login-form/login-form.component';
@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.scss']
})
export class AuthComponent implements OnInit {

  constructor(private dialog: MatDialog) { }

  ngOnInit(): void {
  }
  login() {
    const dialogRef = this.dialog.open(LoginFormComponent, {
      width: '300px',
      data: {}
    });

    dialogRef.afterClosed().subscribe(result => {

    });
  }
}
