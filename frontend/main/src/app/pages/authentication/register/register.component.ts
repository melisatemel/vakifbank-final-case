import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/services/auth.service';
import { StorageService } from 'src/app/services/storage.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
})
export class AppSideRegisterComponent {
  constructor(
    private router: Router,
    private toastr: ToastrService,
    private authService: AuthService,
    private storage: StorageService
  ) {}

  email: string;
  password: string;
  rePassword: string;
  firstName: string;
  lastName: string;

  register(){

    if(this.password!==this.rePassword){
      this.toastr.error('Password do not match');
    }
    else{
      this.authService.register(this.email,this.password,this.firstName,this.lastName).subscribe((data:any)=>{
        if(data.success === true){
          this.authService.login(this.email, this.password).subscribe(data=>{
            if(data.success === true){
              this.storage.saveUser(data.response) //chrome kayıt etti.
              this.router.navigate(['/dashboard']); //kullanıcıyı farklı sayfalara yönlendiri. (dashboarda yönlendirdik)
            }
            else{
              this.toastr.error('You have logged in incorrectly');
            }
          })


        }
        else{
          this.toastr.error('You have logged in incorrectly');
        }
      })
    }
  }
}
