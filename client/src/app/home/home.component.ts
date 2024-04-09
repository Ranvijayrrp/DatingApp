import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit{

  registerMode = false;
  users : any;

  constructor(private http:HttpClient){

  };

  ngOnInit(): void {
  }

  registerToggle(){
   this.registerMode = !this.registerMode;
  }

  getUserDemo(){
    this.http.get('https://localhost:7001/api/users').subscribe(user=> this.users = user);
  }

  getUser(){
    this.http.get('https://localhost:7001/api/users').subscribe({
      next: (response) => {
        this.users = response;
      },
      error: (e) => console.error(e),
      complete: () => console.info('getting user details completed') 
      });
  };

  cancelRegisterMode(event : boolean){
    this.registerMode = event;
  }
}
