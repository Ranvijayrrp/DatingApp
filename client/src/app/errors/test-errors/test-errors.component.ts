import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-test-errors',
  templateUrl: './test-errors.component.html',
  styleUrls: ['./test-errors.component.scss']
})
export class TestErrorsComponent implements OnInit {
  baseUrl = 'https://localhost:7001/api/';
  validationError : string[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit(): void {

  }

  get404Error() {
    this.http.get(this.baseUrl + 'buggy/not-found').subscribe({
      next: (reponse) => {
        console.log(reponse);
      },
      error : (err) =>{
        console.error(err)
      }
    });
  }

  get500Error() {
    this.http.get(this.baseUrl + 'buggy/server-error').subscribe({
      next: (reponse) => {
        console.log(reponse);
      },
      error : (err) =>{
        console.error(err)
      }
    });
  }

  get401Error() {
    this.http.get(this.baseUrl + 'buggy/auth').subscribe({
      next: (reponse) => {
        console.log(reponse);
      },
      error : (err) =>{
        console.error(err)
      }
    });
  }

  post400ValidateError() {
    this.http.post(this.baseUrl + 'account/register',{}).subscribe({
      next: (reponse) => {
        console.log(reponse);
      },
      error : (err) =>{
        console.error(err)
        this.validationError = err;
        
      }
    });
  }

}
