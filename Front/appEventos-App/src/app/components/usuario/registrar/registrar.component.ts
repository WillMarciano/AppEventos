import { ValidatorField } from '@app/helpers/ValidatorField';
import {
  AbstractControlOptions,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Component } from '@angular/core';
import { AccountService } from './../../../services/account.service';
import { Router } from '@angular/router';
import { User } from '@app/models/identity/User';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-registrar',
  templateUrl: './registrar.component.html',
  styleUrls: ['./registrar.component.scss'],
})
export class RegistrarComponent {
  user = {} as User;
  form!: FormGroup;

  get f(): any {
    return this.form.controls;
  }
  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private router: Router,
    private toaster: ToastrService
  ) {}

  ngOnInit(): void {
    this.validation();
  }

  public validation(): void {
    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmarPassword'),
    };

    this.form = this.fb.group(
      {
        nome: ['', [Validators.required]],
        sobrenome: ['', [Validators.required]],
        email: ['', [Validators.required, Validators.email]],
        username: ['', [Validators.required]],
        password: ['', [Validators.required, Validators.minLength(4)]],
        confirmarPassword: ['', [Validators.required]],
        termos: ['', [Validators.required]],
      },
      formOptions
    );
  }

  register(): void {
    this.user = { ...this.form.value };
    this.accountService.register(this.user).subscribe({
      next: () => {
        this.router.navigateByUrl('/dashboard');
      },
      error: (error: any) => {
        this.toaster.error(error.error);
      },
    });
  }
}
