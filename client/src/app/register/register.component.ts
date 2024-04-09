import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  @Input() userFromHomeComponent: any;
  @Output() cancelRegister = new EventEmitter();
  model: any = {};

  ngOnInit(): void {

  }

  register() {
    console.log(this.model);
  }

  cancel() {
    this.cancelRegister.emit(false);
    console.log('cancelled');
  }
}
