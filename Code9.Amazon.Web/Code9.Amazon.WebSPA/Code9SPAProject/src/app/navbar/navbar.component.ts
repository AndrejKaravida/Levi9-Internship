import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { User } from '../_models/user';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent implements OnInit {
  user: any;
  constructor(public authService: AuthService) {}

  ngOnInit(): void {
    this.authService.userData.subscribe((res) => {
      if (res !== null) {
        this.user = res;
      }
    });

    if (!this.user) {
      this.user = JSON.parse(localStorage.getItem('user'));
    }
  }
}
