import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable, switchMap } from 'rxjs';
import { User } from '../_models/user';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {

  model: any = {};
  loggedIn: boolean = false; 
  //currentUser$ = new Observable<User>;

  constructor(public accountService: AccountService) { }

  ngOnInit() {
    //this.getCurrentUser();
    //this.currentUser$ = this.accountService.currentUser$
    //const userData = JSON.parse(localStorage.getItem('user'));
    //this.model = userData;
    this.loggedIn = localStorage.getItem('user')?.length > 0;

  };


  login() {
    this.accountService.login(this.model).subscribe({
      next: (reponse) => {
        this.loggedIn = true;
        console.log(reponse);
        //console.log(this.accountService.currentUser$);  //testing purspose
      },
      error: (e) => console.log(e),
      complete: () => console.info("Login call completed")
    });
    //console.log(this.model);
  }


  logout() {
    this.accountService.logout();
    this.loggedIn = false;
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
