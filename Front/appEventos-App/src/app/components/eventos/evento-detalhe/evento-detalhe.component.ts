import { Lote } from './../../../models/Lote';
import { LoteService } from './../../../services/lote.service';
import { EventoService } from '@app/services/evento.service';
import { Component, OnInit, TemplateRef } from '@angular/core';
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
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { DatePipe } from '@angular/common';
import { environment } from '@environments/environment';
import { DateTimeFormatPipe } from '@app/helpers/DateTimeFormat.pipe';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss'],
  providers: [DatePipe],
})
export class EventoDetalheComponent implements OnInit {
  modalRef: BsModalRef;
  eventoId: number;
  evento = {} as Evento;
  form: FormGroup;
  modoSalvar = 'post';
  loteAtual = { id: 0, nome: '', indice: 0 };
  imagemUrl = 'assets/img/upload.png';
  file: File;

  get modoEditar(): boolean {
    return this.modoSalvar === 'put';
  }

  get lotes(): FormArray {
    return this.form.get('lotes') as FormArray;
  }

  get f(): any {
    return this.form.controls;
  }

  get bsConfig(): any {
    return {
      adaptivePosition: true,
      dateInputFormat: 'DD-MM-YYYY hh:mm A',
      containerClass: 'theme-default',
      isAnimated: true,
      showWeekNumbers: false,
    };
  }

  get bsConfigLote(): any {
    return {
      adaptivePosition: true,
      dateInputFormat: 'DD-MM-YYYY',
      containerClass: 'theme-default',
      isAnimated: true,
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
    private toastr: ToastrService,
    private modalService: BsModalService,
    private datePipe: DatePipe
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
      imagemUrl: [''],
      lotes: this.fb.array([]),
    });
  }

  public cssValidator(campoValidacao: FormControl | AbstractControl): any {
    return { 'is-invalid': campoValidacao.errors && campoValidacao.touched };
  }

  public resetForm(): void {
    this.form.reset();
  }

  public mudarValorData(value: Date, indice: number, campo: string): void {
    this.lotes.value[indice][campo] = value;
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
          // this.evento.lotes.forEach(lote =>{
          //   this.lotes.push(this.criarLote(lote));
          // })
          if (this.evento.imagemUrl !== '') {
            this.imagemUrl =
              environment.apiURL + 'resources/images/' + this.evento.imagemUrl;
          }
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
        next: (eventoRetorno: Evento) => {
          this.toastr.success('Evento Salvo com Sucesso!', 'Sucesso');
          this.router.navigate([`eventos/detalhe/${eventoRetorno.id}`]);
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
          if (lotesRetorno !== null)
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
      dataInicio: [lote.dataInicio, Validators.required],
      dataFim: [lote.dataFim, Validators.required],
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

  public removerLote(template: TemplateRef<any>, indice: number): void {
    this.loteAtual.id = this.lotes.get(indice + '.id').value;
    this.loteAtual.nome = this.lotes.get(indice + '.nome').value;
    this.loteAtual.indice = indice;

    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
    // this.lotes.removeAt(indice);
  }

  public confirmDeleteLote(): void {
    this.modalRef.hide();
    this.spinner.show();

    this.lotesService
      .deleteLote(this.eventoId, this.loteAtual.id)
      .subscribe({
        next: () => {
          this.toastr.success('O Lote foi deletado com Sucesso.', 'Deletado');
          this.lotes.removeAt(this.loteAtual.indice);
        },
        error: (error: any) => {
          console.error(error);
          this.toastr.error(
            `Erro ao tentar deletar o evento ${this.loteAtual.id}`,
            'Erro'
          );
        },
      })
      .add(() => this.spinner.hide());
  }

  public declineDeleteLote(): void {
    this.modalRef.hide();
  }

  public retornaTituloLote(nome: string): string {
    return nome === null || nome === '' ? 'Nome do lote' : nome;
  }

  public onFileChange(ev: any): void {
    const reader = new FileReader();

    reader.onload = (event: any) => (this.imagemUrl = event.target.result);

    this.file = ev.target.files;
    reader.readAsDataURL(this.file[0]);

    this.uploadImagem();
  }

  uploadImagem(): void {
    this.spinner.show();
    this.eventoService
      .postUpload(this.eventoId, this.file)
      .subscribe({
        next: () => {
          this.carregarEvento();
          this.toastr.success('Imagem atualizada com sucesso', 'Sucesso');
        },
        error: (error: any) => {
          this.toastr.success('Erro ao fazer Upload de imagem', 'Erro');
          console.log(error);
        },
      })
      .add(() => this.spinner.hide());
  }
}
