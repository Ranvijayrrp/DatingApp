import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Dating App';
  users : any;

  constructor (private http : HttpClient,private accountService : AccountService) {}

  ngOnInit(){
    //this.getUser();
    this.setCurrentUser();
  }

  setCurrentUser(){
    const user : User = JSON.parse(localStorage.getItem('user') || "{}");
    this.accountService.setCurrentUser(user)
  }
  
}
