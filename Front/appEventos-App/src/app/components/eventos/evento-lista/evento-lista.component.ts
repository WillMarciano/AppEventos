import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { EventoService } from '@app/services/evento.service';
import { Evento } from '@app/models/Evento';
import { Router } from '@angular/router';
import { Lote } from '@app/models/Lote';
import { environment } from '@environments/environment';
import { PaginatedResult, Pagination } from '@app/models/Pagination';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss'],
})
export class EventoListaComponent {
  modalRef?: BsModalRef;
  public eventos: Evento[] = [];
  public lotes: Lote[] = [];
  public eventoId = 0;
  public pagination = {} as Pagination;

  public larguraImagem = 150;
  public margemImagem = 2;
  public exibirImagem = true;

  public filtrarEventos(evt: any): void {
    this.eventoService
      .getEvento(this.pagination.currentPage, this.pagination.itemsPerPage, evt.value)
      .subscribe({
        next: (paginatedResult: PaginatedResult<Evento[]>) => {
          this.eventos = paginatedResult.result;
          this.pagination = paginatedResult.pagination;
        },
        error: (error: any) => {
          this.spinner.hide();
          this.toastr.error('Erro ao carregar os Eventos.', 'Erro');
        },
      })
      .add(() => this.spinner.hide());
  }

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) {}

  public ngOnInit(): void {
    this.pagination = {
      currentPage: 1,
      itemsPerPage: 3,
      totalItems: 1,
    } as Pagination;
    this.spinner.show();
    this.carregarEventos();
  }

  public alterImage(): void {
    this.exibirImagem = !this.exibirImagem;
  }

  public mostraImagem(imagemUrl: string): string {
    return imagemUrl !== ''
      ? `${environment.apiURL}resources/images/${imagemUrl}`
      : 'assets/img/semImagem.png';
  }

  public carregarEventos(): void {
    this.spinner.show();

    this.eventoService
      .getEvento(this.pagination.currentPage, this.pagination.itemsPerPage)
      .subscribe({
        next: (paginatedResult: PaginatedResult<Evento[]>) => {
          this.eventos = paginatedResult.result;
          this.pagination = paginatedResult.pagination;
        },
        error: () => {
          this.spinner.hide();
          this.toastr.error('Erro ao carregar os Eventos.', 'Erro');
        },
      })
      .add(() => this.spinner.hide());
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

  public pageChanged(event): void {
    this.pagination.currentPage = event.page;
    this.carregarEventos();
  }
}
