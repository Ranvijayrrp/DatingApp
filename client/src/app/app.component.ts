import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Dating App';
  users : any
  userArrays : any

  constructor (private http : HttpClient) {}

  ngOnInit(){
    this.getUser();
    ///this.userArrays = this.users.data;
    //alert(this.users.data);
  }

  getUser(){
    this.http.get('https://localhost:7001/api/users').subscribe({
      next: (response) => {
        this.users = response
        //console.log(this.users)
      },
      error: (e) => console.error(e),
      complete: () => console.info('complete') 
      });
  };
  
  
  
}
