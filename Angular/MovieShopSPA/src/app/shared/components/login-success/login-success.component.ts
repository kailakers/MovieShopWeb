import { Component, OnInit } from '@angular/core';
import { JwtStorageService } from 'src/app/core/services/jwtstorage.service'

@Component({
  selector: 'app-login-success',
  templateUrl: './login-success.component.html',
  styleUrls: ['./login-success.component.css']
})
export class LoginSuccessComponent implements OnInit {

  token: string = "";
  status: boolean = false;
  constructor(private JwtStorageService: JwtStorageService) { }

  ngOnInit(): void {
    this.token += this.JwtStorageService.getToken();
    if(this.token!=null){
      this.status = true;
    }
    console.log("token="+this.token);
    console.log(this.status);
  }

}
