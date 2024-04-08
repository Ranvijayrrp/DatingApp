import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {

  model : any = {} 
  loggedIn : boolean = false;

  constructor(private accountService : AccountService) {}

  ngOnInit(){
    
  }
  login(){
    this.accountService.login(this.model).subscribe({
      next : (reponse) =>{
       console.log(reponse);
       this.loggedIn = true
      },
      error : (e) => console.log(e),
      complete : () => console.info("Login call completed")
    });
    //console.log(this.model);
  }

  logout(){
    this.loggedIn =false;
  }

}
