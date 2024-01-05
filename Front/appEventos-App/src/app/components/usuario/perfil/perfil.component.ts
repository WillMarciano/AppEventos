import { ToastrService } from 'ngx-toastr';
import { Component, OnInit } from '@angular/core';
import {
  AbstractControlOptions,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { AccountService } from '@app/services/account.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
import { UserUpdate } from '@app/models/identity/UserUpdate';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.css'],
})
export class PerfilComponent implements OnInit {
  [x: string]: any;
  userUpdate = {} as UserUpdate;
  form: FormGroup;

  get f(): any {
    return this.form.controls;
  }

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private router: Router,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) {}

  ngOnInit(): void {
    this.validation();
    this.CarregarUsuario();
  }



  private CarregarUsuario(): void {
    this.spinner.show();
    this.accountService
      .getUser()
      .subscribe({
        next: (userRetorno: UserUpdate) => {
          console.log(userRetorno);
          this.userUpdate = userRetorno;
          this.form.patchValue(this.userUpdate);
          // this.toastr.success('Usuário Carregado', 'Sucesso');
        },
        error: (error: any) => {
          console.error(error);
          this.toastr.error('Erro ao tentar carregar Usuário', 'Erro');
          this.router.navigate(['/dashboard']);
        },
      })
      .add(() => this.spinner.hide());
  }

  public validation(): void {
    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmarPassword'),
    };

    this.form = this.fb.group(
      {
        userName: [''],
        titulo: ['NaoInformado', Validators.required],
        nome: ['', Validators.required],
        sobrenome: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        phoneNumber: ['', [Validators.required]],
        descricao: ['', Validators.required],
        funcao: ['NaoInformado', Validators.required],
        password: ['', [Validators.minLength(4), Validators.nullValidator]],
        confirmarPassword: ['', Validators.nullValidator],
      },
      formOptions
    );
  }

  onSubmit(): void {
    // Vai parar aqui se o form estiver inválido
    // if (this.form.invalid) return;
    this.atualizarUsuario();
  }

  public atualizarUsuario() {
    this.userUpdate = { ...this.form.value };
    this.spinner.show();

    this.accountService
      .updateUser(this.userUpdate)
      .subscribe({
        next: () => {
          this.toastr.success('Usuário atualizado', 'Sucesso');
        },
        error: (error: any) => {
          this.toastr.error(error.error);
          console.error(error);
        },
      })
      .add(() => this.spinner.hide());
  }

  resetForm(event: any): void {
    event.preventDefault();
    this.form.reset();
  }
}
