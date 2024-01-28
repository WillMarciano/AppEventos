import { Component, OnInit } from '@angular/core';
import {
  FormGroup,
} from '@angular/forms';
import { UserUpdate } from '@app/models/identity/UserUpdate';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.css'],
})
export class PerfilComponent implements OnInit {
  userUpdate = {} as UserUpdate;
  form: FormGroup;


  constructor() {}

  ngOnInit(): void {}

  get f(): any {
    return '';
  }

}
