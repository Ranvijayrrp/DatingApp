import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Member } from '../_models/member';


//console.log('Token:', JSON.parse(localStorage.getItem('user')).token);

const httpOptions = {
  headers : new HttpHeaders({
    Authorization: 'Bearer ' + JSON.parse(localStorage.getItem('user'))?.token
  }) 
}

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members : []

  constructor(private http : HttpClient) { }

  getMembers() {
    return this.http.get<Member[]>(this.baseUrl + 'users',httpOptions)

  }

  getMember(username){
    return this.http.get(this.baseUrl + 'users/' + username, httpOptions)
  }
}
