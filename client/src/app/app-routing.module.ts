import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { ListComponent } from './list/list.component';
import { MessagesComponent } from './messages/messages.component';
import{ canActivateTeam } from './_gaurds/auth.guard' 

const routes: Routes = [
  {path : '',component : HomeComponent},
  {
    path : '',
    runGuardsAndResolvers : 'always',
    canActivate : [canActivateTeam],
    children : [
      {path : 'members',component : MemberListComponent,canActivate: [canActivateTeam],},
      {path : 'members/:id',component : MemberDetailComponent},
      {path : 'lists',component : ListComponent},
      {path : 'message',component : MessagesComponent},
    ]
  },
  
  {path : '**',component : HomeComponent,pathMatch : 'full'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
