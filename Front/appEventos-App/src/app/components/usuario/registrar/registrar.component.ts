import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-registrar',
  templateUrl: './registrar.component.html',
  styleUrls: ['./registrar.component.scss'],
})
export class RegistrarComponent {
  form!: FormGroup;

  get f(): any {
    return this.form.controls;
  }
  constructor(private fb: FormBuilder) {}
  ngOnInit(): void {
    this.validation();
  }

  public validation(): void {
    this.form = this.fb.group({
      nome: ['', [Validators.required]],
      sobrenome: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      usuario: ['', [Validators.required]],
      senha: ['', [Validators.required]],
      confirmarSenha: ['', [Validators.required]],
      termos: ['', [Validators.required]],
    });
  }
}
