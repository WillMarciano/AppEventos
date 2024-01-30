import { Component, EventEmitter, Output } from '@angular/core';
import {
  AbstractControlOptions,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { UserUpdate } from '@app/models/identity/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { PalestranteService } from '@app/services/palestrante.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-perfil-detalhe',
  templateUrl: './perfil-detalhe.component.html',
  styleUrls: ['./perfil-detalhe.component.scss'],
})
export class PerfilDetalheComponent {
  @Output() changeFormValue = new EventEmitter();

  userUpdate = {} as UserUpdate;
  form: FormGroup;

  get f(): any {
    return this.form.controls;
  }

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private palestranteService: PalestranteService,
    private router: Router,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) {}

  ngOnInit(): void {
    this.validation();
    this.carregarUsuario();
    this.verificaForm();
  }

  onSubmit(): void {
    // Vai parar aqui se o form estiver inválido
    // if (this.form.invalid) return;
    this.atualizarUsuario();
  }

  private verificaForm(): void {
    this.form.valueChanges.subscribe((value) => {
      this.changeFormValue.emit(value);
    });
  }

  private carregarUsuario(): void {
    this.spinner.show();
    this.accountService
      .getUser()
      .subscribe({
        next: (userRetorno: UserUpdate) => {
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
        imagemUrl: [''],
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

  public atualizarUsuario() {
    this.userUpdate = { ...this.form.value };
    this.spinner.show();

    if (this.f.funcao.value === 'Palestrante') {
      this.palestranteService.post().subscribe({
        next: () => {
          this.toastr.success('Palestrante criado', 'Sucesso');
        },
        error: (error: any) => {
          this.toastr.error(
            `Não foi possível criar o Palestrante.${error.error},`,
            'Erro'
          );
          console.error(error);
        },
      });
    }

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
