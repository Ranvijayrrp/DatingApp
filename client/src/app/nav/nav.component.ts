import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable, switchMap } from 'rxjs';
import { User } from '../_models/user';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {

  model: any = {};
  loggedIn: boolean = false; 
  //currentUser$ = new Observable<User>;

  constructor(public accountService: AccountService,private router : Router,
    private toastr : ToastrService) { }

  ngOnInit() {
    //this.getCurrentUser();
    //this.currentUser$ = this.accountService.currentUser$
    //const userData = JSON.parse(localStorage.getItem('user'))
    
  };


  login() {
    this.accountService.login(this.model).subscribe({
      next: (reponse) => {
        this.router.navigateByUrl('/members');
        this.loggedIn = true;
        console.log(this.accountService.currentUser$);
        //console.log(this.accountService.currentUser$);  //testing purspose
      },
      // error: (e) => {
      //   console.log(e)
      //   this.toastr.error(e.error)
        
      // },
      complete: () => console.info("Login call completed")
    });
    //console.log(this.model);
  }


  logout() {
    this.accountService.logout();
    this.loggedIn = false;
    this.router.navigateByUrl('/');
    console.log("On logout oberserver value " + this.accountService.currentUser$);  //testing purspose 
  }

  // getCurrentUser() {
  //   this.accountService.currentUser$.subscribe({
  //     next: (user) => {
  //       this.loggedIn = !!(user && (user.username != null || undefined))
  //       console.log("User details found and username is " + user?.username)
  //     },
  //     error: (error) => console.log(error),
  //     complete: () => console.info("Current user set completed")
  //   });
  // }

}
