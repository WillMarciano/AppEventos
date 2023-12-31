import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { EventoService } from '@app/services/evento.service';
import { Evento } from '@app/models/Evento';
import { Router } from '@angular/router';
import { Lote } from '@app/models/Lote';
import { environment } from '@environments/environment';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss'],
})
export class EventoListaComponent {
  modalRef?: BsModalRef;
  public eventos: Evento[] = [];
  public lotes: Lote[] = [];
  public eventosFiltrados: Evento[] = [];
  public eventoId = 0;

  public larguraImagem = 150;
  public margemImagem = 2;
  public exibirImagem = true;
  private filtroListado = '';

  public get filtroLista(): string {
    return this.filtroListado;
  }

  public set filtroLista(value: string) {
    this.filtroListado = value;
    this.eventosFiltrados = this.filtroLista
      ? this.filtrarEventos(this.filtroLista)
      : this.eventos;
  }

  public filtrarEventos(filtrarPor: string): Evento[] {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      (evento: { tema: string; local: string }) =>
        evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1 ||
        evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    );
  }

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) {}

  public ngOnInit(): void {
    this.spinner.show();
    this.carregarEventos();
    // setTimeout(() => {
    //   /** spinner ends after 5 seconds */
    // this.spinner.hide();
    // }, 1000);
  }

  public alterImage(): void {
    this.exibirImagem = !this.exibirImagem;
  }

  public mostraImagem(imagemUrl: string): string {
    return (imagemUrl !== '' ) ? `${environment.apiURL}resources/images/${imagemUrl}` : 'assets/img/semImagem.png'
  }

  public carregarEventos(): void {
    const observer = {
      next: (_eventos: Evento[]) => {
        this.eventos = _eventos;
        this.eventosFiltrados = this.eventos;
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Erro ao carregar os Eventos.', 'Erro');
      },
      complete: () => this.spinner.hide(),
    };
    this.eventoService.getEvento().subscribe(observer);
  }
  openModal(event: any, template: TemplateRef<any>, eventoId: number): void {
    event.stopPropagation();
    this.eventoId = eventoId;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }
  confirm(): void {
    this.modalRef?.hide();
    this.spinner.show();
    this.eventoService
      .deleteEvento(this.eventoId)
      .subscribe({
        next: (result: any) => {
          if (result.message === 'Deletado') {
            this.toastr.success(
              'O Evento foi deletado com Sucesso.',
              'Excluido!'
            );
          }
          this.carregarEventos();
        },
        error: (error: any) => {
          console.error(error);
          this.toastr.error(
            `Erro ao tentar deletar o evento ${this.eventoId}`,
            'Erro'
          );
        },
      })
      .add(() => this.spinner.hide());
  }

  decline(): void {
    this.modalRef?.hide();
  }

  detalheEvento(id: number): void {
    this.router.navigate([`eventos/detalhe/${id}`]);
  }
}
