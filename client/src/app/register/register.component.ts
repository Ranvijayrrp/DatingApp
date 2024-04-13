import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  //@Input() userFromHomeComponent: any; to pass data from parent to child
  @Output() cancelRegister = new EventEmitter();
  model: any = {};

  constructor(private accountService : AccountService , private toastr : ToastrService
  ){}

  ngOnInit(): void {

  }

  register() {
    this.accountService.register(this.model).subscribe({
      next: (response) =>{
         this.cancel();
         console.log(this.model);
      },
      error : (er) => {
        console.log(er)
        this.toastr.error(er.error.message);
      },
      complete : ()=> console.log('registration completed for user : ' + this.model.username)
    });
  }

  cancel() {
    this.cancelRegister.emit(false);
    console.log('cancelled');
  }
}
