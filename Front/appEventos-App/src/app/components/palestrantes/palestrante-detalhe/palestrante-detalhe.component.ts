import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Palestrante } from '@app/models/Palestrante';
import { PalestranteService } from '@app/services/palestrante.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { debounceTime, map, tap } from 'rxjs';

@Component({
  selector: 'app-palestrante-detalhe',
  templateUrl: './palestrante-detalhe.component.html',
  styleUrls: ['./palestrante-detalhe.component.scss'],
})
export class PalestranteDetalheComponent {
  public form!: FormGroup;
  public situacaoDoForm = '';
  public corDaDescricao = '';

  constructor(
    private fb: FormBuilder,
    public palestranteService: PalestranteService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) {}

  ngOnInit() {
    this.validation();
    this.verificaForm();
    this.carregarPalestrante();
  }

  private carregarPalestrante(): void {
    this.spinner.show();
    this.palestranteService
      .getPalestrante()
      .subscribe({
        next: (palestrante: Palestrante) => this.patchForm(palestrante),
        error: (error: any) => this.handleError(error),
      })
      .add(() => {
        this.spinner.hide();
      });
  }

  private patchForm(palestrante: Palestrante): void {
    this.form.patchValue(palestrante);
  }

  private handleError(error: any): void {
    this.toastr.error(
      `Erro ao tentar carregar Palestrante ${error.error}`,
      'Erro'
    );
  }

  private validation(): void {
    this.form = this.fb.group({
      miniCurriculo: [''],
    });
  }

  public get f(): any {
    return this.form.controls;
  }

  private verificaForm(): void {
    this.form.valueChanges
      .pipe(
        map(() => this.updateFormStatus()),
        debounceTime(1000),
        tap(() => this.spinner.show())
      )
      .subscribe(() => this.updatePalestrante());
  }

  private updateFormStatus(): void {
    this.situacaoDoForm = 'Minicurrículo está sendo Atualizado!';
    this.corDaDescricao = 'text-warning';
  }

  private updatePalestrante(): void {
    this.palestranteService
      .put(this.form.value)
      .subscribe({
        next: () => this.handleSuccess(),
        error: () => this.handleErrorOnUpdate(),
      })
      .add(() => this.spinner.hide());
  }

  private handleSuccess(): void {
    this.situacaoDoForm = 'Minicurrículo foi ataulizado!';
    this.corDaDescricao = 'text-success';

    setTimeout(() => {
      this.situacaoDoForm = 'Minicurrículo foi carregado!';
      this.corDaDescricao = 'text-muted';
    }, 2000);
  }

  private handleErrorOnUpdate(): void {
    this.toastr.error('Erro ao tentar atualizar Palestrante', 'Erro');
  }
}
