import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../services/authentication.service';
import { User } from 'src/app/shared/models/user';
import { userInfo } from 'os';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  IsUserAuthenticated: boolean;
  logedInUser: User;
  constructor(
    private authService: AuthenticationService,
    private router: Router
    ) { }

  ngOnInit(): void {
    this.authService.isUserAuthenticated.subscribe(
      isLogedIn => {
        this.IsUserAuthenticated = isLogedIn;

        if (this.IsUserAuthenticated) {
          // get the user info
          this.authService.currentLogedInUser.subscribe(
            user => {
              this.logedInUser = user;
            });
        }
      }
    );
  }
  logout() {
    this.authService.logout();
    this.router.navigate(['/']);
  }
}
