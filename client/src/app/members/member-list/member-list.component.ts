import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.scss']
})
export class MemberListComponent implements OnInit {

  members : Member[]

  constructor(private memberService : MembersService) {}

  ngOnInit(){
    this.loadMembers();
  }

  loadMembers(){
      this.memberService.getMembers().subscribe(members => {
       // console.log(members);
        this.members = members
      })
  }

}
