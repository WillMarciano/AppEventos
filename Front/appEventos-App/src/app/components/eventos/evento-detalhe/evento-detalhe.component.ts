import { Lote } from './../../../models/Lote';
import { LoteService } from './../../../services/lote.service';
import { EventoService } from '@app/services/evento.service';
import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Evento } from '@app/models/Evento';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss'],
})
export class EventoDetalheComponent implements OnInit {
  evento = {} as Evento;
  form!: FormGroup;
  eventoId: number;
  modoSalvar = 'post';

  get lotes(): FormArray {
    return this.form.get('lotes') as FormArray;
  }

  get f(): any {
    return this.form.controls;
  }

  get modoEditar(): boolean {
    return this.modoSalvar === 'put';
  }

  get bsConfig(): any {
    return {
      dateInputFormat: 'DD-MM-YYYY hh:mm A',
      isAnimated: true,
      adaptivePosition: true,
      containerClass: 'theme-default',
      showWeekNumbers: false,
    };
  }

  constructor(
    private fb: FormBuilder,
    private localeService: BsLocaleService,
    private activatedRouter: ActivatedRoute,
    private router: Router,
    private eventoService: EventoService,
    private lotesService: LoteService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService
  ) {
    this.localeService.use('pt-br');
  }

  ngOnInit(): void {
    this.carregarEvento();
    this.validation();
  }

  public validation(): void {
    this.form = this.fb.group({
      tema: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(50),
        ],
      ],
      local: ['', [Validators.required]],
      dataEvento: ['', [Validators.required]],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      telefone: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      imagemUrl: ['', [Validators.required]],
      lotes: this.fb.array([]),
    });
  }

  public cssValidator(campoValidacao: FormControl | AbstractControl): any {
    return { 'is-invalid': campoValidacao.errors && campoValidacao.touched };
  }

  public resetForm(): void {
    this.form.reset();
  }

  public carregarEvento(): void {
    this.eventoId = +this.activatedRouter.snapshot.paramMap.get('id');

    if (this.eventoId !== null && this.eventoId !== 0) {
      this.spinner.show();

      this.modoSalvar = 'put';

      this.eventoService.getEventoById(this.eventoId).subscribe({
        next: (evento: Evento) => {
          this.evento = { ...evento };
          this.form.patchValue(this.evento);
          // if (this.evento.imagemUrl !== '') {
          //   this.imagemUrl = environment.apiURL + 'resources/images/' + this.evento.imagemUrl;
          // }
          this.carregarLotes();
        },
        error: (error: any) => {
          this.spinner.hide();
          this.toastr.error('Erro ao tentar carregar o evento.', 'Erro');
          console.error(error);
        },
        complete: () => {
          this.spinner.hide();
        },
      });
    }
  }

  public salvarEvento(): void {
    this.spinner.show();
    if (this.form.valid) {
      this.evento =
        this.modoSalvar === 'post'
          ? { ...this.form.value }
          : { id: this.evento.id, ...this.form.value };

      this.eventoService[this.modoSalvar](this.evento).subscribe({
        next: () => {
          this.toastr.success('Evento Salvo com Sucesso!', 'Sucesso');
          this.modoSalvar = 'put';
        },
        error: (error: any) => {
          console.error(error);
          this.toastr.error('Erro ao tentar Salvar o evento.', 'Erro');
          this.spinner.hide();
        },
        complete: () => {
          this.spinner.hide();
        },
      });
    }
  }

  public carregarLotes(): void {
    this.lotesService
      .getLotesByEventoId(this.eventoId)
      .subscribe({
        next: (lotesRetorno: Lote[]) => {
          lotesRetorno.forEach((lote) => {
            this.lotes.push(this.criarLote(lote));
          });
        },
        error: (error: any) => {
          this.toastr.error('Erro ao tentar carregar lotes', 'Erro');
          console.error(error);
        },
      })
      .add(() => this.spinner.hide());
  }

  public adicionarLote(): void {
    this.lotes.push(this.criarLote({ id: 0 } as Lote));
  }

  criarLote(lote: Lote): FormGroup {
    return this.fb.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
      preco: [lote.preco, Validators.required],
      dataInicio: [lote.dataInicio],
      dataFim: [lote.dataFim],
    });
  }

  public salvarLotes(): void {
    if (this.form.controls['lotes'].valid) {
      this.spinner.show();
      this.lotesService
        .saveLote(this.eventoId, this.form.value.lotes)
        .subscribe({
          next: () => {
            this.toastr.success('Lotes Salvos com Sucesso!', 'Sucesso');
          },
          error: (error: any) => {
            console.error(error);
            this.toastr.error('Erro ao tentar Salvar os lotes', 'Erro');
          },
        })
        .add(() => this.spinner.hide());
    }
  }
}
