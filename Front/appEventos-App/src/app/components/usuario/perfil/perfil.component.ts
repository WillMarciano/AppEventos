import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { UserUpdate } from '@app/models/identity/UserUpdate';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.css'],
})
export class PerfilComponent implements OnInit {
  usuario = {} as UserUpdate;
  form: FormGroup;

  public get ehPalestrante(): boolean {
    return this.usuario.funcao === 'Palestrante';
  }

  constructor() {}

  ngOnInit(): void {}

  public getFormValue(usuario: UserUpdate): void {
    this.usuario = usuario;
  }

  get f(): any {
    return '';
  }
}
