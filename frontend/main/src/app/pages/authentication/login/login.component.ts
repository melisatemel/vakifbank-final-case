import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { StorageService } from 'src/app/services/storage.service';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
})
export class AppSideLoginComponent implements OnInit{

  email: string;
  password: string;
  constructor(private authService: AuthService, private toastr: ToastrService, private router: Router, private storage: StorageService) {}

  ngOnInit(): void {
    if(this.storage.getUser()){
      this.router.navigate(['/dashboard']);
    }
  }


  login(){
    this.authService.login(this.email, this.password).subscribe(data=>{
      if(this.password.length < 4){
        this.toastr.error('The password must be at least 4 digits');
        return;
      }
      if(data.success === true){
        this.storage.saveUser(data.response)
        if(this.authService.getRole()){
          this.router.navigate(['admin/dashboard']);
        }else{
          this.router.navigate(['/dashboard']);
        }
      }
      else{
        this.toastr.error('You have logged in incorrectly');
      }
    })
  }
}
